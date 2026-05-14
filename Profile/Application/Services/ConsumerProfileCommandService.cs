using Klippr_Backend.Profile.Application.OutboundServices;
using Klippr_Backend.Profile.Domain.Aggregates;
using Klippr_Backend.Profile.Domain.Commands;
using Klippr_Backend.Profile.Domain.Repositories;
using Klippr_Backend.Profile.Domain.Services;
using Klippr_Backend.Profile.Domain.ValueObjects;

namespace Klippr_Backend.Profile.Application.Services;

public class ConsumerProfileCommandService : IConsumerProfileCommandService
{
    private readonly IConsumerProfileRepository _repository;
    private readonly IEventPublisher _eventPublisher;

    public ConsumerProfileCommandService(IConsumerProfileRepository repository, IEventPublisher eventPublisher)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    public async Task<ConsumerProfile> CreateProfileAsync(CreateConsumerProfileCommand command, CancellationToken cancellationToken = default)
    {
        ValidateCreateCommand(command);

        var profile = ConsumerProfile.Create(command.UserId, command.FirstName, command.LastName, command.PhoneNumber);
        var createdProfile = await _repository.AddAsync(profile, cancellationToken);

        return createdProfile;
    }

    public async Task<ConsumerProfile> UpdateProfileAsync(UpdateConsumerProfileCommand command, CancellationToken cancellationToken = default)
    {
        ValidateUpdateCommand(command);

        var profile = await _repository.GetByIdAsync(command.ProfileId, cancellationToken);
        if (profile == null)
            throw new InvalidOperationException("Profile not found.");

        Location? location = null;
        if (!string.IsNullOrWhiteSpace(command.Street))
        {
            location = Location.Create(command.Street, command.City!, command.State!, command.Country!, command.ZipCode!);
        }

        profile.UpdateProfile(command.FirstName, command.LastName, command.PhoneNumber, location);
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

    private static void ValidateCreateCommand(CreateConsumerProfileCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (command.UserId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(command.UserId));
        if (string.IsNullOrWhiteSpace(command.FirstName))
            throw new ArgumentException("First name is required.", nameof(command.FirstName));
        if (string.IsNullOrWhiteSpace(command.LastName))
            throw new ArgumentException("Last name is required.", nameof(command.LastName));
    }

    private static void ValidateUpdateCommand(UpdateConsumerProfileCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));
        if (command.ProfileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(command.ProfileId));
        if (string.IsNullOrWhiteSpace(command.FirstName))
            throw new ArgumentException("First name is required.", nameof(command.FirstName));
        if (string.IsNullOrWhiteSpace(command.LastName))
            throw new ArgumentException("Last name is required.", nameof(command.LastName));
    }
}
