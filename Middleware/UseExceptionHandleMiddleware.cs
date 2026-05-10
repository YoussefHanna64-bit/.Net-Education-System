namespace Education_System.Middleware
{
	public static class UseExceptionHandleMiddleware
	{
		public static IApplicationBuilder UseExceptionHandle(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandleMiddleware>();
		}
	}
}
