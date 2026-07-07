namespace Klippr_Backend.Redemption.Domain.Exceptions;

/// <summary>
/// Represents a business conflict that prevents creating or confirming a redemption.
/// </summary>
/// <author>Samuel Bonifacio</author>
public class RedemptionConflictException(string message) : InvalidOperationException(message);
