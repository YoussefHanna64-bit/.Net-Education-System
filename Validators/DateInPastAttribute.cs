using System.ComponentModel.DataAnnotations;
using Education_System.Models;

namespace Education_System.Validators
{
	public class DateInPastAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			DateTime date = (DateTime)value!;

			if (date > DateTime.Now)
			{
				return new ValidationResult("Date of birth must be in the past");
			}

			var st = validationContext.ObjectInstance as Student;
			var age = st?.Age;

			if (date > DateTime.Now.AddYears(-age!.Value))
			{
				return new ValidationResult("Date of birth does not match the age");
			}

			return ValidationResult.Success;
		}
	}
}
