using Domain.Aggregates;
using Domain.Commands;

namespace Domain.Services;

public interface IUserCommandService
{
    Task<User> SignInAsync(SignInCommand command, CancellationToken cancellationToken = default);
    Task<User> SignUpConsumerAsync(SignUpConsumerCommand command, CancellationToken cancellationToken = default);
    Task<User> SignUpBusinessAsync(SignUpBusinessCommand command, CancellationToken cancellationToken = default);
}
