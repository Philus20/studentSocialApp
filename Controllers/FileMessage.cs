using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    public class FileMessage : Controller
    {

        private readonly AppDb _context;
        public FileMessage()
        {
            _context = new AppDb();
        }


        //[HttpGet("{fileName}")]
        //public IActionResult GetFile(string fileName)
        //{
        //    var file = Path.Combine(Directory.GetCurrentDirectory(), "Files", "sharingFiles", fileName);
        //    var memory = new MemoryStream();

        //    using (var stream = new FileStream(file, FileMode.Open))
        //    {
        //        stream.CopyTo(memory);
        //    }
        //    memory.Position = 0;

        //    var contentType = GetContentType(file);

        //    return File(memory, contentType, Path.GetFileName(file));
        //}

        [HttpGet("{fileName}/documents")]
        public IActionResult GetFile(string fileName)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "Files", "sharingFiles", fileName);
            var memory = new MemoryStream();

            using (var stream = new FileStream(file, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            var contentType = GetContentType(file);

            return File(memory, contentType); // Removed the third argument Path.GetFileName(file)
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
        {
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".png", "image/png"},
            {".gif", "image/gif"},
            {".mp4", "video/mp4"},

            // Add more mime types as needed
        };
        }
        [HttpGet("{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("Invalid file name");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "sharingFiles", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            // Determine the content type based on the file extension
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            // Set the Content-Disposition header
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            // Return the file content as a file download
            return File(memory, contentType, Path.GetFileName(filePath));
        }

        //[HttpGet("{fileName}")]
        //public async Task<IActionResult> GetFile(string fileName)
        //{
        //    if (string.IsNullOrEmpty(fileName))
        //        return BadRequest("Invalid file name");

        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "sharingFiles", fileName);

        //    if (!System.IO.File.Exists(filePath))
        //        return NotFound("File not found");

        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(filePath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    // Determine the content type based on the file extension
        //    var provider = new FileExtensionContentTypeProvider();
        //    if (!provider.TryGetContentType(fileName, out var contentType))
        //    {
        //        contentType = "application/octet-stream";
        //    }

        //    // Return the file for download
        //    return File(memory, contentType, Path.GetFileName(filePath));
        //}


        //[HttpGet("{sender}/{active}")]
        //public async Task<ActionResult<Message>> Get(string sender, string active)
        //{
        //    var message = await _context.FileMessages
        //        .Where(x => ((x.SenderEmail == sender) || (x.ReceiverEmail == sender)) && ((x.SenderEmail == active) || (x.ReceiverEmail == active)))
        //       .ToListAsync();


        //    if (message == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(message);
        //}


        // POST api/values
        [HttpPost("{sender}/{receiver}/{content}/{status}" )]
        public async Task<IActionResult> Upload(IFormFile file, string sender, string receiver,String content , string status)
        {
            //var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            //ar file = new filesMessages();
            var message = new Message();
          
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            // Check if the file is an image based on its content type
            //if (!(file.ContentType.StartsWith("image/") || file.ContentType.StartsWith("video/")))
            //    return BadRequest("Invalid file type. Only images or videos are allowed.");


           
            // Get the file extension from the content type
            var fileExtension = file.ContentType.Split("/").LastOrDefault();

            // Generate a unique filename with the image file extension
            var uniqueFileName = $"{Guid.NewGuid()}.{fileExtension}";

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", "sharingFiles", uniqueFileName);


            using (var stream = new FileStream(path, FileMode.Create))
            {


                await file.CopyToAsync(stream);
               message.file = uniqueFileName;
                //_context.Students.Update(student);
            }
            if (content=="undefined") {
                message.Subject = null;
                    }
            else { message.Subject = content; }
            
            message.ReceiverEmail = receiver;
            message.SenderEmail = sender;
            message.isFile = "1";
            message.Status = status;
           message.time = DateTime.Now;

            //{ ".jpg", "image/jpeg"},
            //{ ".jpeg", "image/jpeg"},
            //{ ".png", "image/png"},
            //{ ".gif", "image/gif"},
            //{ ".mp4", "video/mp4
            if(fileExtension == "jpg" || fileExtension=="jpeg"|| fileExtension=="png" || fileExtension=="gif")
            {
                message.ext = "image";
            }
            else if (fileExtension == "mp4")
            {
                message.ext = "video";
            }
            else
            {
                message.ext = "file";
            }
           
            //files.Status = mes.Status;

           _context.Messages.Add(message);
            await _context.SaveChangesAsync();


            // Return success message with the file name
            return Ok(message);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessageStatus(int id)
        {

            var mess = await _context.FileMessages.FindAsync(id);
            mess.Status = "1";
            await _context.SaveChangesAsync();

            //Where(x => ((x.SenderEmail == sender && x.ReceiverEmail == receiver) || (x.SenderEmail == receiver && x.ReceiverEmail == sender)) && x.Status == "0").
            //Select(x =>  x.Status ).ToListAsync();

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

