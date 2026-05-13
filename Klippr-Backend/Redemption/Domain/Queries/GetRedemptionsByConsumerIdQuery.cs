namespace Klippr_Backend.Redemption.Domain.Queries;

/// <summary>
/// Query para obtener los canjes asociados a un consumidor.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="ConsumerId">Identificador del consumidor consultado.</param>
public record GetRedemptionsByConsumerIdQuery(Guid ConsumerId);
