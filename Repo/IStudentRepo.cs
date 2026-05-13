using Education_System.Models;

namespace Education_System.IRepo
{
	public interface IStudentRepo : IGenericRepo<Student>
	{
		Task<List<Student>> GetAllWithDeptsAsync(CancellationToken ct);
		Student GetByIdWithDepts(int id);
		Student GetByNameWithDepts(string name);
	}
}
