using Klippr_Backend.IAM.Application.OutboundServices.Hashing;
using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Commands;
using Klippr_Backend.IAM.Domain.Repositories;
using Klippr_Backend.IAM.Domain.Services;
using Klippr_Backend.IAM.Domain.ValueObjects;

namespace Klippr_Backend.IAM.Application.Services;

public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _repository;
    private readonly IHashingService _hashingService;

    public UserCommandService(IUserRepository repository, IHashingService hashingService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
    }

    public async Task<User> SignInAsync(SignInCommand command, CancellationToken cancellationToken = default)
    {
        ValidateSignInCommand(command);

        var email = Email.Create(command.Email);
        var existingUser = await _repository.GetByEmailAsync(email, cancellationToken);

        if (existingUser == null)
            throw new InvalidOperationException("User not found.");

        if (!existingUser.IsActive)
            throw new InvalidOperationException("User account is inactive.");

        var passwordHash = _hashingService.Hash(command.Password);
        if (!existingUser.IsPasswordValid(passwordHash))
            throw new InvalidOperationException("Invalid credentials.");

        return existingUser;
    }

    public async Task<User> SignUpConsumerAsync(SignUpConsumerCommand command, CancellationToken cancellationToken = default)
    {
        ValidateSignUpConsumerCommand(command);

        var email = Email.Create(command.Email);

        var emailExists = await _repository.ExistsByEmailAsync(email, cancellationToken);
        if (emailExists)
            throw new InvalidOperationException("Email already registered.");

        var passwordHash = _hashingService.Hash(command.Password);
        var user = User.CreateConsumer(email, passwordHash, command.FirstName, command.LastName);

        var createdUser = await _repository.AddAsync(user, cancellationToken);
        return createdUser;
    }

    public async Task<User> SignUpBusinessAsync(SignUpBusinessCommand command, CancellationToken cancellationToken = default)
    {
        ValidateSignUpBusinessCommand(command);

        var email = Email.Create(command.Email);

        var emailExists = await _repository.ExistsByEmailAsync(email, cancellationToken);
        if (emailExists)
            throw new InvalidOperationException("Email already registered.");

        var passwordHash = _hashingService.Hash(command.Password);
        var user = User.CreateBusiness(email, passwordHash, command.BusinessName, command.TaxId);

        var createdUser = await _repository.AddAsync(user, cancellationToken);
        return createdUser;
    }

    private static void ValidateSignInCommand(SignInCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrWhiteSpace(command.Email))
            throw new ArgumentException("Email is required.", nameof(command.Email));

        if (string.IsNullOrWhiteSpace(command.Password))
            throw new ArgumentException("Password is required.", nameof(command.Password));

        if (command.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long.", nameof(command.Password));
    }

    private static void ValidateSignUpConsumerCommand(SignUpConsumerCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrWhiteSpace(command.Email))
            throw new ArgumentException("Email is required.", nameof(command.Email));

        if (string.IsNullOrWhiteSpace(command.Password))
            throw new ArgumentException("Password is required.", nameof(command.Password));

        if (command.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long.", nameof(command.Password));

        if (string.IsNullOrWhiteSpace(command.FirstName))
            throw new ArgumentException("First name is required.", nameof(command.FirstName));

        if (string.IsNullOrWhiteSpace(command.LastName))
            throw new ArgumentException("Last name is required.", nameof(command.LastName));
    }

    private static void ValidateSignUpBusinessCommand(SignUpBusinessCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrWhiteSpace(command.Email))
            throw new ArgumentException("Email is required.", nameof(command.Email));

        if (string.IsNullOrWhiteSpace(command.Password))
            throw new ArgumentException("Password is required.", nameof(command.Password));

        if (command.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long.", nameof(command.Password));

        if (string.IsNullOrWhiteSpace(command.BusinessName))
            throw new ArgumentException("Business name is required.", nameof(command.BusinessName));

        if (string.IsNullOrWhiteSpace(command.TaxId))
            throw new ArgumentException("Tax ID is required.", nameof(command.TaxId));
    }
}
