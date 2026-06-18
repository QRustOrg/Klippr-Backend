using Klippr_Backend.Setting.Domain.Model.Commands;
using Klippr_Backend.Setting.Interfaces.REST.Resources;

namespace Klippr_Backend.Setting.Interfaces.REST.Transform;

public static class UpdatePreferenceCommandFromResourceAssembler
{
    public static UpdatePreferenceCommand ToCommandFromResource(int id, UpdatePreferenceResource resource) =>
        new UpdatePreferenceCommand(
            id,
            resource.UserId, 
            resource.DarkMode,
            resource.LanguageCode,
            resource.Timezone,
            resource.EmailNotifications,
            resource.PushNotifications,
            resource.SmsNotifications,
            resource.ProfileVisibility,
            resource.DataSharingConsent
        );
}