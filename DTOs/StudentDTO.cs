namespace Education_System.DTOs
{
	public class StudentDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public int Level { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string? ImagePath { get; set; }
		public string Department { get; set; }
		public string Message { get; set; }
	}
}
