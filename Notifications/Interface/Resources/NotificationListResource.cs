namespace Klippr_Backend.Notifications.Interface.Resources;

public record NotificationListResource(
    string                              UserId,
    int                                 Count,
    int                                 UnreadCount,
    IReadOnlyList<NotificationResource> Items);