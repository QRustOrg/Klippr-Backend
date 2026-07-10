namespace Klippr_Backend.Notifications.Domain.Commands;

public record DeleteNotificationCommand(string UserId, string NotificationId);