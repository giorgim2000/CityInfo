using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }
        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var filePath = "test2.txt";

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            if(!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, contentType, Path.GetFileName(filePath));
        }
    }
}
