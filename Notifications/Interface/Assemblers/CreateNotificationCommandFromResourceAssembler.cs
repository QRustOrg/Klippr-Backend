using Klippr_Backend.Notifications.Domain.Commands;
using Klippr_Backend.Notifications.Interface.Resources;

namespace Klippr_Backend.Notifications.Interface.Assemblers;

public static class CreateNotificationCommandFromResourceAssembler
{
    public static CreateNotificationCommand ToCommandFromResource(CreateNotificationResource resource) =>
        new(resource.UserId,
            resource.Type,
            resource.Title,
            resource.Message,
            resource.RelatedId);
}