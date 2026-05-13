using Klippr_Backend.IAM.Application.OutboundServices.Hashing;
using BCrypt.Net;

namespace Klippr_Backend.IAM.Infrastructure.Hashing;

public class HashingService : IHashingService
{
    private const int WorkFactor = 12;

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool Verify(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));

        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
