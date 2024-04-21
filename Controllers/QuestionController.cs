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
    public class QuestionController : ControllerBase
    {
        private readonly AppDb _context;

        public QuestionController()
        {
            _context = new AppDb();
        }

        [HttpGet]
        public async Task<IEnumerable<Question>> GetQuestions()
        {
            var questions = await _context.Questions.ToListAsync();
            var comment = await _context.QuestionComments.ToListAsync();

            int count = 1;
            int i = 0;
            if (questions != null) {

                
                foreach (var quest in questions)
                {
                    quest.index = i;
                    foreach(var com in comment)
                    {
                        if (quest.id == com.id)
                        {
                            quest.comment = count;

                            count += 1;
                        }

                        _context.Questions.Update(quest);
                    }
                    i += 1;

                }

            }
           
           // _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return await _context.Questions.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Question>> NewQuestion(Question question)
        {
            question.questionDate = DateTime.Now;

            await _context.Questions.AddAsync(question);
           await  _context.SaveChangesAsync();
            return Ok(question);
        }
    }
}
