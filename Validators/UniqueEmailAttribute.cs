using System.ComponentModel.DataAnnotations;
using Education_System.Context;
using Education_System.Models;

namespace Education_System.Validators
{
	public class UniqueEmailAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			string email = (string)value!;
			var st = validationContext.ObjectInstance as Student;

			var db = (EduContext)validationContext.GetService(typeof(EduContext))!;
			var student = db.Students.FirstOrDefault(s => s.Email == email && s.Id != st!.Id);

			if (student != null)
			{
				return new ValidationResult("Email must be unique");
			}

			return ValidationResult.Success;
		}
	}
}
