using System.ComponentModel.DataAnnotations;

namespace Klippr_Backend.Setting.Interfaces.REST.Resources;

public record UpdatePreferenceResource(
    [Required] string UserId,
    [Required] bool DarkMode,
    [Required] string LanguageCode,
    [Required] string Timezone,
    [Required] bool EmailNotifications,
    [Required] bool PushNotifications,
    [Required] bool SmsNotifications,
    [Required] string ProfileVisibility,
    [Required] bool DataSharingConsent);