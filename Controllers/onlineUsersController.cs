using FinalProject.Models;
using FinalProject.services;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class onlineUsersController : Controller
    {
        private readonly ChatServices _chatServices;
        public onlineUsersController()
        {
            _chatServices = new ChatServices();
        }
        [HttpPost("register-user")]
        public IActionResult RegisterUser(User model)
        {
            if (_chatServices.AddUserTOList(model.UserName))
            {
                _chatServices.userName = model.UserName;
                //202 status code

                return NoContent();

            }
            return BadRequest("This name is taken please choose another name");
        }

        [HttpDelete("{UserName}")]
        public IActionResult DeleteUser(string email) {

            if (email == null)
            {

                return BadRequest("User not found");
            }
            _chatServices.RemoveUserFromList(email);
            return NoContent();
            
        }
    }

}
