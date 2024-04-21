using System;
namespace FinalProject.Models
{
	public class StudentComment
    {
        public int Id { get; set; }
        public string? firstName { get; set; }
        public string? surname { get; set; }
      
        public string? profilePictureName { get; set; }
        
        public string? Programme { get; set; }
        public int comentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
        //public bool d { get; set; }
    }
}

