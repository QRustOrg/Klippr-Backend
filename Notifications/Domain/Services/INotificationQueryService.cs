using Klippr_Backend.Notifications.Domain.Aggregates;
using Klippr_Backend.Notifications.Domain.Queries;

namespace Klippr_Backend.Notifications.Domain.Services;

public interface INotificationQueryService
{
    Task<IEnumerable<NotificationItem>> Handle(GetUserNotificationsQuery query);
    Task<int>                       HandleUnreadCount(string userId);
}