using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Queries;

namespace Klippr_Backend.IAM.Domain.Services;

public interface IUserQueryService
{
    Task<User?> GetUserByIdAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(GetUserByEmailQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllUsersAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersByRoleAsync(GetUsersByRoleQuery query, CancellationToken cancellationToken = default);
}
