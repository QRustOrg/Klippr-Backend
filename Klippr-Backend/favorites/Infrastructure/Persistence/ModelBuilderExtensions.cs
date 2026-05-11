using Klippr_Backend.Favorites.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Favorites.Infrastructure.Persistence;

public static class ModelBuilderExtensions
{
    /// <summary>Maps the Favorite entity to its database table.</summary>
    public static void ApplyFavoritesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Favorite>().HasKey(f => f.Id);
        builder.Entity<Favorite>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Favorite>().Property(f => f.FavoriteId).IsRequired().HasMaxLength(36);
        builder.Entity<Favorite>().Property(f => f.UserId).IsRequired().HasMaxLength(36);
        builder.Entity<Favorite>().Property(f => f.PromotionId).IsRequired().HasMaxLength(36);

        // A user cannot favorite the same promotion twice
        builder.Entity<Favorite>()
            .HasIndex(f => new { f.UserId, f.PromotionId })
            .IsUnique();

        builder.Entity<Favorite>()
            .HasIndex(f => f.FavoriteId)
            .IsUnique();
    }

    /// <summary>
    /// Converts all table/column/key names to snake_case.
    /// Internalized copy of Shared's ModelBuilderExtensions — no Shared dependency.
    /// </summary>
    public static void UseSnakeCaseNamingConvention(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
                entity.SetTableName(tableName.ToPlural().ToSnakeCase());

            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.GetColumnName().ToSnakeCase());

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrEmpty(keyName)) key.SetName(keyName.ToSnakeCase());
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                var fkName = fk.GetConstraintName();
                if (!string.IsNullOrEmpty(fkName)) fk.SetConstraintName(fkName.ToSnakeCase());
            }

            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (!string.IsNullOrEmpty(indexName)) index.SetDatabaseName(indexName.ToSnakeCase());
            }
        }
    }
}
