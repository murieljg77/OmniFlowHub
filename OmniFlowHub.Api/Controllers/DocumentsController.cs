using Microsoft.AspNetCore.Mvc;
using OmniFlowHub.Application.Documents.Commands;

namespace OmniFlowHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly UploadDocumentCommandHandler _uploadHandler;

        public DocumentsController(UploadDocumentCommandHandler uploadHandler)
        {
            _uploadHandler = uploadHandler;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            using var stream = file.OpenReadStream();

            var command = new UploadDocumentCommand(file.FileName, file.ContentType, stream);

            var documentId = await _uploadHandler.HandleAsync(command, cancellationToken);

            return Ok(
                new { 
                    Message = "Document uploaded successfully and pending processing.", 
                    DocumentId = documentId 
                }
            );
        }
    }
}