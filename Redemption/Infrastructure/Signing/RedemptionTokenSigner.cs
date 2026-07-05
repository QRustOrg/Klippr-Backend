using System.Security.Cryptography;
using System.Text;
using Klippr_Backend.Redemption.Application.OutboundServices.Signing;

namespace Klippr_Backend.Redemption.Infrastructure.Signing;

/// <summary>
/// Implementa la firma HMAC-SHA256 del token de canje usando un secreto de servidor.
/// </summary>
/// <author>Samuel Bonifacio</author>
public class RedemptionTokenSigner : IRedemptionTokenSigner
{
    private readonly byte[] _secretKeyBytes;

    public RedemptionTokenSigner(string secretKey)
    {
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentException("Secret key cannot be null or empty.", nameof(secretKey));

        if (secretKey.Length < 32)
            throw new ArgumentException("Secret key must be at least 32 characters long.", nameof(secretKey));

        _secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
    }

    public string Sign(Guid uniqueToken)
    {
        var payload = Encoding.UTF8.GetBytes(uniqueToken.ToString());
        var hash = HMACSHA256.HashData(_secretKeyBytes, payload);

        return Base64UrlEncode(hash);
    }

    public bool Verify(Guid uniqueToken, string signature)
    {
        if (string.IsNullOrWhiteSpace(signature))
            return false;

        var expectedSignature = Sign(uniqueToken);
        var expectedBytes = Encoding.UTF8.GetBytes(expectedSignature);
        var actualBytes = Encoding.UTF8.GetBytes(signature);

        return expectedBytes.Length == actualBytes.Length &&
               CryptographicOperations.FixedTimeEquals(expectedBytes, actualBytes);
    }

    private static string Base64UrlEncode(byte[] data) =>
        Convert.ToBase64String(data)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
}
