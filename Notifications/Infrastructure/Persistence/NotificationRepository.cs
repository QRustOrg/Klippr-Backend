using Klippr_Backend.Notifications.Domain.Aggregates;
using Klippr_Backend.Notifications.Domain.Repositories;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Notifications.Infrastructure.Persistence;

public class NotificationRepository(AppDbContext context)
    : BaseRepository<NotificationItem>(context), INotificationRepository
{
    public async Task<IEnumerable<NotificationItem>> FindByUserIdAsync(string userId, bool unreadOnly = false) =>
        await Context.Set<NotificationItem>()
            .Where(n => n.UserId == userId && (!unreadOnly || !n.IsRead))
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();

    public async Task<NotificationItem?> FindByNotificationIdAsync(string notificationId) =>
        await Context.Set<NotificationItem>()
            .FirstOrDefaultAsync(n => n.NotificationId == notificationId);

    public async Task<int> CountUnreadAsync(string userId) =>
        await Context.Set<NotificationItem>()
            .CountAsync(n => n.UserId == userId && !n.IsRead);

    public async Task MarkAllAsReadAsync(string userId)
    {
        var notifications = await Context.Set<NotificationItem>()
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var n in notifications)
            n.MarkAsRead();
    }
}