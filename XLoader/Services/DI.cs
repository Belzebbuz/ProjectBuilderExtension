using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using XLoader.Contracts;

namespace XLoader.Services
{
	internal static class DI
	{
		public static IServiceProvider Services { get; private set; }

		public static IServiceProvider Init(Options options)
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddTransient<IAuthorizeService, AuthorizeService>();
			serviceCollection.AddSingleton(options);
			serviceCollection.AddTransient<IFileService, FileService>();
			serviceCollection.AddTransient<AuthenticationHeaderHandler>();
			serviceCollection.AddHttpClient("Server", client =>
				{
					client.BaseAddress = new Uri(options.ServerUrl);
					client.Timeout = Timeout.InfiniteTimeSpan;
				})
				.AddHttpMessageHandler<AuthenticationHeaderHandler>();
			Services = serviceCollection.BuildServiceProvider();
			return Services;
		}
	}
}
