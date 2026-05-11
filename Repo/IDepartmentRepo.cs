using Education_System.Models;

namespace Education_System.IRepo
{
	public interface IDepartmentRepo : IGenericRepo<Department>
	{
		List<Department> GetAllWithStds();
		Department GetByIdWithStds(int id);
	}
}
