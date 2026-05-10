using System.IO;
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
	public class StudentController : ControllerBase
	{
		EduContext db;

		public StudentController()
		{
			db = new EduContext();
		}
		private async Task<string> UploadImage(IFormFile file)
		{

			string filePath = Path.Combine("wwwroot/images", file.FileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return file.FileName;
		}

		[HttpGet]
		public IActionResult getAllStudents()
		{
			var st = db.Students.Include(d => d.Department).ToList();
			if (st is null)
			{
				return NotFound("No Students Found");
			}

			List<StudentDTO> stDTOs = new List<StudentDTO>();

			foreach (var s in st)
			{
				stDTOs.Add(new StudentDTO
				{
					Id = s.Id,
					Name = s.Name,
					Address = s.Address,
					Email = s.Email,
					Age = s.Age,
					Level = s.Level,
					DateOfBirth = s.DateOfBirth,
					ImagePath = s.ImagePath,
					Department = s.Department?.Name,
					Message = $"Student {s.Name} is in department {s.DeptId}"
				});
			}

			return Ok(new { msg = "success", data = stDTOs });
		}

		[HttpGet("{id:int}")]
		public IActionResult getStudentByID(int id)
		{
			// if (id == 0)
			// {
			// 	throw new Exception("ID can't be zero");
			// }

			var st = db.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id);

			if (st == null)
			{
				return NotFound(new { msg = "Student Not Found" });
			}

			StudentDTO stDTO = new StudentDTO
			{
				Id = st.Id,
				Name = st.Name,
				Age = st.Age,
				Address = st.Address,
				Email = st.Email,
				Level = st.Level,
				DateOfBirth = st.DateOfBirth,
				ImagePath = st.ImagePath,
				Department = st.Department?.Name,
				Message = $"Student {st.Name} is in department {st.DeptId}"
			};

			return Ok(new { msg = "success", data = stDTO });
		}


		[HttpGet("{name:alpha}")]
		public IActionResult getStudentByName(string name)
		{
			var st = db.Students.Include(s => s.Department).FirstOrDefault(s => s.Name == name);

			if (st == null)
			{
				return NotFound(new { msg = "Student Not Found" });
			}

			StudentDTO stDTO = new StudentDTO
			{
				Id = st.Id,
				Name = st.Name,
				Age = st.Age,
				Address = st.Address,
				Email = st.Email,
				Level = st.Level,
				DateOfBirth = st.DateOfBirth,
				ImagePath = st.ImagePath,
				Department = st.Department?.Name,
				Message = $"Student {st.Name} is in department {st.DeptId}"
			};

			return Ok(new { msg = "success", data = stDTO });
		}

		[HttpPost]
		public async Task<IActionResult> addStudent([FromForm] StudentInputDTO stDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var st = new Student
			{
				Name = stDTO.Name,
				Age = stDTO.Age,
				Address = stDTO.Address,
				Email = stDTO.Email,
				Level = stDTO.Level,
				DateOfBirth = stDTO.DateOfBirth,
				DeptId = stDTO.DeptId,
			};

			if (stDTO.ImageFile != null)
			{
				st.ImagePath = await UploadImage(stDTO.ImageFile);
			}

			db.Students.Add(st);
			db.SaveChanges();

			return CreatedAtAction(nameof(getStudentByID), new { id = st.Id }, new { msg = "success", data = st });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> updateStudent(int id, [FromForm] StudentInputDTO stDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var st = db.Students.FirstOrDefault(s => s.Id == id);

			if (st == null)
			{
				return NotFound(new { msg = "Student Not Found" });
			}

			st.Name = stDTO.Name;
			st.Age = stDTO.Age;
			st.Address = stDTO.Address;
			st.Email = stDTO.Email;
			st.Level = stDTO.Level;
			st.DateOfBirth = stDTO.DateOfBirth;
			st.DeptId = stDTO.DeptId;

			if (stDTO.ImageFile != null)
			{
				st.ImagePath = await UploadImage(stDTO.ImageFile);
			}

			db.SaveChanges();

			return Ok(new { msg = "success", data = st });
		}


		[HttpDelete("{id}")]
		public IActionResult deleteStudent(int id)
		{
			var st = db.Students.FirstOrDefault(s => s.Id == id);

			if (st == null)
			{
				return NotFound(new { msg = "Student Not Found" });
			}
			db.Students.Remove(st);
			db.SaveChanges();

			return Ok(new { msg = "success" });
		}
	}
}
