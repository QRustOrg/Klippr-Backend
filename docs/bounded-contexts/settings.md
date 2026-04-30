# Settings Bounded Context

## Propósito

Gestiona preferencias del usuario: notificaciones, privacidad, tema visual, idioma y zona horaria.

## Source tree propuesto

```text
Settings/
├── Settings.Domain/
│   ├── Aggregates/
│   │   └── UserSettings.cs
│   ├── Commands/
│   │   ├── UpdateNotificationSettingsCommand.cs
│   │   ├── UpdatePrivacySettingsCommand.cs
│   │   ├── UpdateThemeCommand.cs
│   │   ├── UpdateLanguageCommand.cs
│   │   └── UpdateTimezoneCommand.cs
│   ├── Queries/
│   │   └── GetUserSettingsQuery.cs
│   ├── ValueObjects/
│   │   ├── NotificationPreferences.cs
│   │   ├── PrivacySettings.cs
│   │   ├── ThemePreference.cs
│   │   ├── LanguagePreference.cs
│   │   └── TimezonePreference.cs
│   ├── Services/
│   │   ├── IUserSettingsCommandService.cs
│   │   └── IUserSettingsQueryService.cs
│   └── Repositories/
│       └── IUserSettingsRepository.cs
├── Settings.Application/
│   └── Services/
│       ├── UserSettingsCommandService.cs
│       └── UserSettingsQueryService.cs
├── Settings.Infrastructure/
│   └── Persistence/
│       └── UserSettingsRepository.cs
└── Settings.Interface/
    ├── Controllers/
    │   └── SettingsController.cs
    ├── Resources/
    │   ├── UserSettingsResource.cs
    │   └── UpdateSettingsResource.cs
    ├── Assemblers/
    │   ├── UserSettingsResourceFromEntityAssembler.cs
    │   └── UpdateSettingsCommandFromResourceAssembler.cs
    └── Facade/
        └── SettingsContextFacade.cs
```
