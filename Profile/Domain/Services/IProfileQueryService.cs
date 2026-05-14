using Klippr_Backend.Profile.Domain.Aggregates;
using Klippr_Backend.Profile.Domain.Queries;

namespace Klippr_Backend.Profile.Domain.Services;

public interface IProfileQueryService
{
    Task<ConsumerProfile?> GetConsumerProfileByUserIdAsync(GetConsumerProfileByUserIdQuery query, CancellationToken cancellationToken = default);
    Task<BusinessProfile?> GetBusinessProfileByUserIdAsync(GetBusinessProfileByUserIdQuery query, CancellationToken cancellationToken = default);
    Task<string?> GetVerificationStatusAsync(GetVerificationStatusQuery query, CancellationToken cancellationToken = default);
    Task<object?> GetBusinessRatingAsync(GetBusinessRatingQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<BusinessProfile>> GetProfilesWithVerificationPendingAsync(GetProfilesWithVerificationPendingQuery query, CancellationToken cancellationToken = default);
}
