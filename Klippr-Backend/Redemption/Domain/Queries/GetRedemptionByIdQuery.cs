namespace Klippr_Backend.Redemption.Domain.Queries;

/// <summary>
/// Query para obtener un canje por su identificador.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="RedemptionId">Identificador del canje consultado.</param>
public record GetRedemptionByIdQuery(int RedemptionId);
