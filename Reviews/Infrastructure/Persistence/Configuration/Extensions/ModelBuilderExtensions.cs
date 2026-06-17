using System.Text.Json;
using Klippr_Backend.Reviews.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Klippr_Backend.Reviews.Infrastructure.Persistence.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyReviewsConfiguration(this ModelBuilder builder)
    {
        var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset, long>(
            value => value.ToUnixTimeMilliseconds(),
            value => DateTimeOffset.FromUnixTimeMilliseconds(value));

        var likedByUserIdsConverter = new ValueConverter<List<Guid>, string>(
            value => JsonSerializer.Serialize(value, (JsonSerializerOptions?)null),
            value => JsonSerializer.Deserialize<List<Guid>>(value, (JsonSerializerOptions?)null) ?? new List<Guid>());

        var likedByUserIdsComparer = new ValueComparer<List<Guid>>(
            (left, right) => (left ?? new List<Guid>()).SequenceEqual(right ?? new List<Guid>()),
            value => value.Aggregate(0, (hash, id) => HashCode.Combine(hash, id)),
            value => value.ToList());

        builder.Entity<Review>(review =>
        {
            review.ToTable("reviews");

            review.HasKey(entity => entity.Id);

            review.Property(entity => entity.PromotionId).IsRequired();
            review.Property(entity => entity.UserId).IsRequired();
            review.Property(entity => entity.Rating).IsRequired();
            review.Property(entity => entity.Comment).HasMaxLength(2000).IsRequired();
            review.Property(entity => entity.CreatedAt).HasConversion(dateTimeOffsetConverter).IsRequired();

            review.Property(typeof(List<Guid>), "_likedByUserIds")
                .HasConversion(likedByUserIdsConverter, likedByUserIdsComparer)
                .HasColumnName("liked_by_user_ids")
                .IsRequired();

            review.HasMany(entity => entity.Comments)
                .WithOne()
                .HasForeignKey("ReviewId")
                .OnDelete(DeleteBehavior.Cascade);

            review.HasIndex(entity => entity.PromotionId);
            review.HasIndex(entity => entity.UserId);
            review.HasIndex(entity => new { entity.PromotionId, entity.UserId }).IsUnique();
        });

        builder.Entity<ReviewComment>(comment =>
        {
            comment.ToTable("review_comments");

            comment.HasKey(entity => entity.Id);

            comment.Property(entity => entity.UserId).IsRequired();
            comment.Property(entity => entity.Comment).HasMaxLength(1000).IsRequired();
            comment.Property(entity => entity.CreatedAt).HasConversion(dateTimeOffsetConverter).IsRequired();

            comment.HasIndex("ReviewId");
        });
    }
}
