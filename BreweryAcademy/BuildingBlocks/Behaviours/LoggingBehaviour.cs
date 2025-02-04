using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
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

                // Create a LogMessage object
                var logMessage = new LogMessage
                {
                    Level = "Info",
                    Message = $"Request: {context.Request.Method} {context.Request.Path}",
                    RequestBody = requestBody,
                    ResponseBody = responseBody,
                    StatusCode = context.Response.StatusCode,
                    ElapsedMilliseconds = timelapse.Milliseconds,
                    Timestamp = DateTime.UtcNow
                };

                // Send the log message to Logstash
                await SendLogToLogstash(logMessage);

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

        private async Task SendLogToLogstash(LogMessage logMessage)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonSerializer.Serialize(logMessage);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5514", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Failed to send log to Logstash. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending log to Logstash.");
            }
        }
    }
}