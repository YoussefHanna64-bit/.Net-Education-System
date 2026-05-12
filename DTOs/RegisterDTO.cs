using System.ComponentModel.DataAnnotations;

namespace Education_System.DTOs
{
	public class RegisterDTO
	{
		public string UserName { get; set; }

		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		public string Password { get; set; }

		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; }

		public string? Role { get; set; }
	}
}
