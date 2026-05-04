using Domain.Aggregates;
using Domain.Queries;

namespace Domain.Services;

public interface IProfileQueryService
{
    Task<ConsumerProfile?> GetConsumerProfileByUserIdAsync(GetConsumerProfileByUserIdQuery query, CancellationToken cancellationToken = default);
    Task<BusinessProfile?> GetBusinessProfileByUserIdAsync(GetBusinessProfileByUserIdQuery query, CancellationToken cancellationToken = default);
    Task<string?> GetVerificationStatusAsync(GetVerificationStatusQuery query, CancellationToken cancellationToken = default);
    Task<object?> GetBusinessRatingAsync(GetBusinessRatingQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<BusinessProfile>> GetProfilesWithVerificationPendingAsync(GetProfilesWithVerificationPendingQuery query, CancellationToken cancellationToken = default);
}
