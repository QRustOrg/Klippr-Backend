using Klippr_Backend.Community.Domain.Model.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Community.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyCommunityConfiguration(this ModelBuilder builder)
    {
        // Review Context
        builder.Entity<Review>().HasKey(r => r.Id);
        builder.Entity<Review>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Review>().Property(r => r.UserId).IsRequired().HasMaxLength(30);
        builder.Entity<Review>().Property(r => r.PromotionId).IsRequired().HasMaxLength(30);
        builder.Entity<Review>().Property(r => r.RedemptionId).IsRequired().HasMaxLength(30);
        builder.Entity<Review>().Property(r => r.Rating).IsRequired();
        builder.Entity<Review>().Property(r => r.Status).IsRequired().HasMaxLength(30);
        builder.Entity<Review>().Property(r => r.Comment).IsRequired().HasMaxLength(240);
        builder.Entity<Review>().Property(r => r.ReviewId).IsRequired().HasMaxLength(30);
        builder.Entity<Review>().Property(r => r.Content).IsRequired().HasMaxLength(240);
        builder.Entity<Review>().Property(r => r.BusinessId).IsRequired().HasMaxLength(30);
    }
}