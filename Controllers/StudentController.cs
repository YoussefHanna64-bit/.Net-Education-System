using System.IO;
using Education_System.Context;
using Education_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
			var students = db.Students.ToList();
			return Ok(new { msg = "success", data = students });
		}

		[HttpGet("{id:int}")]
		public IActionResult getStudentByID(int id)
		{
			if (id == 0)
			{
				throw new Exception("ID can't be zero");
			}

			var st = db.Students.FirstOrDefault(s => s.Id == id);

			if (st == null)
			{
				return NotFound(new { msg = "Student Not Found" });
			}
			return Ok(new { msg = "success", data = st });
		}


		[HttpGet("{name:alpha}")]
		public IActionResult getStudentByName(string name)
		{
			var st = db.Students.FirstOrDefault(s => s.Name == name);

			if (st == null)
			{
				return NotFound(new { msg = "Student Not Found" });
			}
			return Ok(new { msg = "success", data = st });
		}

		[HttpPost]
		public async Task<IActionResult> addStudent([FromForm] Student st)
		{
			if (st.ImageFile != null)
			{
				st.ImagePath = await UploadImage(st.ImageFile);
			}

			db.Students.Add(st);
			db.SaveChanges();

			return CreatedAtAction(nameof(getStudentByID), new { id = st.Id }, new { msg = "success", data = st });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> updateStudent(int id, [FromForm] Student st)
		{
			if (st.ImageFile != null)
			{
				st.ImagePath = await UploadImage(st.ImageFile);
			}

			var student = db.Students.FirstOrDefault(s => s.Id == id);

			if (student == null)
			{
				return NotFound(new { msg = "Student Not Found" });
			}
			student.Name = st.Name;
			student.Age = st.Age;
			student.Address = st.Address;
			student.Email = st.Email;
			student.Level = st.Level;
			student.DateOfBirth = st.DateOfBirth;

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
