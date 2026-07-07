namespace Klippr_Backend.Promotions.Domain.Queries;

/// <summary>
/// Query para obtener todas las promociones sin filtrar por estado ni vigencia.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Devuelve el listado completo de promociones en cualquier estado (borrador, publicada, cancelada).
/// </remarks>
public record GetAllPromotionsQuery;
