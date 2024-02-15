using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDb _context;

        public MessageController()
        {
            _context = new AppDb();
        }
        [HttpGet]
        public async Task <IEnumerable<Message>> Messages1()
        {

            return await _context.Messages.ToListAsync();
        }

        [HttpGet("{sender}/{active}")]
        public async Task <ActionResult<Message>> Get(string sender, string active)
        {
            var message = await _context.Messages
                .Where(x =>( (x.SenderEmail == sender) || (x.ReceiverEmail == sender)) && ((x.SenderEmail == active) || (x.ReceiverEmail == active)))
               .ToListAsync();
           

            if(message == null ) {
                return NotFound();
            }
            return Ok( message);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessageStatus(int id){

            var mess = await _context.Messages.FindAsync(id);
            mess.Status = "1";
           await _context.SaveChangesAsync();

                //Where(x => ((x.SenderEmail == sender && x.ReceiverEmail == receiver) || (x.SenderEmail == receiver && x.ReceiverEmail == sender)) && x.Status == "0").
                //Select(x =>  x.Status ).ToListAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Message>> returnMessage(Message message)
        {

            try
            {
               await  _context.Messages.AddAsync(message);
               await  _context.SaveChangesAsync();

                return message;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
