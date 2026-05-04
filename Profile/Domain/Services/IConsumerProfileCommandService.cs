using Domain.Aggregates;
using Domain.Commands;

namespace Domain.Services;

public interface IConsumerProfileCommandService
{
    Task<ConsumerProfile> CreateProfileAsync(CreateConsumerProfileCommand command, CancellationToken cancellationToken = default);
    Task<ConsumerProfile> UpdateProfileAsync(UpdateConsumerProfileCommand command, CancellationToken cancellationToken = default);
    Task<bool> DeactivateProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
}
