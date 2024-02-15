namespace FinalProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string ? ConnectionId { get; set; } // Add this property for ConnectionId

    }
}
