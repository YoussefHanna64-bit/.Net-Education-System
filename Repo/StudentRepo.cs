using Education_System.Context;
using Education_System.IRepo;
using Education_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Education_System.Repo
{
	public class StudentRepo : GenericRepo<Student>, IStudentRepo
	{
		public StudentRepo(EduContext context) : base(context)
		{
		}

		public async Task<List<Student>> GetAllWithDeptsAsync(CancellationToken ct)
		{
			return await db.Students.Include(s => s.Department).ToListAsync(ct);
		}

		public Student GetByIdWithDepts(int id)
		{
			return db.Students.Include(s => s.Department).FirstOrDefault(s => s.Id == id)!;
		}

		public Student GetByNameWithDepts(string name)
		{
			return db.Students.Include(s => s.Department).FirstOrDefault(s => s.Name == name)!;
		}
	}
}
