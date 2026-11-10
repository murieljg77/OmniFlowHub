using Microsoft.Extensions.Logging;
using NSubstitute;
using OmniFlowHub.Application.Services;
using OmniFlowHub.Domain.Entities;
using OmniFlowHub.Domain.Enums;
using OmniFlowHub.Domain.Interfaces;

namespace OmniFlowHub.UnitTests
{
    public class DocumentProcessingServiceTests
    {
        [Fact]
        public async Task ProcessPendingDocumentsAsync_WhenDocumentsArePending_ShouldMarkAsCompletedAndUpdate()
        {
            //Arrange
            var repositoryMock = Substitute.For<IDocumentRepository>();
            var loggerMock = Substitute.For<ILogger<DocumentProcessingService>>();

            //Crear una lista falsa de documentos pendientes
            var doc = new DocumentItem("ejemplo1.txt", "text/plain", "documents/ejemplo1.txt");
            var pendingDocs = new List<DocumentItem> { doc };

            //Configurar el mock: cuando invoque a GetPendingDocumentsAsync, devuelve la lista anterior
            repositoryMock.GetPendingDocumentsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<DocumentItem>>(pendingDocs));

            var service = new DocumentProcessingService(repositoryMock, loggerMock);

            //Act
            await service.ProcessPendingDocumentsAsync(CancellationToken.None);

            //Assert
            //Verificar que se haya invocado a UpdateStatusAsync exactamente una vez 
            //y que el estado del documento haya cambiado a Completed
            await repositoryMock.Received(1).UpdateStatusAsync(
                doc,
                Arg.Any<CancellationToken>()
            );
            Assert.Equal(DocumentStatus.Completed, doc.Status);
        }

        [Fact]
        public async Task ProcessPendingDocumentsAsync_WhenExceptionOccurs_ShouldMarkAsFailedAndUpdate()
        {
            //Arrange
            var repositoryMock = Substitute.For<IDocumentRepository>();
            var loggerMock = Substitute.For<ILogger<DocumentProcessingService>>();

            var doc = new DocumentItem("ejemplo_error.txt", "text/plain", "documents/error.txt");
            var pendingDocs = new List<DocumentItem> { doc };

            repositoryMock.GetPendingDocumentsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<DocumentItem>>(pendingDocs));

            //Forzar a que la actualización falle o simular un error de procesamiento lanzando una excepción
            var callCount = 0;
            repositoryMock.When(x => x.UpdateStatusAsync(Arg.Any<DocumentItem>(), Arg.Any<CancellationToken>()))
                .Do(x =>
                {
                    callCount++;
                    if (callCount == 1)
                    {
                        throw new Exception("Error de conexión simulado.");
                    }
                });

            var service = new DocumentProcessingService(repositoryMock, loggerMock);

            //Act & Assert
            //Como el catch interno maneja la excepción de forma individual, el método no debe colapsar
            //el hilo principal, pero sí tiene que llamar a actualizar con el estado Failed.
            await service.ProcessPendingDocumentsAsync(CancellationToken.None);

            //Verificar que se intentó actualizar el estado a Failed
            await repositoryMock.Received(2).UpdateStatusAsync(
                doc,
                Arg.Any<CancellationToken>()
            );
            Assert.Equal(DocumentStatus.Failed, doc.Status);
        }
    }
}