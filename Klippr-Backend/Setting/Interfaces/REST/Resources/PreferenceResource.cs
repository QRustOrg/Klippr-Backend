namespace Klippr_Backend.Setting.Interfaces.REST.Resources;

public record PreferenceResource(int Id, string UserId, bool DarkMode, string LanguageCode, string Timezone, 
    bool EmailNotifications, bool PushNotifications, bool SmsNotifications, string ProfileVisibility, 
    bool DataSharingConsent);