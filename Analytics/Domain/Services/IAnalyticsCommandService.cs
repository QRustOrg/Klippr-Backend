using Klippr_Backend.Analytics.Domain.Commands;

namespace Klippr_Backend.Analytics.Domain.Services;


public interface IAnalyticsCommandService
{
    /// <remarks>Uso interno (p. ej. <see cref="Klippr_Backend.Analytics.Interface.Facade.AnalyticsContextFacade"/>): no exponer directamente a clientes HTTP, ya que acepta conteos arbitrarios.</remarks>
    Task Handle(UpdateMetricsCommand command);
    Task Handle(RegisterViewCommand command);
    Task Handle(RegisterAbuseReportCommand command);
    Task Handle(UpdateAbuseReportStatusCommand command);
}