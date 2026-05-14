using Klippr_Backend.Profile.Application.QueryServices;
using Klippr_Backend.Profile.Application.Services;
using Klippr_Backend.Profile.Domain.Aggregates;
using Klippr_Backend.Profile.Domain.Commands;
using Klippr_Backend.Profile.Domain.Queries;
using Klippr_Backend.Profile.Domain.Services;

namespace Klippr_Backend.Profile.Interface.Facade;

public class ProfileContextFacade
{
    private readonly IConsumerProfileCommandService _consumerCommandService;
    private readonly IBusinessProfileCommandService _businessCommandService;
    private readonly IProfileQueryService _queryService;

    public ProfileContextFacade(
        IConsumerProfileCommandService consumerCommandService,
        IBusinessProfileCommandService businessCommandService,
        IProfileQueryService queryService)
    {
        _consumerCommandService = consumerCommandService ?? throw new ArgumentNullException(nameof(consumerCommandService));
        _businessCommandService = businessCommandService ?? throw new ArgumentNullException(nameof(businessCommandService));
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
    }

    // Consumer Profile Operations
    public async Task<ConsumerProfile> CreateConsumerProfileAsync(CreateConsumerProfileCommand command, CancellationToken cancellationToken = default)
    {
        return await _consumerCommandService.CreateProfileAsync(command, cancellationToken);
    }

    public async Task<ConsumerProfile> UpdateConsumerProfileAsync(UpdateConsumerProfileCommand command, CancellationToken cancellationToken = default)
    {
        return await _consumerCommandService.UpdateProfileAsync(command, cancellationToken);
    }

    public async Task<ConsumerProfile?> GetConsumerProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = new GetConsumerProfileByUserIdQuery { UserId = userId };
        return await _queryService.GetConsumerProfileByUserIdAsync(query, cancellationToken);
    }

    // Business Profile Operations
    public async Task<BusinessProfile> CreateBusinessProfileAsync(CreateBusinessProfileCommand command, CancellationToken cancellationToken = default)
    {
        return await _businessCommandService.CreateProfileAsync(command, cancellationToken);
    }

    public async Task<BusinessProfile> UpdateBusinessProfileAsync(UpdateBusinessProfileCommand command, CancellationToken cancellationToken = default)
    {
        return await _businessCommandService.UpdateProfileAsync(command, cancellationToken);
    }

    public async Task<BusinessProfile?> GetBusinessProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = new GetBusinessProfileByUserIdQuery { UserId = userId };
        return await _queryService.GetBusinessProfileByUserIdAsync(query, cancellationToken);
    }

    public async Task<BusinessProfile> SubmitVerificationAsync(SubmitBusinessVerificationCommand command, CancellationToken cancellationToken = default)
    {
        return await _businessCommandService.SubmitVerificationAsync(command, cancellationToken);
    }

    public async Task<BusinessProfile> ApproveVerificationAsync(ApproveBusinessVerificationCommand command, CancellationToken cancellationToken = default)
    {
        return await _businessCommandService.ApproveVerificationAsync(command, cancellationToken);
    }

    // Query Operations
    public async Task<string?> GetVerificationStatusAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        var query = new GetVerificationStatusQuery { ProfileId = profileId };
        return await _queryService.GetVerificationStatusAsync(query, cancellationToken);
    }

    public async Task<object?> GetBusinessRatingAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        var query = new GetBusinessRatingQuery { ProfileId = profileId };
        return await _queryService.GetBusinessRatingAsync(query, cancellationToken);
    }

    public async Task<IEnumerable<BusinessProfile>> GetProfilesWithVerificationPendingAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetProfilesWithVerificationPendingQuery { PageNumber = pageNumber, PageSize = pageSize };
        return await _queryService.GetProfilesWithVerificationPendingAsync(query, cancellationToken);
    }
}
