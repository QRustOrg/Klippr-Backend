using Klippr_Backend.Community.Domain.Aggregates;
using Klippr_Backend.Community.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Community.Infrastructure.Persistence.Repositories;

public class ReviewRepository(ReviewsDbContext dbContext) : IReviewRepository
{
    public Task<Review?> FindByIdAsync(Guid id) =>
        dbContext.Reviews
            .Include(review => review.Comments)
            .FirstOrDefaultAsync(review => review.Id == id);

    public async Task<IEnumerable<Review>> FindAllAsync(Guid? promotionId, Guid? userId)
    {
        var query = dbContext.Reviews.AsNoTracking().Include(review => review.Comments).AsQueryable();

        if (promotionId.HasValue)
            query = query.Where(review => review.PromotionId == promotionId.Value);

        if (userId.HasValue)
            query = query.Where(review => review.UserId == userId.Value);

        return await query.OrderByDescending(review => review.CreatedAt).ToListAsync().ConfigureAwait(false);
    }

    public Task<bool> ExistsByUserAndPromotionAsync(Guid userId, Guid promotionId) =>
        dbContext.Reviews.AnyAsync(review => review.UserId == userId && review.PromotionId == promotionId);

    public async Task AddAsync(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);
        await dbContext.Reviews.AddAsync(review).ConfigureAwait(false);
    }

    public void Update(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);
        dbContext.Reviews.Update(review);
    }

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
