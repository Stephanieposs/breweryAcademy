using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
			context.Request.EnableBuffering();

			var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

			_logger.LogInformation("RequestMethod: {Method} - Route: {Path} - Body: {body}", context.Request.Method, context.Request.Path, requestBody);

			context.Request.Body.Position = 0;
			await _next(context);

			_logger.LogInformation("ResponseStatus: {StatusCode} - Route: {Route}", context.Response.StatusCode, context.Request.Path);
		}
	}

}
