namespace OmniFlowHub.Application.Interfaces
{
    public interface IDocumentProcessingService
    {
        Task ProcessPendingDocumentsAsync(CancellationToken cancellationToken);
    }
}