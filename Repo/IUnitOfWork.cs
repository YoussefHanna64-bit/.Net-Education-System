using Education_System.IRepo;

namespace Education_System.Repo
{
	public interface IUnitOfWork
	{
		IDepartmentRepo Departments { get; }
		IStudentRepo Students { get; }
		int Save();
	}
}
