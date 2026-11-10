using OmniFlowHub.Domain.Entities;
using OmniFlowHub.Domain.Interfaces;
using OmniFlowHub.Application.Interfaces;

namespace OmniFlowHub.Application.Documents.Commands
{
    public class UploadDocumentCommandHandler
    {
        private readonly IStorageService _storageService;
        private readonly IDocumentRepository _documentRepository;

        public UploadDocumentCommandHandler(
            IStorageService storageService,
            IDocumentRepository documentRepository)
        {
            _storageService = storageService;
            _documentRepository = documentRepository;
        }

        public async Task<Guid> HandleAsync(UploadDocumentCommand command, CancellationToken cancellationToken = default)
        {
            //1. Subir el archivo al bucket de AWS S3 y obtener la clave generada
            var s3Key = await _storageService.UploadFileAsync(
                command.FileStream,
                command.FileName,
                command.ContentType,
                cancellationToken);

            //2. Crear la entidad de dominio con estado inicial Pending
            var document = new DocumentItem(command.FileName, command.ContentType, s3Key);

            //3. Persistir la metadata en MySQL usando Dapper mediante el repositorio
            var documentId = await _documentRepository.AddAsync(document);

            return documentId;
        }
    }
}