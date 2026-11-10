using Dapper;
using OmniFlowHub.Application.Interfaces;
using OmniFlowHub.Domain.Entities;
using OmniFlowHub.Domain.Enums;
using OmniFlowHub.Domain.Interfaces;
using OmniFlowHub.Infrastructure.Persistence.Models;

namespace OmniFlowHub.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DocumentRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Guid> AddAsync(DocumentItem document)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
            INSERT INTO Documents (Id, FileName, ContentType, S3Key, Status, CreatedAt, ProcessedAt, ErrorMessage)
            VALUES (@Id, @FileName, @ContentType, @S3Key, @Status, @CreatedAt, @ProcessedAt, @ErrorMessage);";

            await connection.ExecuteAsync(sql, new
            {
                document.Id,
                document.FileName,
                document.ContentType,
                document.S3Key,
                Status = (int)document.Status,
                document.CreatedAt,
                document.ProcessedAt,
                document.ErrorMessage
            });

            return document.Id;
        }

        public async Task<DocumentItem?> GetByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = "SELECT * FROM Documents WHERE Id = @Id;";
            var dto = await connection.QueryFirstOrDefaultAsync<DocumentDto>(sql, new { Id = id });

            if (dto == null)
            {
                return null;
            }

            return MapToDomain(dto);
        }

        public async Task<IEnumerable<DocumentItem>> GetPendingDocumentsAsync(int limit, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = "SELECT * FROM Documents WHERE Status = @Status LIMIT @Limit;";
            var dtos = await connection.QueryAsync<DocumentDto>(sql, new { Status = (int)DocumentStatus.Pending, Limit = limit });

            return dtos.Select(MapToDomain);
        }

        public async Task UpdateStatusAsync(DocumentItem document, CancellationToken cancellationToken = default)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                UPDATE Documents 
                SET Status = @Status, ProcessedAt = @ProcessedAt, ErrorMessage = @ErrorMessage
                WHERE Id = @Id;";

            await connection.ExecuteAsync(sql, new
            {
                Id = document.Id,
                Status = (int)document.Status,
                document.ProcessedAt,
                document.ErrorMessage
            });
        }

        private static DocumentItem MapToDomain(DocumentDto dto)
        {
            return new DocumentItem(
                dto.Id,
                dto.FileName,
                dto.ContentType,
                dto.S3Key,
                (DocumentStatus)dto.Status,
                dto.CreatedAt,
                dto.ProcessedAt,
                dto.ErrorMessage
            );
        }
    }
}
