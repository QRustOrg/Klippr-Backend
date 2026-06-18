using Klippr_Backend.Setting.Domain.Model.Commands;
using Klippr_Backend.Setting.Interfaces.REST.Resources;

namespace Klippr_Backend.Setting.Interfaces.REST.Transform;

public static class CreatePreferenceCommandFromResourceAssembler
{
    public static CreatePreferenceCommand ToCommandFromResource(CreatePreferenceResource resource) =>
        new CreatePreferenceCommand(
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