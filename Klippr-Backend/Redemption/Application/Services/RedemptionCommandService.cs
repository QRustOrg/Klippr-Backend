using Klippr_Backend.Redemption.Domain.Commands;
using Klippr_Backend.Redemption.Domain.Repositories;
using Klippr_Backend.Redemption.Domain.Services;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Application.Services;

/// <summary>
/// Implementa los casos de uso de escritura para el bounded context de canjes.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Coordina repositorio y agregado sin duplicar reglas de negocio. Las invariantes permanecen dentro de <see cref="RedemptionAggregate"/>.
/// </remarks>
public class RedemptionCommandService(IRedemptionRepository redemptionRepository) : IRedemptionCommandService
{
    /// <inheritdoc />
    public async Task<RedemptionAggregate?> Handle(RedeemPromotionCommand command)
    {
        var redemption = RedemptionAggregate.Create(command);

        await redemptionRepository
            .AddAsync(redemption)
            .ConfigureAwait(false);

        await redemptionRepository
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return redemption;
    }

    /// <inheritdoc />
    public async Task<RedemptionAggregate?> Handle(ConfirmRedemptionCommand command)
    {
        var redemption = await redemptionRepository
            .FindByIdAsync(command.RedemptionId)
            .ConfigureAwait(false);

        if (redemption is null)
            return null;

        if (redemption.BusinessId != command.BusinessId)
            throw new InvalidOperationException("El negocio no esta autorizado para confirmar este canje.");

        redemption.Confirm(command.ConfirmedAt, command.ValidationMethod);

        redemptionRepository.Update(redemption);

        await redemptionRepository
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return redemption;
    }
}
