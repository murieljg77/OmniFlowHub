using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace OmniFlowHub.Infrastructure.Services;

public class S3StorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3StorageService(IConfiguration configuration)
    {
        var awsOptions = configuration.GetSection("AWS");
        var serviceUrl = awsOptions["ServiceURL"]; //Para LocalStack (ej.: http://localhost:4566)
        var accessKey = awsOptions["AccessKey"] ?? "test";
        var secretKey = awsOptions["SecretKey"] ?? "test";
        _bucketName = awsOptions["BucketName"] ?? "omniflowhub-bucket";

        var config = new AmazonS3Config();

        //Si estamos usando LocalStack, configuramos el ServiceURL y desactivamos la validación de región estricta
        if (!string.IsNullOrEmpty(serviceUrl))
        {
            config.ServiceURL = serviceUrl;
            config.ForcePathStyle = true; //Para LocalStack/ S3 compatible
        }

        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    public async Task<bool> UploadFileAsync(string fileName, Stream fileStream, string contentType)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType
            };

            var response = await _s3Client.PutObjectAsync(putRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception)
        {
            //Aquí integrar un logger o retornar un Result Pattern
            return false;
        }
    }
}