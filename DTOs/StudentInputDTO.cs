using System.ComponentModel.DataAnnotations;
using Education_System.Validators;

namespace Education_System.DTOs
{
	public class StudentInputDTO
	{
		[MinLength(6, ErrorMessage = "Name must be at least 6 chars")]
		[MaxLength(15, ErrorMessage = "Name must be at most 15 chars")]
		[RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must contain only letters and spaces")]
		public string Name { get; set; }

		[Range(14, 18, ErrorMessage = "Age must be between 14 and 18")]
		public int Age { get; set; }

		public string Address { get; set; }

		[UniqueEmail]
		public string Email { get; set; }

		public int Level { get; set; }

		[DateInPast]
		public DateTime DateOfBirth { get; set; }

		public int? DeptId { get; set; }

		public IFormFile? ImageFile { get; set; }
	}
}
