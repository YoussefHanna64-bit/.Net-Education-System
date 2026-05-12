using Education_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Education_System.Context
{
	public class EduContext : DbContext
	{

		public EduContext(DbContextOptions op) : base(op)
		{

		}

		public DbSet<Student> Students { get; set; }
		public DbSet<Department> Departments { get; set; }
	}
}