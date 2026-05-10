using Education_System.Context;
using Education_System.DTOs;
using Education_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Education_System.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentController : ControllerBase
	{
		EduContext db;

		public DepartmentController()
		{
			db = new EduContext();
		}

		[HttpGet]
		public IActionResult getAllDepartments()
		{
			var depts = db.Departments.Include(d => d.Students).ToList();

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

			var dept = db.Departments.Include(d => d.Students).FirstOrDefault(d => d.Id == id);

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
			db.Departments.Add(dept);
			db.SaveChanges();

			return CreatedAtAction(nameof(getDepartmentByID), new { id = dept.Id }, new { msg = "success", data = dept });
		}

		[HttpPut("{id}")]
		public IActionResult updateDepartment(int id, DepartmentInputDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var department = db.Departments.FirstOrDefault(d => d.Id == id);

			if (department == null)
			{
				return NotFound(new { msg = "Department Not Found" });
			}

			department.Name = dto.Name;
			department.Location = dto.Location;
			department.PhoneNumber = dto.PhoneNumber;
			department.Manager = dto.Manager;

			db.SaveChanges();

			return Ok(new { msg = "success", data = department });
		}

		[HttpDelete("{id}")]
		public IActionResult deleteDepartment(int id)
		{
			var dept = db.Departments.FirstOrDefault(d => d.Id == id);

			if (dept == null)
			{
				return NotFound(new { msg = "Department Not Found" });
			}

			db.Departments.Remove(dept);
			db.SaveChanges();

			return Ok(new { msg = "success" });
		}
	}
}
