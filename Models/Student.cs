namespace FinalProject.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        public string email { get; set; }
        public string password { get; set; }
        public string? firstName { get; set; }
        public string?surname { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string? profilePictureName { get; set; }
        public DateTime? registrationDate { get; set; }
        public string? Programme { get; set; }
    }
}
