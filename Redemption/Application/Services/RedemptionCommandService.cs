using Klippr_Backend.Redemption.Domain.Commands;
using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Services;
using Klippr_Backend.Analytics.Interface.Facade;
using Klippr_Backend.Redemption.Domain.Exceptions;
using Klippr_Backend.Redemption.Domain.Repositories;
using Klippr_Backend.Redemption.Domain.Services;
using Klippr_Backend.Redemption.Domain.ValueObjects;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Application.Services;

/// <summary>
/// Implementa los casos de uso de escritura para el bounded context de canjes.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Coordina repositorio y agregado sin duplicar reglas de negocio. Las invariantes permanecen dentro de <see cref="RedemptionAggregate"/>.
/// </remarks>
public class RedemptionCommandService(
    IRedemptionRepository redemptionRepository,
    IPromotionQueryService promotionQueryService,
    IPromotionCommandService promotionCommandService,
    AnalyticsContextFacade analyticsContextFacade,
    int qrExpirationMinutes = 120) : IRedemptionCommandService
{
    /// <inheritdoc />
    public async Task<RedeemPromotionResult?> Handle(RedeemPromotionCommand command)
    {
        var now = DateTimeOffset.UtcNow;

        var promotion = await GetPromotionOrThrowAsync(command.PromotionId)
            .ConfigureAwait(false);

        // BusinessId y ExpiresAt no se confían del cliente: BusinessId se resuelve del dueño real
        // de la promoción y ExpiresAt se calcula server-side (antes venía del body del request).
        command = command with
        {
            BusinessId = promotion.BusinessId,
            ExpiresAt = now.AddMinutes(qrExpirationMinutes)
        };

        var existingRedemptions = await redemptionRepository
            .FindByConsumerAndPromotionAsync(command.ConsumerId, command.PromotionId)
            .ConfigureAwait(false);

        if (existingRedemptions.Any(IsFinalized))
            throw new RedemptionConflictException("Ya canjeaste esta promoción.");

        var activeRedemption = existingRedemptions
            .FirstOrDefault(redemption =>
                redemption.Status == RedemptionStatus.Generated &&
                !redemption.IsExpired(now));

        if (activeRedemption is not null)
            return new RedeemPromotionResult(activeRedemption, Created: false);

        foreach (var expiredRedemption in existingRedemptions.Where(redemption =>
                     redemption.Status == RedemptionStatus.Generated &&
                     redemption.IsExpired(now)))
        {
            expiredRedemption.Expire(now);
            redemptionRepository.Update(expiredRedemption);
        }

        await EnsurePromotionCanReceiveRedemptionAsync(promotion)
            .ConfigureAwait(false);

        var redemption = RedemptionAggregate.Create(command);

        await redemptionRepository
            .AddAsync(redemption)
            .ConfigureAwait(false);

        await redemptionRepository
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return new RedeemPromotionResult(redemption, Created: true);
    }

    /// <inheritdoc />
    public async Task<RedemptionAggregate?> Handle(ConfirmRedemptionCommand command, CancellationToken cancellationToken = default)
    {
        var redemption = await redemptionRepository
            .FindByIdAsync(command.RedemptionId)
            .ConfigureAwait(false);

        return await ConfirmAsync(
                redemption,
                command.BusinessId,
                command.ValidationMethod,
                command.ConfirmedAt,
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<RedemptionAggregate?> Handle(ConfirmRedemptionByTokenCommand command, CancellationToken cancellationToken = default)
    {
        var redemption = await redemptionRepository
            .FindByUniqueTokenAsync(command.UniqueToken)
            .ConfigureAwait(false);

        return await ConfirmAsync(
                redemption,
                command.BusinessId,
                command.ValidationMethod,
                command.ConfirmedAt,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<RedemptionAggregate?> ConfirmAsync(
        RedemptionAggregate? redemption,
        Guid businessId,
        RedemptionValidationMethod validationMethod,
        DateTimeOffset confirmedAt,
        CancellationToken cancellationToken)
    {
        if (redemption is null)
            return null;

        if (redemption.BusinessId != businessId)
            throw new InvalidOperationException("El negocio no esta autorizado para confirmar este canje.");

        if (!Guid.TryParse(redemption.PromotionId, out var promotionGuid))
            throw new ArgumentException("Promotion id must be a valid GUID.", nameof(redemption));

        // Único guard real contra el límite de canjes: UPDATE atómico condicional,
        // seguro bajo concurrencia sin transacciones ni locks explícitos.
        var slotConsumed = await promotionCommandService
            .TryConsumeRedemptionSlotAsync(promotionGuid, cancellationToken)
            .ConfigureAwait(false);

        if (!slotConsumed)
            throw new RedemptionConflictException("Esta promoción ya alcanzó el límite de canjes.");

        redemption.Confirm(confirmedAt, validationMethod);
        redemption.Block(confirmedAt);

        redemptionRepository.Update(redemption);

        await redemptionRepository
            .SaveChangesAsync()
            .ConfigureAwait(false);

        await analyticsContextFacade
            .RegisterRedemptionAsync(promotionGuid, redemption.BusinessId)
            .ConfigureAwait(false);

        return redemption;
    }

    private async Task<Promotion> GetPromotionOrThrowAsync(string promotionId)
    {
        if (!Guid.TryParse(promotionId, out var promotionGuid))
            throw new ArgumentException("Promotion id must be a valid GUID.", nameof(promotionId));

        var promotion = await promotionQueryService
            .GetByIdAsync(new GetPromotionByIdQuery(promotionGuid))
            .ConfigureAwait(false);

        if (promotion is null)
            throw new ArgumentException("Promoción no encontrada.", nameof(promotionId));

        return promotion;
    }

    private async Task EnsurePromotionCanReceiveRedemptionAsync(Promotion promotion)
    {
        if (promotion.RedemptionCap is not { } redemptionCap)
            return;

        var finalizedCount = await redemptionRepository
            .CountFinalizedByPromotionIdAsync(promotion.Id.ToString())
            .ConfigureAwait(false);

        if (finalizedCount >= redemptionCap)
            throw new RedemptionConflictException("Esta promoción ya alcanzó el límite de canjes.");
    }

    private static bool IsFinalized(RedemptionAggregate redemption) =>
        redemption.Status is RedemptionStatus.Redeemed or RedemptionStatus.Blocked;
}
