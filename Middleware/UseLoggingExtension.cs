namespace Education_System.Middleware
{
	public static class UseLoggingExtension
	{
		public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<LoggingMiddleware>();
		}
	}
}
