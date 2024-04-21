using System;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Models

{
    public class Message
    {
         public int id { get; set; }
        
        public string SenderEmail { get; set; }

        public string? ReceiverEmail { get; set; }
        public string? Subject { get; set; }
        public string? Status { get; set; }
        public string? file { get; set; }
        public string ? isFile { get; set; }
        public string? ext { get; set; }
        public DateTime? time { get; set; }
    }
}
