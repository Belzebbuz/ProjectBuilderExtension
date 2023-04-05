using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Serilog;
using System;
using System.IO;

namespace ProjectsUploaderService3._1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if(WindowsServiceHelpers.IsWindowsService())
				Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseWindowsService()
				.UseSerilog((context, config) =>
				{
					config.WriteTo.Console()
						.ReadFrom.Configuration(context.Configuration);
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
