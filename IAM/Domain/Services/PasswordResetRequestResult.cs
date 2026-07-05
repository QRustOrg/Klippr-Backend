namespace Klippr_Backend.IAM.Domain.Services;

/// <summary>Resultado de solicitar un reset de contraseña: si el usuario existe y, de existir, el código en texto plano.</summary>
public record PasswordResetRequestResult(bool UserExists, string? Code);
