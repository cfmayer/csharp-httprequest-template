using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using function;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace root
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.Run(async (context) =>
			{
				if (context.Request.Path != "/")
				{
					context.Response.StatusCode = 404;
					await context.Response.WriteAsync("404 - Not Found");
					return;
				}

				if (context.Request.Method != "POST")
				{
					context.Response.StatusCode = 405;
					await context.Response.WriteAsync("405 - Only POST method allowed");
					return;
				}

				try
				{
					var (status, text) = await new FunctionHandler().Handle(context.Request);
					context.Response.StatusCode = status;
					if (!string.IsNullOrEmpty(text))
						await context.Response.WriteAsync(text);
				}
				catch (NotImplementedException nie)
				{
					context.Response.StatusCode = 501;
					await context.Response.WriteAsync(nie.ToString());
				}
				catch (Exception ex)
				{
					context.Response.StatusCode = 500;
					await context.Response.WriteAsync(ex.ToString());
				}
			});
		}
	}
}