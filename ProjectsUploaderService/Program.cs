using ProjectsUploaderService.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<TcpServerBackgroundService>();
var app = builder.Build();
app.Map("/health", () =>
{
	return "Ok";
});
app.Run();
