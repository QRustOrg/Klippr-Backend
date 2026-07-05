namespace Klippr_Backend.Analytics.Domain.Commands;

/// <summary>Registra una única vista de campaña; a diferencia de <see cref="UpdateMetricsCommand"/>, no acepta un conteo arbitrario del cliente.</summary>
public record RegisterViewCommand(Guid CampaignId, Guid BusinessId);
