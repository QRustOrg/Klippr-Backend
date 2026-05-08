using Klippr_Backend.Community.Domain.Model.Aggregate;
using Klippr_Backend.Community.Domain.Repositories;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Klippr_Backend.Community.Infrastructure.Persistence.EFC.Repositories;

public class ReviewRepository(AppDbContext context)
    : BaseRepository<Review>(context), IReviewRepository
{
    
}