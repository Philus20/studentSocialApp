using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AppDb _appDb;

        public FilesController()
        {
                _appDb = new AppDb();
        }

        //        [HttpPost]
        //        public async Task<string> writeFile(IFormFile file){


        //            if (file == null || file.Length == 0)
        //                return "no file";

        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files\\profilePictures", file.FileName) ;
        //            path = path + ".jpeg";

        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //}

        //return "File uploaded successfully)";
        //    }
        [HttpPost("upload/{studentId}")]
        public async Task<IActionResult> Upload(IFormFile file, int studentId )
        {
            var student = await _appDb.Students.FirstOrDefaultAsync(x => x.Id == studentId);

            // Check if the student is not found
            if (student == null)
            {
                // Handle the case where the student is not found
                return NotFound($"Student with ID {studentId} not found");
            }
            //if (student is null)
            //    return BadRequest("Student is not found");

            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            // Check if the file is an image based on its content type
            if (!file.ContentType.StartsWith("image"))
                return BadRequest("Invalid file type. Only images are allowed.");


            if (!string.IsNullOrEmpty(student.profilePictureName))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "PP", student.profilePictureName);

                if (System.IO.File.Exists(oldFilePath))
                {
                   
                    System.IO.File.Delete(oldFilePath);
                }
            }

            // Get the file extension from the content type
            var fileExtension = file.ContentType.Split("/").LastOrDefault();

            // Generate a unique filename with the image file extension
            var uniqueFileName = $"{Guid.NewGuid()}.{fileExtension}";

            var path = Path.Combine(Directory.GetCurrentDirectory(), "PP",uniqueFileName);


            using (var stream = new FileStream(path, FileMode.Create))
            {


                await file.CopyToAsync(stream);
                student.profilePictureName = uniqueFileName;
                _appDb.Students.Update(student);
            }

            await _appDb.SaveChangesAsync();


            // Return success message with the file name
            return Ok(student);
        }

        // GET: api/file/{fileName}
        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            // Construct the full path to the file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PP", fileName);

            // Log the file path for debugging
            Console.WriteLine($"Attempting to retrieve file at path: {filePath}");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                // Log an error for debugging
                Console.WriteLine($"File not found at path: {filePath}");

                return NotFound("File not found");
            }

            // Read the file content and return as a file response
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return new FileContentResult(fileBytes, "image/*");
        }
    }
}