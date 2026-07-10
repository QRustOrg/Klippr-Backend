using Klippr_Backend.Notifications.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Notifications.Infrastructure.Persistence;

public static class ModelBuilderExtensions
{
    public static void ApplyNotificationsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<NotificationItem>().HasKey(n => n.Id);
        builder.Entity<NotificationItem>().Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<NotificationItem>().Property(n => n.NotificationId).IsRequired().HasMaxLength(36);
        builder.Entity<NotificationItem>().Property(n => n.UserId).IsRequired().HasMaxLength(36);
        builder.Entity<NotificationItem>().Property(n => n.Type).IsRequired().HasConversion<string>().HasMaxLength(40);
        builder.Entity<NotificationItem>().Property(n => n.Title).IsRequired().HasMaxLength(200);
        builder.Entity<NotificationItem>().Property(n => n.Message).IsRequired().HasMaxLength(500);
        builder.Entity<NotificationItem>().Property(n => n.RelatedId).HasMaxLength(36);
        builder.Entity<NotificationItem>().Property(n => n.IsRead).IsRequired();

        builder.Entity<NotificationItem>()
            .HasIndex(n => n.NotificationId)
            .IsUnique();

        builder.Entity<NotificationItem>()
            .HasIndex(n => n.UserId);
    }
}