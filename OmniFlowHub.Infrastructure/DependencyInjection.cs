using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmniFlowHub.Application.Interfaces;
using OmniFlowHub.Domain.Interfaces;
using OmniFlowHub.Infrastructure.Persistence;
using OmniFlowHub.Infrastructure.Repositories;
using OmniFlowHub.Infrastructure.Services;

namespace OmniFlowHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //1. Registrar factory de MySQL/Dapper
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            //2. Registrar repositorios
            services.AddScoped<TenantDataRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();

            //3. Registrar el servicio de almacenamiento LocalStack/S3
            services.AddScoped<IStorageService, S3StorageService>();

            //4. Registrar el cliente de Amazon S3
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = configuration["AWS:ServiceUrl"] ?? "http://localhost:4566",
                    ForcePathStyle = true
                };
                return new AmazonS3Client("test", "test", config);
            });

            return services;
        }
    }
}