namespace Klippr_Backend.Profile.Application.OutboundServices;

public interface IRatingAggregator
{
    Task<(double AverageRating, int TotalReviews)> GetAggregatedRatingAsync(Guid profileId, CancellationToken cancellationToken = default);
}
