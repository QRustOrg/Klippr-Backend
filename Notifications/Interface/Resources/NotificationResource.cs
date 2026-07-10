using Klippr_Backend.Notifications.Domain.ValueObjects;

namespace Klippr_Backend.Notifications.Interface.Resources;

public record NotificationResource(
    int              Id,
    string           NotificationId,
    string           UserId,
    NotificationType Type,
    string           Title,
    string           Message,
    string?          RelatedId,
    bool             IsRead,
    DateTimeOffset?  CreatedAt);