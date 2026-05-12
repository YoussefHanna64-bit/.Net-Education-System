using Education_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Education_System.Context
{
	public class EduContext : IdentityDbContext<ApplicationUser>
	{

		public EduContext(DbContextOptions op) : base(op)
		{

		}

		public DbSet<Student> Students { get; set; }
		public DbSet<Department> Departments { get; set; }
	}
}