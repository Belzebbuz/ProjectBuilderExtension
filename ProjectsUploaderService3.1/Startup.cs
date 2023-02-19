using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectsUploaderService.Shared.Services;
using ProjectsUploaderService.Shared.Settings;
using ProjectsUploaderService3._1.Contracts;
using ProjectsUploaderService3._1.Services;
using ProjectsUploaderService3._1.Services.Settings;

namespace ProjectsUploaderService3._1
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var securitySettings = Configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>();
			var uploadSettings = Configuration.GetSection(nameof(UploadSettings)).Get<UploadSettings>();
			services.AddHostedService<TcpServerBackgroundService>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddSingleton(uploadSettings);
			services.AddSingleton(securitySettings);
			services.AddAuth(securitySettings);
			services.AddControllers();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
