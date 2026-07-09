namespace Klippr_Backend.Profile.Domain.Exceptions;

public class UnauthorizedProfileAccessException(string message) : InvalidOperationException(message);
