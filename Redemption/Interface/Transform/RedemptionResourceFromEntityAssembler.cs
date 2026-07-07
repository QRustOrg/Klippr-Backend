using Klippr_Backend.Redemption.Application.OutboundServices.Signing;
using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Interface.Transform;

/// <summary>
/// Convierte agregados de canjes en recursos de respuesta HTTP.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Expone valores simples para la capa HTTP sin exponer detalles internos del agregado.
/// El codigo se serializa usando el formato sin guiones de <see cref="Klippr_Backend.Redemption.Domain.ValueObjects.RedemptionCode.ToString"/>.
/// </remarks>
public static class RedemptionResourceFromEntityAssembler
{
    /// <summary>
    /// Convierte un canje en un recurso de respuesta.
    /// </summary>
    /// <param name="redemption">Agregado de canje que sera expuesto.</param>
    /// <param name="tokenSigner">Firmador usado para calcular <see cref="RedemptionResource.TokenSignature"/>.</param>
    /// <returns>Recurso de respuesta de canje.</returns>
    public static RedemptionResource ToResource(RedemptionAggregate redemption, IRedemptionTokenSigner tokenSigner)
    {
        ArgumentNullException.ThrowIfNull(redemption);
        ArgumentNullException.ThrowIfNull(tokenSigner);

        return new RedemptionResource(
            redemption.Id,
            redemption.ConsumerId,
            redemption.BusinessId,
            redemption.PromotionId,
            redemption.Code.ToString(),
            redemption.UniqueToken,
            tokenSigner.Sign(redemption.UniqueToken),
            redemption.Status.ToString(),
            redemption.ValidationMethod.ToString(),
            redemption.DiscountAppliedAmount,
            redemption.GeneratedAt,
            redemption.ExpiresAt,
            redemption.RedeemedAt,
            redemption.BlockedAt
        );
    }

    /// <summary>
    /// Convierte una coleccion de canjes en recursos de respuesta.
    /// </summary>
    /// <param name="redemptions">Coleccion de canjes que sera expuesta.</param>
    /// <param name="tokenSigner">Firmador usado para calcular <see cref="RedemptionResource.TokenSignature"/>.</param>
    /// <returns>Coleccion de recursos de respuesta.</returns>
    public static IReadOnlyList<RedemptionResource> ToResources(IEnumerable<RedemptionAggregate> redemptions, IRedemptionTokenSigner tokenSigner)
    {
        ArgumentNullException.ThrowIfNull(redemptions);

        return redemptions
            .Select(redemption => ToResource(redemption, tokenSigner))
            .ToList();
    }
}
