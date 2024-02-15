namespace FinalProject.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public int Status { get; set; } // 0: Pending, 1: Accepted, 2: Rejected
    }

}
