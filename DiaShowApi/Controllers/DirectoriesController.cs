using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace DiaShowApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoriesController : ControllerBase
    {
        [HttpGet("getAllDirectoriesNames")]
        public IActionResult GetAllDirectoriesNames(string path)
        {
            if (!Directory.Exists(path))
            {
                return NotFound(new { error = "Directory not found" });
            }

            var directories = Directory.GetDirectories(path)
                .Select(Path.GetFileName)
                .Where(name => name != "." && name != ".." && name != "thumbs" && name != "raw")
                .ToArray();

            return Ok(directories);
        }
    }
}
