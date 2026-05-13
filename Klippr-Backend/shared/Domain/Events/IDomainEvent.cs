namespace Klippr_Backend.Shared.Domain.Events;

/// <summary>
/// Define el contrato base para eventos de dominio emitidos por los bounded contexts.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este contrato pertenece al bounded context Shared porque representa una abstracción
/// transversal usada por agregados de distintos contextos.
/// </remarks>
public interface IDomainEvent
{
    /// <summary>
    /// Fecha y hora UTC en la que ocurrió el evento.
    /// </summary>
    DateTime OccurredOn { get; }
}
