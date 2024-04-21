using System;
namespace FinalProject.Models
{
	public class Like
	{
       
            public int Id { get; set; }
            public int postId { get; set; }
            public int userId { get; set; }
        public bool? like { get; set; }
        }
    
}

