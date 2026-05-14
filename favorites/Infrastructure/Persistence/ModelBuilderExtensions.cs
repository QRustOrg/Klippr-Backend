using Klippr_Backend.Favorites.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Favorites.Infrastructure.Persistence;

public static class ModelBuilderExtensions
{
    public static void ApplyFavoritesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Favorite>().HasKey(f => f.Id);
        builder.Entity<Favorite>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Favorite>().Property(f => f.FavoriteId).IsRequired().HasMaxLength(36);
        builder.Entity<Favorite>().Property(f => f.UserId).IsRequired().HasMaxLength(36);
        builder.Entity<Favorite>().Property(f => f.PromotionId).IsRequired().HasMaxLength(36);

        builder.Entity<Favorite>()
            .HasIndex(f => new { f.UserId, f.PromotionId })
            .IsUnique();

        builder.Entity<Favorite>()
            .HasIndex(f => f.FavoriteId)
            .IsUnique();
    }
}