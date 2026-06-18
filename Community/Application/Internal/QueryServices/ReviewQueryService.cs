using Klippr_Backend.Community.Domain.Model.Aggregate;
using Klippr_Backend.Community.Domain.Model.Queries;
using Klippr_Backend.Community.Domain.Repositories;
using Klippr_Backend.Community.Domain.Services;

namespace Klippr_Backend.Community.Application.Internal.QueryServices;

public class ReviewQueryService(IReviewRepository reviewRepository)
    : IReviewQueryServices
{
    public async Task<Review?> Handle(GetReviewByIdQuery query)
    {
        return await reviewRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<IEnumerable<Review>> Handle(GetAllReviewQuery query)
    {
        return await reviewRepository.ListAsync();
    }
}