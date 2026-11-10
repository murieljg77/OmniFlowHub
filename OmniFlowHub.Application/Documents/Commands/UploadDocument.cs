namespace OmniFlowHub.Application.Documents.Commands
{
    public record UploadDocumentCommand(
        string FileName,
        string ContentType,
        Stream FileStream
    );
}