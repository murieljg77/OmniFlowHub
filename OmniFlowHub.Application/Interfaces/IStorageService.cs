namespace OmniFlowHub.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
        Task<Stream> DownloadFileAsync(string s3Key, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string s3Key, CancellationToken cancellationToken = default);
    }
}