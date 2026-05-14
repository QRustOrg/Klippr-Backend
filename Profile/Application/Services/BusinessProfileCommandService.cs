using Klippr_Backend.Profile.Application.OutboundServices;
using Klippr_Backend.Profile.Domain.Aggregates;
using Klippr_Backend.Profile.Domain.Commands;
using Klippr_Backend.Profile.Domain.Repositories;
using Klippr_Backend.Profile.Domain.Services;
using Klippr_Backend.Profile.Domain.ValueObjects;

namespace Klippr_Backend.Profile.Application.Services;

public class BusinessProfileCommandService : IBusinessProfileCommandService
{
    private readonly IBusinessProfileRepository _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IVerificationService _verificationService;

    public BusinessProfileCommandService(
        IBusinessProfileRepository repository,
        IEventPublisher eventPublisher,
        IVerificationService verificationService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        _verificationService = verificationService ?? throw new ArgumentNullException(nameof(verificationService));
    }

    public async Task<BusinessProfile> CreateProfileAsync(CreateBusinessProfileCommand command, CancellationToken cancellationToken = default)
    {
        ValidateCreateCommand(command);

        var category = BusinessCategory.Create(command.Category);
        var profile = BusinessProfile.Create(command.UserId, command.BusinessName, command.TaxId, category);

        var createdProfile = await _repository.AddAsync(profile, cancellationToken);
        return createdProfile;
    }

    public async Task<BusinessProfile> UpdateProfileAsync(UpdateBusinessProfileCommand command, CancellationToken cancellationToken = default)
    {
        ValidateUpdateCommand(command);

        var profile = await _repository.GetByIdAsync(command.ProfileId, cancellationToken);
        if (profile == null)
            throw new InvalidOperationException("Profile not found.");

        var category = BusinessCategory.Create(command.Category);

        Location? location = null;
        if (!string.IsNullOrWhiteSpace(command.Street))
        {
            location = Location.Create(command.Street, command.City!, command.State!, command.Country!, command.ZipCode!);
        }

        profile.UpdateProfile(command.BusinessName, category, command.Description, location);
        var updatedProfile = await _repository.UpdateAsync(profile, cancellationToken);

        return updatedProfile;
    }

    public async Task<BusinessProfile> SubmitVerificationAsync(SubmitBusinessVerificationCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (command.ProfileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(command.ProfileId));
        if (string.IsNullOrWhiteSpace(command.DocumentUrl))
            throw new ArgumentException("Document URL is required.", nameof(command.DocumentUrl));

        var profile = await _repository.GetByIdAsync(command.ProfileId, cancellationToken);
        if (profile == null)
            throw new InvalidOperationException("Profile not found.");

        profile.SubmitVerification(command.DocumentUrl);
        var updatedProfile = await _repository.UpdateAsync(profile, cancellationToken);

        return updatedProfile;
    }

    public async Task<BusinessProfile> ApproveVerificationAsync(ApproveBusinessVerificationCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (command.ProfileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(command.ProfileId));

        var profile = await _repository.GetByIdAsync(command.ProfileId, cancellationToken);
        if (profile == null)
            throw new InvalidOperationException("Profile not found.");

        profile.ApproveVerification();
        var updatedProfile = await _repository.UpdateAsync(profile, cancellationToken);

        return updatedProfile;
    }

    public async Task<bool> DeactivateProfileAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        if (profileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(profileId));

        var profile = await _repository.GetByIdAsync(profileId, cancellationToken);
        if (profile == null)
            throw new InvalidOperationException("Profile not found.");

        profile.Deactivate();
        await _repository.UpdateAsync(profile, cancellationToken);

        return true;
    }

    private static void ValidateCreateCommand(CreateBusinessProfileCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (command.UserId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(command.UserId));
        if (string.IsNullOrWhiteSpace(command.BusinessName))
            throw new ArgumentException("Business name is required.", nameof(command.BusinessName));
        if (string.IsNullOrWhiteSpace(command.TaxId))
            throw new ArgumentException("Tax ID is required.", nameof(command.TaxId));
        if (string.IsNullOrWhiteSpace(command.Category))
            throw new ArgumentException("Category is required.", nameof(command.Category));
    }

    private static void ValidateUpdateCommand(UpdateBusinessProfileCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (command.ProfileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(command.ProfileId));
        if (string.IsNullOrWhiteSpace(command.BusinessName))
            throw new ArgumentException("Business name is required.", nameof(command.BusinessName));
        if (string.IsNullOrWhiteSpace(command.Category))
            throw new ArgumentException("Category is required.", nameof(command.Category));
    }
}
