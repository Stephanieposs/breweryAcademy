using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviours 
{
	public class LoggingBehaviour
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<LoggingBehaviour> _logger;

		public LoggingBehaviour(RequestDelegate next, ILogger<LoggingBehaviour> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string requestBody = await ReadRequestBody(context.Request);
			
			var originalBodyStream = context.Response.Body;
			using (var memoryStream = new MemoryStream())
			{
				context.Response.Body = memoryStream;

				var stopWatch = new Stopwatch();
				stopWatch.Start();
				await _next(context);
				stopWatch.Stop();

				var timelapse = stopWatch.Elapsed;

				memoryStream.Position = 0;
				string responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

				memoryStream.Position = 0;
				await memoryStream.CopyToAsync(originalBodyStream); 

				context.Response.Body = originalBodyStream;

				_logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} {requestBody}");
				_logger.LogInformation($"Response: {context.Response.StatusCode} {responseBody}");
				_logger.LogInformation("Request concluded in {time} ms",timelapse.Milliseconds);
			}
		}

		private async Task<string> ReadRequestBody(HttpRequest request)
		{
			request.EnableBuffering(); 

			using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
			{
				var body = await reader.ReadToEndAsync();
				request.Body.Position = 0; 
				return body;
			}
		}
	}
}