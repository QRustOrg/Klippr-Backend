using Klippr_Backend.Setting.Domain.Model.Aggregate;
using Klippr_Backend.Setting.Interfaces.REST.Resources;

namespace Klippr_Backend.Setting.Interfaces.REST.Transform;

public static class PreferenceResourceFromEntityAssembler
{
    public static PreferenceResource ToResourceFromEntity(Preference entity) =>
        new PreferenceResource(
            entity.Id, 
            entity.UserId, 
            entity.DarkMode,
            entity.LanguageCode,
            entity.Timezone,
            entity.EmailNotifications,
            entity.PushNotifications,
            entity.SmsNotifications,
            entity.ProfileVisibility,
            entity.DataSharingConsent
        );
}