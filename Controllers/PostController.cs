using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AppDb _context;

        public PostController()
        {
            _context = new AppDb();
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPosts(int userId)
        {
            var friends = await _context.Friendships
                .Where(x => (x.SenderUserId == userId || x.ReceiverUserId == userId) && x.Status == 1)
                .Select(x => x.SenderUserId == userId ? x.ReceiverUserId : x.SenderUserId)
                .ToListAsync();

            friends.Add(userId); // Include the logged-in user in the list of friends

            var posts = await _context.Posts
                .Where(x => friends.Contains(x.UserId))
                .OrderByDescending(x => x.PostDate)
                .ToListAsync();

            foreach (var post in posts)
            {
                if (post.fileExt == "jpg" || post.fileExt == "jpeg" || post.fileExt == "png" || post.fileExt == "gif")
                {
                    post.image = true;

                }
                else if (post.fileExt == "mp4")
                {
                    post.video = true;
                }
                else
                {
                    post.text = true;
                }
            }
            
                return Ok(posts);
        }
        [HttpPost]
        public async Task<IActionResult> NewPost(Post post)
        {
            await _context.Posts.AddAsync(post);
            _context.SaveChanges();
            return Ok(post);
        }


       

        //    [HttpPost("/post/file/{id}/{content}")]
        //    public async Task<IActionResult> PostVideo(IFormFile file,string content,int id)
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