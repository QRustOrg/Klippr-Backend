namespace Klippr_Backend.Promotions.Domain.Queries;

/// <summary>
/// Query para obtener una promocion por su identificador unico.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="PromotionId">Identificador unico de la promocion consultada.</param>
public record GetPromotionByIdQuery(Guid PromotionId);
