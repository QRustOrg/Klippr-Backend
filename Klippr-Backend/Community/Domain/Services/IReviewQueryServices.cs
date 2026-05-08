using Klippr_Backend.Community.Domain.Model.Aggregate;
using Klippr_Backend.Community.Domain.Model.Queries;

namespace Klippr_Backend.Community.Domain.Services;

public interface IReviewQueryServices
{
    Task<IEnumerable<Review>> Handle(GetAllReviewQuery query);
    Task<Review?> Handle(GetReviewByIdQuery query);
}