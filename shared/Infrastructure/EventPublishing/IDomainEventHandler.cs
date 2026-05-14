using Klippr_Backend.Shared.Domain.Events;

namespace Klippr_Backend.Shared.Infrastructure.EventPublishing;

/// <summary>
/// Define el contrato para manejar un evento de dominio especifico.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <typeparam name="TEvent">Tipo concreto del evento de dominio que sera procesado.</typeparam>
/// <remarks>
/// Los handlers permiten que otros componentes reaccionen a eventos sin acoplarse
/// directamente al agregado que los produjo.
/// </remarks>
public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    /// <summary>
    /// Procesa un evento de dominio publicado por un agregado.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio que debe ser procesado.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
