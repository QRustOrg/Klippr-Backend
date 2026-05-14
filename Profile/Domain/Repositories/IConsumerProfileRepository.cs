using Klippr_Backend.Profile.Domain.Aggregates;

namespace Klippr_Backend.Profile.Domain.Repositories;

public interface IConsumerProfileRepository
{
    Task<ConsumerProfile?> GetByIdAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<ConsumerProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ConsumerProfile> AddAsync(ConsumerProfile profile, CancellationToken cancellationToken = default);
    Task<ConsumerProfile> UpdateAsync(ConsumerProfile profile, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid profileId, CancellationToken cancellationToken = default);
}
