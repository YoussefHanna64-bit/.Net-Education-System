using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Education_System.Validators;

namespace Education_System.Models
{
	public class Department
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[MinLength(2, ErrorMessage = "Name must be at least 2 chars")]
		[UniqueName]
		public string Name { get; set; }

		public string Location { get; set; }

		[RegularExpression("^01[0125][0-9]{8}$", ErrorMessage = "Invalid phone number")]
		public string PhoneNumber { get; set; }

		public string Manager { get; set; }

		public ICollection<Student> Students { get; set; } = new List<Student>();
	}
}
