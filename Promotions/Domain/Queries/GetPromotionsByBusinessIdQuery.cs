namespace Klippr_Backend.Promotions.Domain.Queries;

/// <summary>
/// Query para obtener las promociones asociadas a un negocio especifico.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="BusinessId">Identificador del negocio propietario de las promociones.</param>
public record GetPromotionsByBusinessIdQuery(Guid BusinessId);
