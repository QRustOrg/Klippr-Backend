using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Queries;
using Klippr_Backend.IAM.Domain.Services;

namespace Klippr_Backend.IAM.Interface.Facade;

public class IamContextFacade
{
    private readonly IUserQueryService _userQueryService;

    public IamContextFacade(IUserQueryService userQueryService)
    {
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        var query = new GetUserByIdQuery { UserId = userId };
        return await _userQueryService.GetUserByIdAsync(query, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        var query = new GetUserByEmailQuery { Email = email };
        return await _userQueryService.GetUserByEmailAsync(query, cancellationToken);
    }

    public async Task<bool> UserExistsByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByIdAsync(userId, cancellationToken);
        return user != null && user.IsActive;
    }

    public async Task<bool> UserExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByEmailAsync(email, cancellationToken);
        return user != null && user.IsActive;
    }

    public async Task<string?> GetUserRoleAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByIdAsync(userId, cancellationToken);
        return user?.Role.Value;
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role cannot be null or empty.", nameof(role));

        var query = new GetUsersByRoleQuery { Role = role, PageNumber = pageNumber, PageSize = pageSize };
        return await _userQueryService.GetUsersByRoleAsync(query, cancellationToken);
    }
}
