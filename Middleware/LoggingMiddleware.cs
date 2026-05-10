using System.Diagnostics;

namespace Education_System.Middleware
{
	public class LoggingMiddleware
	{
		RequestDelegate _next;
        ILogger<LoggingMiddleware> _logger;

		public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			_logger.LogInformation($"Request Path {context.Request.Path} , method => {context.Request.Method}");
			await _next(context);
			stopwatch.Stop();
			_logger.LogInformation($"Response Status Code {context.Response.StatusCode} , Time Taken => {stopwatch.ElapsedMilliseconds}ms");
		}
	}
}
