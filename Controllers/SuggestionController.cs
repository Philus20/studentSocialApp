using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuggestionController : Controller
    {
        private readonly AppDb _appDb;

        public SuggestionController()
        {
            _appDb = new AppDb();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Friend(int id)
        {
            var loginStudent = await _appDb.Students.FindAsync(id);

            // Load friends who sent friend requests from the database
            var friendList = await _appDb.Friendships
                .Where(friendSuggestion =>
                    loginStudent.Id == friendSuggestion.ReceiverUserId &&
                    friendSuggestion.Status == 1)
                .ToListAsync();

            // Retrieve additional information about the friends from the Student table
            var friendDetails = friendList
                .Select(friendship => _appDb.Students.Find(friendship.SenderUserId))
                .ToList();

            return Ok(friendDetails);
        }



        
        [HttpGet("{id}/circle")]
        public async Task<IActionResult> Circle(int id)
        {
            var loginStudent = await _appDb.Students.FindAsync(id);

            // Find friends who share the same program but have no existing friend relationship
            var friendList = await _appDb.Students
                .Where(student =>
                    student.Id != loginStudent.Id &&
                    student.Programme == loginStudent.Programme &&
                    !_appDb.Friendships
                        .Where(friendship =>
                            (friendship.SenderUserId == loginStudent.Id && friendship.ReceiverUserId == student.Id) ||
                            (friendship.SenderUserId == student.Id && friendship.ReceiverUserId == loginStudent.Id))
                        .Any())
                .ToListAsync();

            return Ok(friendList);
        }

        [HttpGet("{id}/chat")]
        public async Task<IActionResult> Chats(int id)
        {
            var loginStudent = await _appDb.Students.FindAsync(id);

            // Find friend IDs where either SenderUserId or ReceiverUserId is the login user and Status is 1
            var friendIds = await _appDb.Friendships
                .Where(friendship =>
                      
                    (friendship.SenderUserId == loginStudent.Id || friendship.ReceiverUserId == loginStudent.Id) &&
                    friendship.Status == 1)
                .Select(friendship => friendship.SenderUserId == loginStudent.Id ? friendship.ReceiverUserId : friendship.SenderUserId)
                .ToListAsync();

            // Use the list of friend IDs to retrieve corresponding Student objects
            var friendList = await _appDb.Students
                .Where(student => friendIds.Contains(student.Id) && student.Id != loginStudent.Id )
                .ToListAsync();

            return Ok(friendList);
        }
        [HttpGet("{id}/pen")]
        public async Task<IActionResult> Pending(int id)
        {
            var loginStudent = await _appDb.Students.FindAsync(id);

            // Find friends who share the same program and have a pending friend relationship (Status == 0)
            var friendList = await _appDb.Students
                .Where(student =>
                    student.Id != loginStudent.Id &&
                  
                    _appDb.Friendships
                        .Where(friendship =>
                            
                            (friendship.SenderUserId == student.Id && friendship.ReceiverUserId == loginStudent.Id))
                        .Any(friendship => friendship.Status == 0))
                .ToListAsync();

            return Ok(friendList);
        }





    }
}
