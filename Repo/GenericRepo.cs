using Education_System.Context;
using Microsoft.EntityFrameworkCore;

namespace Education_System.IRepo
{
	public class GenericRepo<T> : IGenericRepo<T> where T : class
	{

		protected readonly EduContext db;

		public GenericRepo(EduContext context)
		{
			db = context;
		}

		public List<T> GetAll()
		{
			return db.Set<T>().ToList();
		}

		public T GetById(int id)
		{
			return db.Set<T>().Find(id)!;
		}

		public void Add(T t)
		{
			db.Set<T>().Add(t);
		}

		public void Update(T t)
		{
			db.Update(t);
		}

		public void Delete(T t)
		{
			db.Set<T>().Remove(t);
		}

	}
}
