using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Commands;

namespace Klippr_Backend.IAM.Domain.Services;

public interface IUserCommandService
{
    Task<User> SignInAsync(SignInCommand command, CancellationToken cancellationToken = default);
    Task<User> SignUpConsumerAsync(SignUpConsumerCommand command, CancellationToken cancellationToken = default);
    Task<User> SignUpBusinessAsync(SignUpBusinessCommand command, CancellationToken cancellationToken = default);
    Task<User> SignUpAdminAsync(SignUpAdminCommand command, CancellationToken cancellationToken = default);
    Task<User> DeactivateUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User> ActivateUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Paso 1 "olvidé mi contraseña": indica si existe un usuario con ese email.</summary>
    Task<bool> VerifyEmailExistsAsync(ForgotPasswordCommand command, CancellationToken cancellationToken = default);

    /// <summary>Paso 2 "olvidé mi contraseña": fija una nueva contraseña para el usuario identificado por email.</summary>
    Task<User> ResetPasswordAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default);
}
