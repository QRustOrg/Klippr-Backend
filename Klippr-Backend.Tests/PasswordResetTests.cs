using Klippr_Backend.IAM.Application.Services;
using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Commands;
using Klippr_Backend.IAM.Domain.Repositories;
using Klippr_Backend.IAM.Domain.ValueObjects;
using Klippr_Backend.IAM.Infrastructure.Hashing;

namespace Klippr_Backend.Tests;

public class PasswordResetTests
{
    [Fact]
    public async Task ResetPassword_SucceedsWithCorrectAndUnexpiredCode()
    {
        var repository = new FakeUserRepository();
        var user = CreateUser(repository, "consumer@klippr.pe");
        var originalPasswordHash = user.PasswordHash;
        var service = new UserCommandService(repository, new HashingService());

        var requestResult = await service.RequestPasswordResetAsync(new ForgotPasswordCommand { Email = user.Email.Value });
        Assert.True(requestResult.UserExists);
        Assert.NotNull(requestResult.Code);

        var updatedUser = await service.ResetPasswordAsync(new ResetPasswordCommand
        {
            Email = user.Email.Value,
            Code = requestResult.Code!,
            NewPassword = "NewPassword123"
        });

        Assert.NotEqual(originalPasswordHash, updatedUser.PasswordHash);
        Assert.Null(updatedUser.PasswordResetCodeHash);
    }

    [Fact]
    public async Task ResetPassword_RejectsIncorrectCode()
    {
        var repository = new FakeUserRepository();
        var user = CreateUser(repository, "consumer2@klippr.pe");
        var service = new UserCommandService(repository, new HashingService());

        await service.RequestPasswordResetAsync(new ForgotPasswordCommand { Email = user.Email.Value });

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ResetPasswordAsync(new ResetPasswordCommand
        {
            Email = user.Email.Value,
            Code = "000000",
            NewPassword = "NewPassword123"
        }));
    }

    [Fact]
    public async Task ResetPassword_RejectsExpiredCode()
    {
        var repository = new FakeUserRepository();
        var user = CreateUser(repository, "consumer3@klippr.pe");
        var hashingService = new HashingService();
        var service = new UserCommandService(repository, hashingService);

        var requestResult = await service.RequestPasswordResetAsync(new ForgotPasswordCommand { Email = user.Email.Value });
        user.SetPasswordResetCode(hashingService.Hash(requestResult.Code!), DateTime.UtcNow.AddMinutes(-1));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ResetPasswordAsync(new ResetPasswordCommand
        {
            Email = user.Email.Value,
            Code = requestResult.Code!,
            NewPassword = "NewPassword123"
        }));
    }

    [Fact]
    public async Task ResetPassword_RejectsReusedCode()
    {
        var repository = new FakeUserRepository();
        var user = CreateUser(repository, "consumer4@klippr.pe");
        var service = new UserCommandService(repository, new HashingService());

        var requestResult = await service.RequestPasswordResetAsync(new ForgotPasswordCommand { Email = user.Email.Value });
        await service.ResetPasswordAsync(new ResetPasswordCommand
        {
            Email = user.Email.Value,
            Code = requestResult.Code!,
            NewPassword = "NewPassword123"
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ResetPasswordAsync(new ResetPasswordCommand
        {
            Email = user.Email.Value,
            Code = requestResult.Code!,
            NewPassword = "AnotherPassword456"
        }));
    }

    private static User CreateUser(FakeUserRepository repository, string email)
    {
        var user = User.CreateConsumer(Email.Create(email), "original-hash", "Ana", "Perez");
        repository.Users[user.Id] = user;
        return user;
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public Dictionary<Guid, User> Users { get; } = [];

        public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
            Task.FromResult(Users.GetValueOrDefault(userId));

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
            Task.FromResult(Users.Values.FirstOrDefault(u => u.Email.Value == email));

        public Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default) =>
            Task.FromResult<IEnumerable<User>>(Users.Values);

        public Task<IEnumerable<User>> GetByRoleAsync(string role, int pageNumber, int pageSize, CancellationToken cancellationToken = default) =>
            Task.FromResult<IEnumerable<User>>(Users.Values.Where(u => u.Role.Value == role));

        public Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            Users[user.Id] = user;
            return Task.FromResult(user);
        }

        public Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            Users[user.Id] = user;
            return Task.FromResult(user);
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
            Task.FromResult(Users.Values.Any(u => u.Email.Value == email));

        public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(Users.Count);
    }
}
