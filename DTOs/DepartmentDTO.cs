namespace Education_System.DTOs
{
	public class DepartmentDTO
	{
		public string Name { get; set; }
		public List<string> Students { get; set; } = new List<string>();
		public int Count { get; set; }
		public string Message { get; set; }
	}
}
