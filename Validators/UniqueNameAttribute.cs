using System.ComponentModel.DataAnnotations;
using Education_System.Context;

namespace Education_System.Validators
{
	public class UniqueNameAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			string name = (string)value!;
			var db = (EduContext)validationContext.GetService(typeof(EduContext))!;

			var dept = db.Departments.FirstOrDefault(d => d.Name == name);

			if (dept != null)
			{
				return new ValidationResult("Department name must be unique");
			}

			return ValidationResult.Success;
		}
	}
}
