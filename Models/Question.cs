using System;
namespace FinalProject.Models
{
	public class Question
	{
		public int id { get; set; }
		public string firstName { get; set; }
		public string surname { get; set; }
		public int comment { get; set; } = 0;
		public string content { get; set; }
	public int index { get; set; }
	   public string profilePictureName { get; set; }
		public DateTime? questionDate { get; set; }
	}

	public class QuestionComment
	{
		public int id { get; set; }

		public string firstName { get; set; }
		public string surname { get; set; }
		public string profilePictureName { get; set; }
		public string content { get; set; }
		public int questionId { get; set; }
		public int? index { get; set; }
		public DateTime? commentDate { get; set; }
	}
}

