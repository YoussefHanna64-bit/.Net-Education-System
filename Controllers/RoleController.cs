using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Education_System.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		RoleManager<IdentityRole> _roleManager;

		public RoleController(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		[HttpPost]
		public async Task<IActionResult> AddRole(string roleName)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			IdentityRole role = new IdentityRole(roleName);

			IdentityResult res = await _roleManager.CreateAsync(role);

			if (res.Succeeded)
			{
				return Ok(new { msg = "Role added successfully" });
			}

			return BadRequest("Failed to add role");
		}
	}
}
