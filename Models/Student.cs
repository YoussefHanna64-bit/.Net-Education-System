using System.ComponentModel.DataAnnotations.Schema;

namespace Education_System.Models
{
	public class Student
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Name { get; set; }

		public int Age { get; set; }

		public string Address { get; set; }

		public string Email { get; set; }

		public int Level { get; set; }

		public DateTime DateOfBirth { get; set; }
	}
}
