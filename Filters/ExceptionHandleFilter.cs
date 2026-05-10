using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Education_System.Filters
{
	public class ExceptionHandleFilter : Attribute, IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			ContentResult res = new ContentResult();

			res.ContentType = "text/plain";
			res.Content = $"Error filter: {context.Exception.Message}";
			res.StatusCode = 500;

			context.Result = res;
		}
	}
}
