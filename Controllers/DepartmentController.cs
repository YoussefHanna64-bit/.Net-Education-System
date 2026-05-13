using Education_System.Context;
using Education_System.DTOs;
using Education_System.Models;
using Education_System.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Education_System.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class DepartmentController : ControllerBase
	{
		IUnitOfWork uw;

		public DepartmentController(IUnitOfWork unitOfWork)
		{
			uw = unitOfWork;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult getAllDepartments()
		{
			var depts = uw.Departments.GetAllWithStds();

			if (depts == null)
			{
				return NotFound(new { msg = "No Departments Found" });
			}

			List<DepartmentDTO> deptDTOs = new List<DepartmentDTO>();

			foreach (var d in depts)
			{
				deptDTOs.Add(new DepartmentDTO
				{
					Name = d.Name,
					Students = d.Students.Select(st => st.Name).ToList(),
					Count = d.Students.Count,
					Message = d.Students.Count > 1 ? "Overloaded" : "Normal"
				});
			}

			return Ok(new { msg = "success", data = deptDTOs });
		}

		[HttpGet("{id:int}")]
		public IActionResult getDepartmentByID(int id)
		{

			var dept = uw.Departments.GetByIdWithStds(id);

			if (dept == null)
			{
				return NotFound(new { msg = "Department Not Found" });
			}

			DepartmentDTO deptDTO = new DepartmentDTO
			{
				Name = dept.Name,
				Students = dept.Students.Select(st => st.Name).ToList(),
				Count = dept.Students.Count,
				Message = dept.Students.Count > 1 ? "Overloaded" : "Normal"
			};

			return Ok(new { msg = "success", data = deptDTO });
		}

		[HttpPost]
		public IActionResult addDepartment(DepartmentInputDTO dto)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var dept = new Department
			{
				Name = dto.Name,
				Location = dto.Location,
				PhoneNumber = dto.PhoneNumber,
				Manager = dto.Manager
			};
			uw.Departments.Add(dept);
			uw.Save();

			return CreatedAtAction(nameof(getDepartmentByID), new { id = dept.Id }, new { msg = "success", data = dept });
		}

		[HttpPut("{id}")]
		public IActionResult updateDepartment(int id, DepartmentInputDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var department = uw.Departments.GetById(id);

			if (department == null)
			{
				return NotFound(new { msg = "Department Not Found" });
			}

			department.Name = dto.Name;
			department.Location = dto.Location;
			department.PhoneNumber = dto.PhoneNumber;
			department.Manager = dto.Manager;

			uw.Save();

			return Ok(new { msg = "success", data = department });
		}

		[HttpDelete("{id}")]
		public IActionResult deleteDepartment(int id)
		{
			var dept = uw.Departments.GetById(id);

			if (dept == null)
			{
				return NotFound(new { msg = "Department Not Found" });
			}

			uw.Departments.Delete(dept);
			uw.Save();

			return Ok(new { msg = "success" });
		}
	}
}
