using Domain.Aggregates;
using Domain.Commands;

namespace Domain.Services;

public interface IBusinessProfileCommandService
{
    Task<BusinessProfile> CreateProfileAsync(CreateBusinessProfileCommand command, CancellationToken cancellationToken = default);
    Task<BusinessProfile> UpdateProfileAsync(UpdateBusinessProfileCommand command, CancellationToken cancellationToken = default);
    Task<BusinessProfile> SubmitVerificationAsync(SubmitBusinessVerificationCommand command, CancellationToken cancellationToken = default);
    Task<BusinessProfile> ApproveVerificationAsync(ApproveBusinessVerificationCommand command, CancellationToken cancellationToken = default);
    Task<bool> DeactivateProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
}
