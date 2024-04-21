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
    public class StuCommentController : ControllerBase
    {

        private readonly AppDb _context;
        public StuCommentController()
        {
            _context = new AppDb();

        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<IEnumerable<StudentComment>>> GetStuCom( int postId)
        {
            try
            {
                var comments = await _context.Comments
                    .Where(c => c.PostId == postId)
                    .ToListAsync();

                

                if (comments == null || comments.Count == 0)
                {
                    return NotFound(); // Return a 404 Not Found response if no comments are found
                }

                foreach (var comment in comments)
                {
                    var stuC = await _context.stuCom.FirstOrDefaultAsync(x => x.comentId == comment.Id);
                    var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == comment.UserId);
                    if (student != null && stuC == null)
                    {
                        var stuCom = new StudentComment
                        {
                            CommentDate = comment.CommentDate,
                            Content = comment.Content,
                            firstName = student.firstName,
                            profilePictureName = student.profilePictureName,
                            Programme = student.Programme,
                            surname = student.surname,
                            UserId = student.Id,
                            PostId = comment.PostId,
                            comentId=comment.Id,
                        };

                        await _context.AddAsync(stuCom);
                    }
                }

                await _context.SaveChangesAsync(); // Save changes asynchronously

                // Return StudentComments
                var studentComments = await _context.stuCom.Where(sc => sc.PostId == postId).ToListAsync();
                return Ok(studentComments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
