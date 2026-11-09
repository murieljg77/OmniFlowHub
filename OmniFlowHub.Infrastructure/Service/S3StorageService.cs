using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using OmniFlowHub.Application.Interfaces;

namespace OmniFlowHub.Infrastructure.Services
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3StorageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException("AWS:BucketName is not configured.");
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            var s3Key = $"documents/{Guid.NewGuid()}_{fileName}";

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = s3Key,
                InputStream = fileStream,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(putRequest, cancellationToken);

            return s3Key;
        }

        public async Task<Stream> DownloadFileAsync(string s3Key, CancellationToken cancellationToken = default)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = s3Key
            };

            var response = await _s3Client.GetObjectAsync(request, cancellationToken);
            return response.ResponseStream;
        }

        public async Task DeleteFileAsync(string s3Key, CancellationToken cancellationToken = default)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = s3Key
            };

            await _s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);
        }
    }
}