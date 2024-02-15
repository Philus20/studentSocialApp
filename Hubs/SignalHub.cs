using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using FinalProject.Models;
using FinalProject.services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinalProject.Hubs
{
    public class SignalHub : Hub
{
        private readonly AppDb _context;
        private readonly ChatServices cs;
        public SignalHub()
        {
            _context = new AppDb();
            cs = new ChatServices();
        }

        public override async Task OnConnectedAsync() { 
        //{
        //    // Get user information (you might need to implement your own logic here)
        //    var user = _context.Users.FirstOrDefault(x => x.UserName == cs.userName);
            
        //    if (user != null) {
        //        user.ConnectionId = Context.ConnectionId;
        //        await _context.SaveChangesAsync();
               
        //    }
        //    else
        //    {
        //        user.ConnectionId = cs.userName; 
        //        await _context.SaveChangesAsync();
        //    }

            // Generate a unique connection ID for the user
            // var connectionId = Context.ConnectionId;

            // Update the user record in the database with the connection ID

            await base.OnConnectedAsync();

        }

        public async Task SendMessage(string name, string user, string message)
        {
           string toId = cs.GetConnectionIdByUser(user);

           
            await Clients.Client(toId).SendAsync("ReceiveMessage", name, user, message);
        }
        public async Task AddUserConnectionId(string name)
        {
          //  cs.AddUserConnectionId(name, Context.ConnectionId);

         
        }

        private string GetUserIdFromContext()
        {
            // Implement your logic to extract user ID from the context (authentication, etc.)
            // For simplicity, assuming the user ID is stored in the ClaimsPrincipal
            return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    
    public async Task SendMessage1(string user)
    {
            cs.AddUserConnectionId(user, Context.ConnectionId);

            await Clients.All.SendAsync("ReceiveMessage1", user);
    }

}
}