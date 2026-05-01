namespace Klippr_Backend.Promotions.Domain.Queries;

/// <summary>
/// Query para obtener las promociones actualmente activas.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Una promocion activa debe interpretarse desde la capa que ejecuta la consulta, normalmente considerando <estado publicado> y <vigencia temporal>.
/// </remarks>
public record GetActivePromotionsQuery;
