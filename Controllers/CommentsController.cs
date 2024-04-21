using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private AppDb _context;

        public CommentsController()
        {
            _context = new AppDb();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            try
            {
                var comments = await _context.Comments.ToListAsync();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 Internal Server Error response
                // You may want to handle exceptions more gracefully in a production scenario
                // and provide more meaningful error messages.
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.
                Where(f => f.PostId == id).ToListAsync();

            if (comment == null)
            {
                return NotFound(); // Return a 404 Not Found response if the comment is not found
            }

            return Ok(comment); // Return t comments
        }


        [HttpPost]
        public async Task<ActionResult<Comment>> AddComment([FromBody] Comment comment)
        {
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == comment.PostId);
                if (post != null)
                {
                    post.count = (post.count ?? 0) + 1; // Increment count by 1 (handling null value)
                    _context.Posts.Update(post);
                    comment.CommentDate = DateTime.Now;
                    await _context.Comments.AddAsync(comment);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound(); // Post not found
                }

                // Return a 201 Created response with the added comment
                return Ok(comment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment updatedComment)
        {
            try
            {
                var existingComment = await _context.Comments.FindAsync(id);

                if (existingComment == null)
                {
                    return NotFound();
                }

                existingComment.Content = updatedComment.Content;
                // Update other properties as needed

                _context.Comments.Update(existingComment);
                await _context.SaveChangesAsync();

                // Return a 200 OK response with the updated comment
                return Ok(existingComment);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var commentToDelete = await _context.Comments.FindAsync(id);

                if (commentToDelete == null)
                {
                    return NotFound();
                }

                _context.Comments.Remove(commentToDelete);
                await _context.SaveChangesAsync();

                // Return a 200 OK response with a message or you can return the deleted comment if needed
                return Ok("Comment deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }



    }
}
