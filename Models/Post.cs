namespace FinalProject.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Content { get; set; }
        public string? fileName { get; set; }
        public string?fileExt { get; set; }
        public int? count { get; set; } = 0;
        public bool? video { get; set; }
        public bool? image { get; set; }
        public bool? text { get; set; }
        public DateTime? PostDate { get; set; }
        public int? likes { get; set; } = 0;
        public bool? like { get; set; } = false;
    }

  
}
