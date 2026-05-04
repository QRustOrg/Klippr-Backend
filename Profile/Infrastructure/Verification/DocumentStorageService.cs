using System.Net.Http;

namespace Infrastructure.Verification;

public class DocumentStorageService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _storageBaseUrl;

    public DocumentStorageService(IHttpClientFactory httpClientFactory, string storageBaseUrl)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _storageBaseUrl = storageBaseUrl ?? throw new ArgumentNullException(nameof(storageBaseUrl));
    }

    public async Task<string> UploadDocumentAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        if (fileStream == null)
            throw new ArgumentNullException(nameof(fileStream));
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty.", nameof(fileName));

        var client = _httpClientFactory.CreateClient();

        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StreamContent(fileStream), "file", fileName);

            var response = await client.PostAsync($"{_storageBaseUrl}/upload", content, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException("Failed to upload document.");

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return responseContent;
        }
    }

    public async Task<Stream> DownloadDocumentAsync(string documentUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentUrl))
            throw new ArgumentException("Document URL cannot be empty.", nameof(documentUrl));

        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(documentUrl, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException("Failed to download document.");

        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    public async Task<bool> DeleteDocumentAsync(string documentUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentUrl))
            throw new ArgumentException("Document URL cannot be empty.", nameof(documentUrl));

        var client = _httpClientFactory.CreateClient();
        var response = await client.DeleteAsync(documentUrl, cancellationToken);

        return response.IsSuccessStatusCode;
    }
}
