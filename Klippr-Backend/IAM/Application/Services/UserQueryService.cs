using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Queries;
using Klippr_Backend.IAM.Domain.Repositories;
using Klippr_Backend.IAM.Domain.Services;

namespace Klippr_Backend.IAM.Application.Services;

public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _repository;

    public UserQueryService(IUserRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<User?> GetUserByIdAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (query.UserId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(query.UserId));

        return await _repository.GetByIdAsync(query.UserId, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(GetUserByEmailQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (string.IsNullOrWhiteSpace(query.Email))
            throw new ArgumentException("Email is required.", nameof(query.Email));

        return await _repository.GetByEmailAsync(query.Email, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (query.PageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(query.PageNumber));

        if (query.PageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(query.PageSize));

        return await _repository.GetAllAsync(query.PageNumber, query.PageSize, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(GetUsersByRoleQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (string.IsNullOrWhiteSpace(query.Role))
            throw new ArgumentException("Role is required.", nameof(query.Role));

        if (query.PageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(query.PageNumber));

        if (query.PageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(query.PageSize));

        return await _repository.GetByRoleAsync(query.Role, query.PageNumber, query.PageSize, cancellationToken);
    }
}
