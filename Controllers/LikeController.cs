using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly AppDb _context;
        public LikeController()
        {
            _context = new AppDb();
        }
        [HttpGet("{postId}/{userId}")]
        public async Task<ActionResult<Post>> LikePost(int postId, int userId)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(x => x.userId == userId && x.postId == postId);
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
                return NotFound("Post not found");

            if (like == null)
            {
                like = new Like
                {
                    postId = postId,
                    userId = userId,
                    like = true
                };

                _context.Likes.Add(like);
                post.likes = (post.likes ?? 0) + 1;
            }
            else
            {
                if (like.like==true) // If already liked, then unlike
                {
                    like.like = false;
                    post.likes = (post.likes ?? 1) - 1;
                }
                else // If not liked, then like
                {
                    like.like = true;
                    post.likes = (post.likes ?? 0) + 1;
                }

                _context.Likes.Update(like);
            }

            await _context.SaveChangesAsync();

            return Ok(post);
        }

    }
}
