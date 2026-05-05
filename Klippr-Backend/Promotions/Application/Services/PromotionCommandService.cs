using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.Repositories;
using Klippr_Backend.Promotions.Domain.Services;

namespace Klippr_Backend.Promotions.Application.Services;

/// <summary>
/// Implementa los casos de uso de escritura para promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este servicio coordina repositorios y agregado sin duplicar reglas de negocio. Las invariantes permanecen dentro de <see cref="Promotion"/>.
/// </remarks>
public class PromotionCommandService(IPromotionRepository promotionRepository) : IPromotionCommandService
{
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(
        CreatePromotionCommand command,
        CancellationToken cancellationToken = default)
    {
        var promotion = Promotion.Create(command);

        await promotionRepository
            .AddAsync(promotion, cancellationToken)
            .ConfigureAwait(false);

        await promotionRepository
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return promotion.Id;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(
        UpdatePromotionCommand command,
        CancellationToken cancellationToken = default)
    {
        var promotion = await GetExistingPromotionAsync(command.PromotionId, cancellationToken)
            .ConfigureAwait(false);

        promotion.Update(command);

        await promotionRepository
            .UpdateAsync(promotion, cancellationToken)
            .ConfigureAwait(false);

        await promotionRepository
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task PublishAsync(
        PublishPromotionCommand command,
        CancellationToken cancellationToken = default)
    {
        var promotion = await GetExistingPromotionAsync(command.PromotionId, cancellationToken)
            .ConfigureAwait(false);

        promotion.Publish(command);

        await promotionRepository
            .UpdateAsync(promotion, cancellationToken)
            .ConfigureAwait(false);

        await promotionRepository
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task CancelAsync(
        CancelPromotionCommand command,
        CancellationToken cancellationToken = default)
    {
        var promotion = await GetExistingPromotionAsync(command.PromotionId, cancellationToken)
            .ConfigureAwait(false);

        promotion.Cancel(command);

        await promotionRepository
            .UpdateAsync(promotion, cancellationToken)
            .ConfigureAwait(false);

        await promotionRepository
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<Promotion> GetExistingPromotionAsync(
        Guid promotionId,
        CancellationToken cancellationToken)
    {
        return await promotionRepository
                   .GetByIdAsync(promotionId, cancellationToken)
                   .ConfigureAwait(false)
               ?? throw new KeyNotFoundException($"Promotion with id '{promotionId}' was not found.");
    }
}
