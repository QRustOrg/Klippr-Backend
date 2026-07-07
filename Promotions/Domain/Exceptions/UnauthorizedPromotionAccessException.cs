namespace Klippr_Backend.Promotions.Domain.Exceptions;

/// <summary>
/// Se lanza cuando un negocio intenta operar sobre una promoción que no le pertenece.
/// </summary>
public class UnauthorizedPromotionAccessException(string message) : InvalidOperationException(message);
