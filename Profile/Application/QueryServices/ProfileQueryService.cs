using Klippr_Backend.Profile.Application.OutboundServices;
using Klippr_Backend.Profile.Domain.Aggregates;
using Klippr_Backend.Profile.Domain.Queries;
using Klippr_Backend.Profile.Domain.Repositories;
using Klippr_Backend.Profile.Domain.Services;

namespace Klippr_Backend.Profile.Application.QueryServices;

public class ProfileQueryService : IProfileQueryService
{
    private readonly IConsumerProfileRepository _consumerRepository;
    private readonly IBusinessProfileRepository _businessRepository;
    private readonly IRatingAggregator _ratingAggregator;

    public ProfileQueryService(
        IConsumerProfileRepository consumerRepository,
        IBusinessProfileRepository businessRepository,
        IRatingAggregator ratingAggregator)
    {
        _consumerRepository = consumerRepository ?? throw new ArgumentNullException(nameof(consumerRepository));
        _businessRepository = businessRepository ?? throw new ArgumentNullException(nameof(businessRepository));
        _ratingAggregator = ratingAggregator ?? throw new ArgumentNullException(nameof(ratingAggregator));
    }

    public async Task<ConsumerProfile?> GetConsumerProfileByUserIdAsync(GetConsumerProfileByUserIdQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (query.UserId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(query.UserId));

        return await _consumerRepository.GetByUserIdAsync(query.UserId, cancellationToken);
    }

    public async Task<BusinessProfile?> GetBusinessProfileByUserIdAsync(GetBusinessProfileByUserIdQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (query.UserId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(query.UserId));

        return await _businessRepository.GetByUserIdAsync(query.UserId, cancellationToken);
    }

    public async Task<string?> GetVerificationStatusAsync(GetVerificationStatusQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (query.ProfileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(query.ProfileId));

        var profile = await _businessRepository.GetByIdAsync(query.ProfileId, cancellationToken);
        return profile?.VerificationStatus.Value;
    }

    public async Task<object?> GetBusinessRatingAsync(GetBusinessRatingQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (query.ProfileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(query.ProfileId));

        var profile = await _businessRepository.GetByIdAsync(query.ProfileId, cancellationToken);
        if (profile == null)
            return null;

        return profile.Rating;
    }

    public async Task<IEnumerable<BusinessProfile>> GetProfilesWithVerificationPendingAsync(GetProfilesWithVerificationPendingQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));
        if (query.PageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(query.PageNumber));
        if (query.PageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(query.PageSize));

        return await _businessRepository.GetByVerificationStatusAsync("PENDING", query.PageNumber, query.PageSize, cancellationToken);
    }
}
