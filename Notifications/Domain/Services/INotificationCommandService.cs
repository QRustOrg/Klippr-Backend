using Klippr_Backend.Notifications.Domain.Aggregates;
using Klippr_Backend.Notifications.Domain.Commands;

namespace Klippr_Backend.Notifications.Domain.Services;

public interface INotificationCommandService
{
    Task<NotificationItem?> Handle(CreateNotificationCommand command);
    Task<bool>          Handle(MarkNotificationAsReadCommand command);
    Task<bool>          Handle(MarkAllNotificationsAsReadCommand command);
    Task<bool>          Handle(DeleteNotificationCommand command);
}