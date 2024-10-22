using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace DiaShowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly ILogger<PicturesController> _logger;

        public PicturesController(ILogger<PicturesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getAllPictures")]
        public IActionResult GetAllPictures(string path)
        {
            if (!Directory.Exists(path))
            {
                return NotFound(new { error = "Directory not found" });
            }

            var pictures = Directory.GetFiles(path)
                .Where(file => new[] { ".jpeg", ".png", ".gif", ".svg" }.Contains(Path.GetExtension(file).ToLower()))
                .Select(Path.GetFileName)
                .ToArray();

            return Ok(pictures);
        }

        [HttpGet("getPicture")]
        public IActionResult GetPicture(string path, string pictureName)
        {
            var filePath = Path.Combine(path, pictureName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { error = "Picture not found" });
            }

            var fileExtension = Path.GetExtension(filePath).ToLower();
            var supportedFileTypes = new[] { ".jpeg", ".png", ".gif", ".svg" };

            if (!supportedFileTypes.Contains(fileExtension))
            {
                _logger.LogWarning($"Unsupported file type requested: {fileExtension}");
                return StatusCode(415, new { error = "Unsupported file type" });
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, $"image/{fileExtension.TrimStart('.')}");
        }

        [HttpGet("getThumb")]
        public IActionResult GetThumb(string path, string pictureName)
        {
            var thumbPath = Path.Combine(path, "thumbs", pictureName);

            if (!System.IO.File.Exists(thumbPath))
            {
                return NotFound(new { error = "Thumbnail not found" });
            }

            var fileExtension = Path.GetExtension(thumbPath).ToLower();
            var supportedFileTypes = new[] { ".jpeg", ".png", ".gif", ".svg" };

            if (!supportedFileTypes.Contains(fileExtension))
            {
                _logger.LogWarning($"Unsupported file type requested: {fileExtension}");
                return StatusCode(415, new { error = "Unsupported file type" });
            }

            var fileBytes = System.IO.File.ReadAllBytes(thumbPath);
            return File(fileBytes, $"image/{fileExtension.TrimStart('.')}");
        }
    }
}
