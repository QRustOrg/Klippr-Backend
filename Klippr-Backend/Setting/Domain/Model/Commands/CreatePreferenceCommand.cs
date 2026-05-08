namespace Klippr_Backend.Setting.Domain.Model.Commands;

public record CreatePreferenceCommand(string UserId, bool DarkMode, string LanguageCode, string Timezone, 
    bool EmailNotifications, bool PushNotifications, bool SmsNotifications, string ProfileVisibility, 
    bool DataSharingConsent);