using System;
namespace FinalProject.Models
{
	public class filesMessages
	{
	


         public int id { get; set; }

        public string SenderEmail { get; set; }

        public string? ReceiverEmail { get; set; }
        public string? url { get; set; }
        public string? Status { get; set; }
        public string? content { get; set; }
        public DateTime? time { get; set; }
    
	}
}

