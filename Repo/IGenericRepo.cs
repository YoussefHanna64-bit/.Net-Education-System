namespace Education_System.IRepo
{
	public interface IGenericRepo<T> where T : class
	{
		List<T> GetAll();
		T GetById(int id);
		void Add(T t);
		void Update(T t);
		void Delete(T t);
	}
}
