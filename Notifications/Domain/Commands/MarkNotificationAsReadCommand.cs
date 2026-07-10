namespace Klippr_Backend.Notifications.Domain.Commands;

public record MarkNotificationAsReadCommand(string UserId, string NotificationId);