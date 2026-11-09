using OmniFlowHub.Domain.Enums;

namespace OmniFlowHub.Domain.Entities 
{
    public class DocumentItem
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public string S3Key { get; private set; }
        public DocumentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string? ErrorMessage { get; private set; }

        public DocumentItem(string fileName, string contentType, string s3Key)
        {
            Id = Guid.NewGuid();
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
            S3Key = s3Key ?? throw new ArgumentNullException(nameof(s3Key));
            Status = DocumentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsProcessing()
        {
            Status = DocumentStatus.Processing;
        }

        public void MarkAsCompleted()
        {
            Status = DocumentStatus.Completed;
            ProcessedAt = DateTime.UtcNow;
        }

        public void MarkAsFailed(string errorMessage)
        {
            Status = DocumentStatus.Failed;
            ErrorMessage = errorMessage;
            ProcessedAt = DateTime.UtcNow;
        }
    }
}