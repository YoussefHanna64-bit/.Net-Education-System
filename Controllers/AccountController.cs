using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Education_System.DTOs;
using Education_System.Models;
using Microsoft.AspNetCore.Authorization;
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
		private static string GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
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

					var refreshToken = GenerateRefreshToken();
					user.RefreshToken = refreshToken;
					user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
					await _userManager.UpdateAsync(user);

					return Ok(new
					{
						msg = "Login successful",
						token = new JwtSecurityTokenHandler().WriteToken(token),
						refreshToken,
						expires = token.ValidTo,
					});
				}
			}
			return BadRequest(new { msg = "Invalid Email or Password" });
		}

		[HttpPost("logout")]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

			if (userEmail == null)
			{
				return BadRequest("You aren't logged in");
			}

			var user = await _userManager.FindByEmailAsync(userEmail);
			if (user == null)
			{
				return BadRequest("Invalid Email");
			}

			user.RefreshToken = null;
			await _userManager.UpdateAsync(user);

			return Ok(new { msg = "Successfully logged out" });
		}

		[HttpPost("refreshtoken")]
		public async Task<IActionResult> RefreshToken(TokenDTO tokenModel)
		{
			if (tokenModel is null || tokenModel.RefreshToken == null)
			{
				return BadRequest("Error happend");
			}

			var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == tokenModel.RefreshToken);

			if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
			{
				return BadRequest("Expired Refresh Token. Please log in again");
			}

			var newRefreshToken = GenerateRefreshToken();
			user.RefreshToken = newRefreshToken;
			await _userManager.UpdateAsync(user);

			var roles = await _userManager.GetRolesAsync(user);
			var claims = new List<Claim>
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
				token = new JwtSecurityTokenHandler().WriteToken(token),
				refreshToken = newRefreshToken
			});
		}
	}
}
