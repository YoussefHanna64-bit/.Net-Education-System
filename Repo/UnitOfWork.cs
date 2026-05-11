using Education_System.Context;
using Education_System.IRepo;
using Microsoft.EntityFrameworkCore;

namespace Education_System.Repo
{
	public class UnitOfWork : IUnitOfWork
	{
		EduContext db;
		public UnitOfWork(EduContext context)
		{
			db = context;

			Departments = new DepartmentRepo(db);
			Students = new StudentRepo(db);
		}
		public IDepartmentRepo Departments { get; private set; }
		public IStudentRepo Students { get; private set; }

		public int Save()
		{
			return db.SaveChanges();
		}
	}
}
