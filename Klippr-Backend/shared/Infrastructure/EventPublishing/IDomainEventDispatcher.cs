using Klippr_Backend.Shared.Domain.Events;

namespace Klippr_Backend.Shared.Infrastructure.EventPublishing;

/// <summary>
/// Define el contrato para despachar eventos de dominio a sus handlers registrados.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Esta abstraccion evita que los agregados conozcan el mecanismo concreto de publicacion
/// o mensajeria usado por la aplicacion.
/// </remarks>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Publica un evento de dominio a los handlers disponibles.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio que sera despachado.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
