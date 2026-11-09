using System.Data;
using Dapper;
using OmniFlowHub.Application.Interfaces;
using OmniFlowHub.Domain.Entities;

namespace OmniFlowHub.Infrastructure.Repositories
{
    public class TenantDataRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TenantDataRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TenantDataPayload>> GetAllAsync()
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                const string sql = "SELECT Id, TenantId, PayloadData, CreatedAt FROM TenantPayloads";

                //Mapear con Dapper
                var result = await connection.QueryAsync<TenantDataPayload>(sql);
                return result;
            }
        }
    }
}