using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
