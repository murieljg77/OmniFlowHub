namespace OmniFlowHub.Infrastructure.Persistence.Models
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public string S3Key { get; set; } = default!;
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }
    }
}