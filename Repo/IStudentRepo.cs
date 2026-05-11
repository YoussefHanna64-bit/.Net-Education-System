using Education_System.Models;

namespace Education_System.IRepo
{
	public interface IStudentRepo : IGenericRepo<Student>
	{
		List<Student> GetAllWithDepts();
		Student GetByIdWithDepts(int id);
		Student GetByNameWithDepts(string name);
	}
}
