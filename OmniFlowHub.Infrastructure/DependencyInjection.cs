using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmniFlowHub.Application.Interfaces;
using OmniFlowHub.Infrastructure.Persistence;
using OmniFlowHub.Infrastructure.Repositories;
using OmniFlowHub.Infrastructure.Services;

namespace OmniFlowHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Registrar factory de MySQL/ Dapper
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            //Registrar repositorios
            services.AddScoped<TenantDataRepository>();

            //Registrar el servicio de almacenamiento LocalStack/ S3 
            services.AddSingleton<S3StorageService>();

            return services;
        }
    }
}