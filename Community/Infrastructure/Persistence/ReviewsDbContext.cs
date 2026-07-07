using Klippr_Backend.Community.Domain.Aggregates;
using Klippr_Backend.Community.Infrastructure.Persistence.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Community.Infrastructure.Persistence;

public class ReviewsDbContext(DbContextOptions<ReviewsDbContext> options) : DbContext(options)
{
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyReviewsConfiguration();
    }
}
