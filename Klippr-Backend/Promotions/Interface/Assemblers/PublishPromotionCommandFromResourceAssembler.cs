using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Interface.Resources;

namespace Klippr_Backend.Promotions.Interface.Assemblers;

/// <summary>
/// Convierte recursos de publicacion en comandos de dominio para promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// El estado de verificacion de negocio llega de forma temporal desde el cuerpo de la solicitud.
/// </remarks>
public static class PublishPromotionCommandFromResourceAssembler
{
    /// <summary>
    /// Convierte un recurso de publicacion en un comando de dominio.
    /// </summary>
    /// <param name="promotionId">Identificador de la promocion recibido por ruta.</param>
    /// <param name="resource">Recurso HTTP con el estado de verificacion del negocio.</param>
    /// <returns>Comando de publicacion de promocion.</returns>
    public static PublishPromotionCommand ToCommand(Guid promotionId, PublishPromotionResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new PublishPromotionCommand(promotionId, resource.IsBusinessVerified);
    }
}
