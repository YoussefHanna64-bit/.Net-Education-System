using Education_System.Context;
using Education_System.IRepo;
using Education_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Education_System.Repo
{
	public class DepartmentRepo : GenericRepo<Department>, IDepartmentRepo
	{
		public DepartmentRepo(EduContext context) : base(context)
		{
		}

		public List<Department> GetAllWithStds()
		{
			return db.Departments.Include(d => d.Students).ToList();
		}

		public Department GetByIdWithStds(int id)
		{
			return db.Departments.Include(d => d.Students).FirstOrDefault(d => d.Id == id)!;
		}
	}
}
