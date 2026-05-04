namespace Application.OutboundServices;

public interface IRatingAggregator
{
    Task<(double AverageRating, int TotalReviews)> GetAggregatedRatingAsync(Guid profileId, CancellationToken cancellationToken = default);
}
