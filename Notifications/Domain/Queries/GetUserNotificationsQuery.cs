namespace Klippr_Backend.Notifications.Domain.Queries;

public record GetUserNotificationsQuery(string UserId, bool UnreadOnly = false);