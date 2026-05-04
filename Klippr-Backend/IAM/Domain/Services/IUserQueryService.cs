using Domain.Aggregates;
using Domain.Queries;

namespace Domain.Services;

public interface IUserQueryService
{
    Task<User?> GetUserByIdAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(GetUserByEmailQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllUsersAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersByRoleAsync(GetUsersByRoleQuery query, CancellationToken cancellationToken = default);
}
