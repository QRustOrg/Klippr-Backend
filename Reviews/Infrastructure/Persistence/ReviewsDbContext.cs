using Klippr_Backend.Reviews.Domain.Aggregates;
using Klippr_Backend.Reviews.Infrastructure.Persistence.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Reviews.Infrastructure.Persistence;

public class ReviewsDbContext(DbContextOptions<ReviewsDbContext> options) : DbContext(options)
{
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyReviewsConfiguration();
    }
}
