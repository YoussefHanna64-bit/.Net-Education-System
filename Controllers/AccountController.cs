using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Education_System.DTOs;
using Education_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
		public async Task<IActionResult> Register(RegisterDTO userDTO)
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
				string role = "Student";

				if (userDTO.Role == "Admin" && User.IsInRole("Admin"))
				{
					role = "Admin";
				}

				await _userManager.AddToRoleAsync(user, role);

				return Ok(new { msg = "User registered successfully" });
			}

			foreach (var error in res.Errors)
			{
				ModelState.AddModelError(error.Code, error.Description);
			}

			return BadRequest(ModelState);

		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDTO userDTO)
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
					var roles = await _userManager.GetRolesAsync(user);

					List<Claim> claims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, user.Id),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					};

					foreach (var role in roles)
					{
						claims.Add(new Claim(ClaimTypes.Role, role));
					}

					SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("ff996dbb717b65380d2bc55fc9a2b570eed98a8699715f400828a615bd1712c0"));

					SigningCredentials sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

					JwtSecurityToken token = new JwtSecurityToken(
						issuer: "https://localhost:44360/",
						audience: "https://localhost:44360/",
						expires: DateTime.Now.AddHours(1),
						claims: claims,
						signingCredentials: sc
					);

					return Ok(new
					{
						msg = "Login successful",
						token = new JwtSecurityTokenHandler().WriteToken(token),
						expires = token.ValidTo
					});
				}
			}
			return BadRequest(new { msg = "Invalid Email or Password" });
		}
	}
}
