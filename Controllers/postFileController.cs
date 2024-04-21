using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class postFileController : ControllerBase
    {
        private readonly AppDb context;

        public postFileController()
        {
            context = new AppDb();
        }

        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("Invalid file name");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "posts", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");

            // Determine the content type based on the file extension
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return new FileContentResult(fileBytes, contentType);
        }

    }
}
