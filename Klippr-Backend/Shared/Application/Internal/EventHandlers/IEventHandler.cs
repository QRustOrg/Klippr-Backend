using Cortex.Mediator.Notifications;
using Klippr_Backend.Shared.Domain.Model.Events;

namespace Klippr_Backend.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
    
}