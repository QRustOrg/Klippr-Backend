using Klippr_Backend.Profile.Domain.Aggregates;
using Klippr_Backend.Profile.Domain.Commands;

namespace Klippr_Backend.Profile.Domain.Services;

public interface IConsumerProfileCommandService
{
    Task<ConsumerProfile> CreateProfileAsync(CreateConsumerProfileCommand command, CancellationToken cancellationToken = default);
    Task<ConsumerProfile> UpdateProfileAsync(UpdateConsumerProfileCommand command, CancellationToken cancellationToken = default);
    Task<bool> DeactivateProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
}
