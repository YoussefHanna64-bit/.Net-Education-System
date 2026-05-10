using System.ComponentModel.DataAnnotations;

namespace Education_System.DTOs
{
	public class DepartmentInputDTO
	{
		[MinLength(2, ErrorMessage = "Name must be at least 2 chars")]
		public string Name { get; set; }

		public string Location { get; set; }

		[RegularExpression("^01[0125][0-9]{8}$", ErrorMessage = "Invalid phone number")]
		public string PhoneNumber { get; set; }

		public string Manager { get; set; }
	}
}
