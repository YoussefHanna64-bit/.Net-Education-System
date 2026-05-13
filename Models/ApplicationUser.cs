using Microsoft.AspNetCore.Identity;

namespace Education_System.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiryTime { get; set; }
	}
}
