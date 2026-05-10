using Microsoft.AspNetCore.Mvc.Filters;

namespace Education_System.Filters
{
	public class CustomResultFilter : IResultFilter
	{
		public void OnResultExecuting(ResultExecutingContext context)
		{
			context.HttpContext.Response.Headers.Append("Response-Time", DateTime.Now.ToString());
		}

		public void OnResultExecuted(ResultExecutedContext context)
		{

		}


	}
}
