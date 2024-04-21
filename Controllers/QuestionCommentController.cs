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
    public class QuestionCommentController : ControllerBase
    {
        private readonly AppDb _context;

        public QuestionCommentController()
        {
            _context = new AppDb();
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<QuestionComment>> Comments(int id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(x => x.id == id);
            var comment = await _context.QuestionComments
                .Where(x=>x.questionId==id).ToListAsync();
            int i = 0;

            foreach(var com in comment)
            {
                com.index = i;
                i += 1;
            }
          //  question.comment = comment.Count();

           // if(question != null) { }
            // _context.Questions.Update(question);
            //await _context.SaveChangesAsync();

            return comment;
        }

        [HttpPost]
        public async Task<ActionResult<QuestionComment>> CommentQuestion(QuestionComment q)
        {
            q.commentDate = DateTime.Now;
            await _context.QuestionComments.AddAsync(q);
           await _context.SaveChangesAsync();


            return Ok(q);
        }
    }
}
