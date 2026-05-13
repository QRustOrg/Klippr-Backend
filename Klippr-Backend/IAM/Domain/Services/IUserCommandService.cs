using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Commands;

namespace Klippr_Backend.IAM.Domain.Services;

public interface IUserCommandService
{
    Task<User> SignInAsync(SignInCommand command, CancellationToken cancellationToken = default);
    Task<User> SignUpConsumerAsync(SignUpConsumerCommand command, CancellationToken cancellationToken = default);
    Task<User> SignUpBusinessAsync(SignUpBusinessCommand command, CancellationToken cancellationToken = default);
}
