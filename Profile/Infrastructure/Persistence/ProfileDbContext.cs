using Klippr_Backend.Profile.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Profile.Infrastructure.Persistence;

public class ProfileDbContext : DbContext
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }

    public DbSet<ConsumerProfile> ConsumerProfiles { get; set; } = null!;
    public DbSet<BusinessProfile> BusinessProfiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureConsumerProfile(modelBuilder);
        ConfigureBusinessProfile(modelBuilder);
    }

    private static void ConfigureConsumerProfile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConsumerProfile>(entity =>
        {
            entity.ToTable("ConsumerProfiles");
            entity.HasKey(cp => cp.Id);

            entity.Property(cp => cp.Id)
                .HasColumnName("Id")
                .IsRequired();

            entity.Property(cp => cp.UserId)
                .HasColumnName("UserId")
                .IsRequired();

            entity.Property(cp => cp.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(cp => cp.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(cp => cp.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20);

            entity.Property(cp => cp.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            entity.Property(cp => cp.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .IsRequired();

            entity.Property(cp => cp.IsActive)
                .HasColumnName("IsActive")
                .IsRequired();

            entity.OwnsOne(cp => cp.Location, locationBuilder =>
            {
                locationBuilder.Property(l => l.Street).HasColumnName("LocationStreet").HasMaxLength(200);
                locationBuilder.Property(l => l.City).HasColumnName("LocationCity").HasMaxLength(100);
                locationBuilder.Property(l => l.State).HasColumnName("LocationState").HasMaxLength(100);
                locationBuilder.Property(l => l.Country).HasColumnName("LocationCountry").HasMaxLength(100);
                locationBuilder.Property(l => l.ZipCode).HasColumnName("LocationZipCode").HasMaxLength(20);
                locationBuilder.Property(l => l.Latitude).HasColumnName("LocationLatitude");
                locationBuilder.Property(l => l.Longitude).HasColumnName("LocationLongitude");
            });

            entity.OwnsOne(cp => cp.SavingsStatistics, savingsBuilder =>
            {
                savingsBuilder.Property(s => s.TotalSavings).HasColumnName("SavingsTotalSavings");
                savingsBuilder.Property(s => s.PromotionsUsed).HasColumnName("SavingsPromotionsUsed");
                savingsBuilder.Property(s => s.PromotionsSaved).HasColumnName("SavingsPromotionsSaved");
            });
        });
    }

    private static void ConfigureBusinessProfile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessProfile>(entity =>
        {
            entity.ToTable("BusinessProfiles");
            entity.HasKey(bp => bp.Id);

            entity.Property(bp => bp.Id)
                .HasColumnName("Id")
                .IsRequired();

            entity.Property(bp => bp.UserId)
                .HasColumnName("UserId")
                .IsRequired();

            entity.Property(bp => bp.BusinessName)
                .HasColumnName("BusinessName")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(bp => bp.TaxId)
                .HasColumnName("TaxId")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(bp => bp.Description)
                .HasColumnName("Description")
                .HasMaxLength(500);

            entity.Property(bp => bp.VerificationDocumentUrl)
                .HasColumnName("DocumentUrl")
                .HasMaxLength(500);

            entity.Property(bp => bp.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            entity.Property(bp => bp.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .IsRequired();

            entity.Property(bp => bp.IsActive)
                .HasColumnName("IsActive")
                .IsRequired();

            entity.OwnsOne(bp => bp.Category, categoryBuilder =>
            {
                categoryBuilder.Property(c => c.Value).HasColumnName("CategoryValue").HasMaxLength(100);
            });

            entity.OwnsOne(bp => bp.Location, locationBuilder =>
            {
                locationBuilder.Property(l => l.Street).HasColumnName("LocationStreet").HasMaxLength(200);
                locationBuilder.Property(l => l.City).HasColumnName("LocationCity").HasMaxLength(100);
                locationBuilder.Property(l => l.State).HasColumnName("LocationState").HasMaxLength(100);
                locationBuilder.Property(l => l.Country).HasColumnName("LocationCountry").HasMaxLength(100);
                locationBuilder.Property(l => l.ZipCode).HasColumnName("LocationZipCode").HasMaxLength(20);
                locationBuilder.Property(l => l.Latitude).HasColumnName("LocationLatitude");
                locationBuilder.Property(l => l.Longitude).HasColumnName("LocationLongitude");
            });

            entity.OwnsOne(bp => bp.VerificationStatus, statusBuilder =>
            {
                statusBuilder.Property(vs => vs.Value).HasColumnName("VerificationStatus").HasMaxLength(50).IsRequired();
            });

            entity.OwnsOne(bp => bp.Rating, ratingBuilder =>
            {
                ratingBuilder.Property(r => r.AverageRating).HasColumnName("RatingAverage");
                ratingBuilder.Property(r => r.TotalReviews).HasColumnName("RatingTotalReviews");
                ratingBuilder.Property(r => r.FiveStarCount).HasColumnName("RatingFiveStarCount");
                ratingBuilder.Property(r => r.FourStarCount).HasColumnName("RatingFourStarCount");
                ratingBuilder.Property(r => r.ThreeStarCount).HasColumnName("RatingThreeStarCount");
                ratingBuilder.Property(r => r.TwoStarCount).HasColumnName("RatingTwoStarCount");
                ratingBuilder.Property(r => r.OneStarCount).HasColumnName("RatingOneStarCount");
            });
        });
    }
}
