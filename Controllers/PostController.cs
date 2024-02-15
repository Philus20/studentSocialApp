using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            return Ok(posts);
        }
        [HttpPost]
        public async Task<IActionResult> NewPost(Post post)
        {
            await _context.Posts.AddAsync(post);
            _context.SaveChanges();
            return Ok(post);
        }
    }
}
