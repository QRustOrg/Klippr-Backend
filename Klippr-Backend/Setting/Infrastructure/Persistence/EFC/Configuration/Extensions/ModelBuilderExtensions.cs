using Klippr_Backend.Setting.Domain.Model.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Setting.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySettingConfiguration(this ModelBuilder builder)
    {
        // Preference Context
        builder.Entity<Preference>().HasKey(r => r.Id);
        builder.Entity<Preference>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Preference>().Property(r => r.UserId).IsRequired().HasMaxLength(30);
        builder.Entity<Preference>().Property(r => r.DarkMode).IsRequired();
        builder.Entity<Preference>().Property(r => r.LanguageCode).IsRequired().HasMaxLength(30);
        builder.Entity<Preference>().Property(r => r.Timezone).IsRequired();
        builder.Entity<Preference>().Property(r => r.EmailNotifications).IsRequired();
        builder.Entity<Preference>().Property(r => r.PushNotifications).IsRequired();
        builder.Entity<Preference>().Property(r => r.SmsNotifications).IsRequired();
        builder.Entity<Preference>().Property(r => r.ProfileVisibility).IsRequired().HasMaxLength(30);
        builder.Entity<Preference>().Property(r => r.DataSharingConsent).IsRequired();
    }
}