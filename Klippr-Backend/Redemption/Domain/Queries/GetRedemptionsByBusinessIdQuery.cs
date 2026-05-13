namespace Klippr_Backend.Redemption.Domain.Queries;

/// <summary>
/// Query para obtener los canjes asociados a un negocio.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="BusinessId">Identificador del negocio consultado.</param>
public record GetRedemptionsByBusinessIdQuery(Guid BusinessId);
