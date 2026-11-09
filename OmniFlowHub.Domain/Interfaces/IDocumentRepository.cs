using OmniFlowHub.Domain.Entities;

namespace OmniFlowHub.Domain.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Guid> AddAsync(DocumentItem document);
        Task<DocumentItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<DocumentItem>> GetPendingDocumentsAsync(int limit);
        Task UpdateStatusAsync(DocumentItem document);
    }
}