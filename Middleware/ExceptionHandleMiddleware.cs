namespace Education_System.Middleware
{
	public class ExceptionHandleMiddleware
	{
		RequestDelegate _next;

		public ExceptionHandleMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception e)
			{
				context.Response.StatusCode = 500;
				await context.Response.WriteAsync($"Error from request: {context.Request.Path.ToString()}");
			}

		}
	}
}
