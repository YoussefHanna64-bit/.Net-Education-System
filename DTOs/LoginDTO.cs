using System.ComponentModel.DataAnnotations;

namespace Education_System.DTOs
{
	public class LoginDTO
	{
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
