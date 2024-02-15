using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using FinalProject.Models;
using FinalProject.services;

namespace FinalProject.Hubs
{
    public class ChatHub: Hub
    {
        private readonly AppDb _context;

        private readonly ChatServices _services;
        public string userName;
        public ChatHub()
        {
            _services = new ChatServices();
            _context = new AppDb();
        }
        //private static readonly Dictionary<string, string> ConnectedUsers = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "come2chat");
            await Clients.Caller.SendAsync("UserConnected");
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "come2chat");
            _services.RemoveUserFromList(Context.User.Identity.Name);
            var onlineUsers = _services.GetOnlineUsers();
            await Clients.Groups("come2chat").SendAsync("OnlinUsers", onlineUsers);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string name)
        {
            _services.AddUserConnectionId(name, Context.ConnectionId);
            var onlineUsers = _services.GetOnlineUsers();
            await Clients.Groups("come2chat").SendAsync("OnlinUsers", onlineUsers);
        }
        public async Task ReceiveMessage(Message message)
        {
            await Clients.Group("come2chat").SendAsync("NewMessage", message);
        }
        public async Task createPrivateChats(Message message)
        {
            try
            {
                 string privateGroupName = GetPrivateGroupName(message.SenderEmail, message.ReceiverEmail);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _services.GetConnectionIdByUser(message.ReceiverEmail);
            await Groups.AddToGroupAsync(toConnectionId, privateGroupName);

            await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in createPrivateChats: {ex.Message}");
                throw; // Rethrow the exception if necessary
            }
           
        }




        public async Task RecievePrivateMessage(Message message)
        {
            string privateGroupName = GetPrivateGroupName(message.SenderEmail, message.ReceiverEmail);
            await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
        }
        private string GetPrivateGroupName(string from, string to)
        {
            var stringCompare = string.CompareOrdinal(from, to) < 0;

            return stringCompare ? $"{from}--{to}" : $"{to}--{from}";
        }

        public async Task RemovePrivateChat(string from, string to)
        {
            string privateGroupName = GetPrivateGroupName(from, to);
            await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _services.GetConnectionIdByUser(to);
            await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
        }
    

}
}


