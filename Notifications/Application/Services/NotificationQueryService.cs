using Klippr_Backend.Notifications.Domain.Aggregates;
using Klippr_Backend.Notifications.Domain.Queries;
using Klippr_Backend.Notifications.Domain.Repositories;
using Klippr_Backend.Notifications.Domain.Services;

namespace Klippr_Backend.Notifications.Application.Services;

public class NotificationQueryService(INotificationRepository notificationRepository)
    : INotificationQueryService
{
    public async Task<IEnumerable<NotificationItem>> Handle(GetUserNotificationsQuery query) =>
        await notificationRepository.FindByUserIdAsync(query.UserId, query.UnreadOnly);

    public async Task<int> HandleUnreadCount(string userId) =>
        await notificationRepository.CountUnreadAsync(userId);
}