using Klippr_Backend.Notifications.Domain.Aggregates;
using Klippr_Backend.Notifications.Domain.Commands;
using Klippr_Backend.Notifications.Domain.Repositories;
using Klippr_Backend.Notifications.Domain.Services;
using Klippr_Backend.Shared.Domain.Repositories;

namespace Klippr_Backend.Notifications.Application.Services;

public class NotificationCommandService(
    INotificationRepository notificationRepository,
    IUnitOfWork             unitOfWork)
    : INotificationCommandService
{
    public async Task<NotificationItem?> Handle(CreateNotificationCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.UserId) ||
            string.IsNullOrWhiteSpace(command.Title)  ||
            string.IsNullOrWhiteSpace(command.Message))
            return null;

        var notification = new NotificationItem(command);
        try
        {
            await notificationRepository.AddAsync(notification);
            await unitOfWork.CompleteAsync();
            return notification;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NotificationCommandService] Create error: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> Handle(MarkNotificationAsReadCommand command)
    {
        var notification = await notificationRepository.FindByNotificationIdAsync(command.NotificationId);
        if (notification is null || !notification.BelongsToUser(command.UserId))
            return false;

        notification.MarkAsRead();
        try
        {
            notificationRepository.Update(notification);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NotificationCommandService] MarkAsRead error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Handle(MarkAllNotificationsAsReadCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.UserId)) return false;
        try
        {
            await notificationRepository.MarkAllAsReadAsync(command.UserId);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NotificationCommandService] MarkAllAsRead error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Handle(DeleteNotificationCommand command)
    {
        var notification = await notificationRepository.FindByNotificationIdAsync(command.NotificationId);
        if (notification is null || !notification.BelongsToUser(command.UserId))
            return false;

        try
        {
            notificationRepository.Remove(notification);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NotificationCommandService] Delete error: {ex.Message}");
            return false;
        }
    }
}