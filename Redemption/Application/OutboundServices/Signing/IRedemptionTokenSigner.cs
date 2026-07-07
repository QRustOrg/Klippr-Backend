namespace Klippr_Backend.Redemption.Application.OutboundServices.Signing;

/// <summary>
/// Firma y verifica el token opaco de un canje para protegerlo contra manipulación.
/// </summary>
/// <author>Samuel Bonifacio</author>
public interface IRedemptionTokenSigner
{
    /// <summary>
    /// Calcula la firma HMAC del token único de un canje.
    /// </summary>
    /// <param name="uniqueToken">Token único del canje.</param>
    /// <returns>Firma en formato base64url.</returns>
    string Sign(Guid uniqueToken);

    /// <summary>
    /// Verifica que una firma corresponda al token único indicado.
    /// </summary>
    /// <param name="uniqueToken">Token único del canje.</param>
    /// <param name="signature">Firma recibida del cliente.</param>
    /// <returns><see langword="true"/> si la firma es válida.</returns>
    bool Verify(Guid uniqueToken, string signature);
}
