using Klippr_Backend.Setting.Domain.Model.Commands;

namespace Klippr_Backend.Setting.Domain.Model.Aggregate;

public partial class Preference
{
    private Preference()
    {
        UserId = string.Empty;
        DarkMode = false;
        LanguageCode = string.Empty;
        Timezone = string.Empty;
        EmailNotifications = false;
        PushNotifications = false;
        SmsNotifications = false;
        ProfileVisibility = string.Empty;
        DataSharingConsent = false;
    }
    
    private Preference(string userId, bool darkMode, string languageCode, string timezone,
        bool emailNotifications, bool pushNotifications, bool smsNotifications, string profileVisibility, 
        bool dataSharingConsent)
    {
        UserId = userId;
        DarkMode = darkMode;
        LanguageCode = languageCode;
        Timezone = timezone;
        EmailNotifications = emailNotifications;
        PushNotifications = pushNotifications;
        SmsNotifications = smsNotifications;
        ProfileVisibility = profileVisibility;
        DataSharingConsent = dataSharingConsent;
    }

    public Preference(CreatePreferenceCommand command)
    {
        UserId = command.UserId;
        DarkMode = false;
        LanguageCode = command.LanguageCode;
        Timezone = command.Timezone;
        EmailNotifications = true;
        PushNotifications = true;
        SmsNotifications = false;
        ProfileVisibility = command.ProfileVisibility;
        DataSharingConsent = false;
    }
    
    public void UpdatePreference(UpdatePreferenceCommand command)
    {
        UserId = command.UserId;
        DarkMode = false;
        LanguageCode = command.LanguageCode;
        Timezone = command.Timezone;
        EmailNotifications = true;
        PushNotifications = true;
        SmsNotifications = false;
        ProfileVisibility = command.ProfileVisibility;
        DataSharingConsent = false;
    }
    
    public int Id { get; private set; }
    public string UserId { get; private set; }
    public bool DarkMode { get; private set; }
    public string LanguageCode { get; private set; }
    public string Timezone { get; private set; }
    public bool EmailNotifications { get; private set; }
    public bool PushNotifications { get; private set; }
    public bool SmsNotifications { get; private set; }
    public string ProfileVisibility { get; private set; }
    public bool DataSharingConsent { get; private set; }
}