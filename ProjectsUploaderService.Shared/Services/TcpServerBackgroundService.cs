using BeetleX;
using BeetleX.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ProjectUpdater.Tcp.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectsUploaderService.Shared.Services
{
	public class TcpServerBackgroundService : BackgroundService
	{
		private readonly IServer _serverDownload;
		public TcpServerBackgroundService(IConfiguration configuration)
		{
			_serverDownload = SocketFactory.CreateTcpServer(new TcpServerDownloadHandler(configuration), new ProtobufPacket());
			_serverDownload.Options.LogLevel = LogType.Warring;
			_serverDownload.Options.BufferSize = 1024 * 8;
			_serverDownload.Options.DefaultListen.Port = int.Parse(configuration["TcpDownloadListenPort"]);
		}
		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Task.Run(() =>
			{
				_serverDownload.Open();
				Thread.Sleep(-1);
			});
			return Task.CompletedTask;
		}
	}
}