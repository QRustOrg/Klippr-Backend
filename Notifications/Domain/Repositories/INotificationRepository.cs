using Klippr_Backend.Notifications.Domain.Aggregates;
using Klippr_Backend.Shared.Domain.Repositories;

namespace Klippr_Backend.Notifications.Domain.Repositories;

public interface INotificationRepository : IBaseRepository<NotificationItem>
{
    Task<IEnumerable<NotificationItem>> FindByUserIdAsync(string userId, bool unreadOnly = false);
    Task<NotificationItem?>             FindByNotificationIdAsync(string notificationId);
    Task<int>                       CountUnreadAsync(string userId);
    Task                            MarkAllAsReadAsync(string userId);
}