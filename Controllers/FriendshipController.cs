using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly AppDb _appDb;

        public FriendshipController()
        {
            _appDb = new AppDb();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Friends(int id)
        {
            // Assuming Friendship is the entity type and _appDb is the DbContext
            var student = await _appDb.Students.FindAsync(id);
            int getId = student.Id;

            var friendship = new Friendship();

            if (friendship != null && (friendship.SenderUserId == getId || friendship.ReceiverUserId == getId) && friendship.Status == 1)
            {
                // Retrieve friends based on the user's ID and friendship status
                var friends = await _appDb.Friendships
                    .Where(f => (f.SenderUserId == id || f.ReceiverUserId == id) && f.Status == 1)
                    .ToListAsync();

                return Ok(student);
            }

            return Ok("User not found or not in a friendship.");
        }

        [HttpPost]
        public async Task<IActionResult> NewFriend(Friendship friend)
        {
            await _appDb.Friendships.AddAsync(friend);
            _appDb.SaveChanges();

            return Ok(friend);
        }

        [HttpPut("accepted/{senderId}/{ReceiverId}")]
        public async Task<IActionResult> Accepted(int senderId, int ReceiverId)
        {
            var friendship = await _appDb.Friendships
                .FirstOrDefaultAsync(f => f.SenderUserId == senderId && f.ReceiverUserId == ReceiverId);

            if (friendship == null)
            {
                // Friendship not found
                return NotFound("Friendship not found.");
            }

            // Perform any additional logic related to the friendship here
            friendship.Status = 1;

            // Save changes to the database if necessary
             _appDb.SaveChanges();
            

            // Return a success response
            return Ok("Friendship accepted successfully.");
        }


    }
}
