using Klippr_Backend.Redemption.Domain.Commands;
using Klippr_Backend.Redemption.Domain.ValueObjects;

namespace Klippr_Backend.Redemption.Interface.Transform;

/// <summary>
/// Convierte recursos de entrada HTTP en comandos de generacion de canje.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Parsea <see cref="RedemptionValidationMethod"/> desde cadena de texto.
/// Lanza <see cref="ArgumentException"/> si el valor no es reconocido.
/// </remarks>
public static class RedeemPromotionCommandFromResourceAssembler
{
    /// <summary>
    /// Convierte un recurso de entrada en un comando de redencion de promocion.
    /// </summary>
    /// <param name="resource">Datos de entrada del canje.</param>
    /// <returns>Comando listo para ser manejado por el servicio de aplicacion.</returns>
    public static RedeemPromotionCommand ToCommand(RedeemPromotionResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        if (!Enum.TryParse<RedemptionValidationMethod>(resource.ValidationMethod, ignoreCase: true, out var validationMethod))
            throw new ArgumentException(
                $"Metodo de validacion no reconocido: '{resource.ValidationMethod}'. Valores validos: QrScan, ManualCode.",
                nameof(resource));

        return new RedeemPromotionCommand(
            resource.ConsumerId,
            resource.BusinessId,
            resource.PromotionId,
            resource.ExpiresAt,
            resource.DiscountAppliedAmount,
            validationMethod
        );
    }
}
