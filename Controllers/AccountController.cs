using Education_System.DTOs;
using Education_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Education_System.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		UserManager<ApplicationUser> _userManager;

		public AccountController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync(RegisterDTO userDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			ApplicationUser user = new ApplicationUser();

			user.UserName = userDTO.UserName;
			user.Email = userDTO.Email;

			IdentityResult res = await _userManager.CreateAsync(user, userDTO.Password);

			if (res.Succeeded)
			{
				return Ok(new { msg = "User registered successfully" });
			}

			foreach (var error in res.Errors)
			{
				ModelState.AddModelError(error.Code, error.Description);
			}

			return BadRequest(ModelState);

		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync(LoginDTO userDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			ApplicationUser user = await _userManager.FindByEmailAsync(userDTO.Email);

			if (user != null)
			{
				bool isValid = await _userManager.CheckPasswordAsync(user, userDTO.Password);

				if (isValid)
				{
					return Ok(new { msg = "Login successful" });
				}
			}
			return BadRequest(new { msg = "Invalid Email or Password" });
		}
	}
}
