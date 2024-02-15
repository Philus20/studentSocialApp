using BCrypt.Net;
using FinalProject.Models;using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;


namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly AppDb _context;

        public RegisterController()
        {
            _context = new AppDb();

        }

    }
}
//        [HttpGet]
//        public async Task <IEnumerable<register>> getUsers()
//        {
//            return await _context.Registers.ToListAsync();
//        }
//        [HttpGet("{email}")]
//        public async Task<ActionResult<register>> GetUserByEmail(string email)
//        {
//            var user = await _context.Registers.FirstOrDefaultAsync(x => x.email == email);

//            if (user == null)
//            {
//                // If user is not found, return a 404 Not Found response
//                return NotFound();
//            }

//            // If user is found, return a 200 OK response with the user data
//            return Ok(user);
//        }


//        private async Task <register?> getUserByEmail(string email)
//        {
//            return await _context.Registers
//                .FirstOrDefaultAsync(x => x.email == email);
//        }

//        [HttpPost]
//        public async Task<IActionResult > registerUser(register user)
//        {
//            try
//            {
//                if (await getUserByEmail(user.email) is register _)
//                    return Conflict("user already exist");

//                //bcrypt
//               // user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

//                await _context.Registers.AddAsync(user);
//                await _context.SaveChangesAsync();

               
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                // Log the exception and return a 500 Internal Server Error response
//                // You may want to handle exceptions more gracefully in a production scenario
//                // and provide more meaningful error messages.
//                Console.WriteLine(ex.Message);
//                return StatusCode(500, "Internal Server Error");
//            }
//        }
//    }
//}
