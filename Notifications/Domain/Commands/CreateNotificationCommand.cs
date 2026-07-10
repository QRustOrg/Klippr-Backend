using Klippr_Backend.Notifications.Domain.ValueObjects;

namespace Klippr_Backend.Notifications.Domain.Commands;

public record CreateNotificationCommand(
    string           UserId,
    NotificationType Type,
    string           Title,
    string           Message,
    string?          RelatedId = null);