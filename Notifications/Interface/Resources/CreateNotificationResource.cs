using Klippr_Backend.Notifications.Domain.ValueObjects;

namespace Klippr_Backend.Notifications.Interface.Resources;

public record CreateNotificationResource(
    string           UserId,
    NotificationType Type,
    string           Title,
    string           Message,
    string?          RelatedId = null);