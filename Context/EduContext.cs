using Education_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Education_System.Context
{
	public class EduContext : DbContext
	{

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=.;Database=Education System;Trusted_Connection=True;Encrypt=False;");
		}

		public DbSet<Student> Students { get; set; }
	}
}