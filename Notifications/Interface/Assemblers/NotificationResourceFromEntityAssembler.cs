using Klippr_Backend.Notifications.Domain.Aggregates;
using Klippr_Backend.Notifications.Interface.Resources;

namespace Klippr_Backend.Notifications.Interface.Assemblers;

public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(NotificationItem entity) =>
        new(entity.Id,
            entity.NotificationId,
            entity.UserId,
            entity.Type,
            entity.Title,
            entity.Message,
            entity.RelatedId,
            entity.IsRead,
            entity.CreatedDate);
}