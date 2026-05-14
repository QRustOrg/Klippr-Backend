namespace Klippr_Backend.Profile.Application.OutboundServices;

public interface IVerificationService
{
    Task<bool> VerifyDocumentAsync(string documentUrl, CancellationToken cancellationToken = default);
    Task<bool> IsDocumentStoredAsync(string documentUrl, CancellationToken cancellationToken = default);
}
