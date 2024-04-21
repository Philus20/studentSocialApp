using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.StaticFiles;
namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostInfoController : ControllerBase
    {
        private readonly AppDb _context;
        public PostInfoController()
        {
            _context = new AppDb();

        }
        [HttpGet("{id}")]
        public async Task<Student> GetInfo(int id)
        {
            var user = await _context.Students.FindAsync(id);


            return user;
        }

        [HttpPost("{id}/{content}")]
        public async Task<IActionResult> Upload(IFormFile file, int id,  String content)
        {
            //var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);
            //ar file = new filesMessages();
            var p = new Post();

            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            // Check if the file is an image based on its content type
            //if (!(file.ContentType.StartsWith("image/") || file.ContentType.StartsWith("video/")))
            //    return BadRequest("Invalid file type. Only images or videos are allowed.");



            // Get the file extension from the content type
            var fileExtension = file.ContentType.Split("/").LastOrDefault();

            // Generate a unique filename with the image file extension
            var uniqueFileName = $"{Guid.NewGuid()}.{fileExtension}";

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", "posts", uniqueFileName);


            using (var stream = new FileStream(path, FileMode.Create))
            {


                await file.CopyToAsync(stream);
               
                //_context.Students.Update(student);
            }
            if (content == "undefined")
            {
                p.Content= null;
            }
            else { p.Content = content; }

            //message.ReceiverEmail = receiver;
            p.fileName = uniqueFileName;
            p.fileExt = fileExtension;
            p.PostDate = DateTime.Now;
            p.UserId = id;
            _context.Posts.Add(p);
            await _context.SaveChangesAsync();


            // Return success message with the file name
            return Ok(p);
        }

        //[HttpPost]
        ////    [HttpPost("/post/file/{id}/{content}")]
        //    public async Task<IActionResult> PostVideo(IFormFile file, int id, string content)
        //    {
        //        var post = new Post();

        //        if (file == null || file.Length == 0)
        //            return BadRequest("Invalid file");





        //        // Get the file extension from the content type
        //        var fileExtension = file.ContentType.Split("/").LastOrDefault();

        //        // Generate a unique filename with the image file extension
        //        var uniqueFileName = $"{Guid.NewGuid()}.{fileExtension}";

        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", "posts", uniqueFileName);


        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {


        //            await file.CopyToAsync(stream);
        //            post.fileName= uniqueFileName;
        //            post.fileExt = fileExtension;
        //            //_context.Students.Update(student);
        //        }
        //        if (content == "undefined")
        //        {
        //            post.Content = null;
        //        }
        //        else { post.Content = content; }


        //        post.PostDate = DateTime.Now;

        //        post.UserId = id;

        //        _context.Posts.Add(post);
        //        await _context.SaveChangesAsync();

        //        return Ok(post);
        //    }
        //}

    }
}