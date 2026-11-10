using Microsoft.Extensions.Logging;
using OmniFlowHub.Application.Interfaces;
using OmniFlowHub.Domain.Interfaces;

namespace OmniFlowHub.Application.Services
{
    public class DocumentProcessingService : IDocumentProcessingService
    {
        readonly IDocumentRepository _documentRepository;
        readonly ILogger<DocumentProcessingService> _logger;

        public DocumentProcessingService(
            IDocumentRepository documentRepository,
            ILogger<DocumentProcessingService> logger)
        {
            _documentRepository = documentRepository;
            _logger = logger;
        }

        public async Task ProcessPendingDocumentsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando documentos pendientes en MySQL...");

            //Pasar el token a la consulta por lotes (pasar un límite, por ejemplo 10 registros)
            var pendingDocuments = await _documentRepository.GetPendingDocumentsAsync(10, cancellationToken);

            if (!pendingDocuments.Any())
            {
                return;
            }

            foreach (var doc in pendingDocuments)
            {
                //Verificar si se solicitó cancelación antes de procesar el siguiente archivo
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    _logger.LogInformation("Procesando documento Id: {Id}, Nombre: {FileName}", doc.Id, doc.FileName);

                    //Lógica de dominio. Cambiar el estado usando el comportamiento encapsulado
                    doc.MarkAsCompleted();

                    //Pasar el token al actualizar
                    await _documentRepository.UpdateStatusAsync(doc, cancellationToken);

                    _logger.LogInformation("Documento Id: {Id} procesado exitosamente.", doc.Id);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("El procesamiento fue cancelado por el sistema.");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al procesar el documento Id: {Id}", doc.Id);

                    //Lógica de dominio. Manejar el fallo y el mensaje de error de forma segura
                    doc.MarkAsFailed(ex.Message);

                    //Pasar el token al actualizar el error
                    await _documentRepository.UpdateStatusAsync(doc, cancellationToken);
                }
            }
        }
    }
}