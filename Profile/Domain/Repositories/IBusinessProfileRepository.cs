using Domain.Aggregates;

namespace Domain.Repositories;

public interface IBusinessProfileRepository
{
    Task<BusinessProfile?> GetByIdAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<BusinessProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BusinessProfile>> GetByVerificationStatusAsync(string status, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<BusinessProfile> AddAsync(BusinessProfile profile, CancellationToken cancellationToken = default);
    Task<BusinessProfile> UpdateAsync(BusinessProfile profile, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid profileId, CancellationToken cancellationToken = default);
}
