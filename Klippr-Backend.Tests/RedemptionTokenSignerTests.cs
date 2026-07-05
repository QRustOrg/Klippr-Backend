using Klippr_Backend.Redemption.Infrastructure.Signing;

namespace Klippr_Backend.Tests;

public class RedemptionTokenSignerTests
{
    private const string SecretKey = "unit-test-secret-key-with-at-least-32-chars";

    [Fact]
    public void Verify_ReturnsTrueForSignatureFromSameKeyAndToken()
    {
        var signer = new RedemptionTokenSigner(SecretKey);
        var token = Guid.NewGuid();

        var signature = signer.Sign(token);

        Assert.True(signer.Verify(token, signature));
    }

    [Fact]
    public void Verify_ReturnsFalseWhenTokenDiffers()
    {
        var signer = new RedemptionTokenSigner(SecretKey);
        var signature = signer.Sign(Guid.NewGuid());

        Assert.False(signer.Verify(Guid.NewGuid(), signature));
    }

    [Fact]
    public void Verify_ReturnsFalseWhenSignatureIsTampered()
    {
        var signer = new RedemptionTokenSigner(SecretKey);
        var token = Guid.NewGuid();
        var signature = signer.Sign(token);

        var tampered = signature[..^1] + (signature[^1] == 'A' ? 'B' : 'A');

        Assert.False(signer.Verify(token, tampered));
    }

    [Fact]
    public void Verify_ReturnsFalseWhenSignatureIsMissing()
    {
        var signer = new RedemptionTokenSigner(SecretKey);

        Assert.False(signer.Verify(Guid.NewGuid(), string.Empty));
    }

    [Fact]
    public void Sign_IsDeterministicForSameKeyAndToken()
    {
        var signer = new RedemptionTokenSigner(SecretKey);
        var token = Guid.NewGuid();

        Assert.Equal(signer.Sign(token), signer.Sign(token));
    }

    [Fact]
    public void Constructor_RejectsShortSecretKey()
    {
        Assert.Throws<ArgumentException>(() => new RedemptionTokenSigner("too-short"));
    }
}
