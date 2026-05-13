using Klippr_Backend.Redemption.Domain.Commands;
using Klippr_Backend.Redemption.Domain.ValueObjects;
using Klippr_Backend.Redemption.Interface.Resources;

namespace Klippr_Backend.Redemption.Interface.Assemblers;

/// <summary>
/// Convierte recursos de entrada HTTP en comandos de confirmacion de canje.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Parsea <see cref="RedemptionValidationMethod"/> desde cadena de texto.
/// Lanza <see cref="ArgumentException"/> si el valor no es reconocido.
/// </remarks>
public static class ConfirmRedemptionCommandFromResourceAssembler
{
    /// <summary>
    /// Convierte un recurso de entrada en un comando de confirmacion de canje.
    /// </summary>
    /// <param name="redemptionId">Identificador del canje que se desea confirmar.</param>
    /// <param name="resource">Datos de entrada de la confirmacion.</param>
    /// <returns>Comando listo para ser manejado por el servicio de aplicacion.</returns>
    public static ConfirmRedemptionCommand ToCommand(int redemptionId, ConfirmRedemptionResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        if (!Enum.TryParse<RedemptionValidationMethod>(resource.ValidationMethod, ignoreCase: true, out var validationMethod))
            throw new ArgumentException(
                $"Metodo de validacion no reconocido: '{resource.ValidationMethod}'. Valores validos: QrScan, ManualCode.",
                nameof(resource));

        return new ConfirmRedemptionCommand(
            redemptionId,
            resource.BusinessId,
            validationMethod,
            resource.ConfirmedAt
        );
    }
}
