using BeetleX;
using BeetleX.EventArgs;
using Microsoft.Extensions.Hosting;
using ProjectUpdater.Tcp.Messages;
using System.Threading;
using System.Threading.Tasks;
using ProjectsUploaderService.Shared.Settings;
using Microsoft.Extensions.Logging;

namespace ProjectsUploaderService.Shared.Services
{
	public class TcpServerBackgroundService : BackgroundService
	{
		private readonly IServer _serverDownload;
		public TcpServerBackgroundService(UploadSettings settings, ILogger<TcpServerUploadHandler> logger)
		{
			_serverDownload = SocketFactory.CreateTcpServer(new TcpServerUploadHandler(settings, logger), new ProtobufPacket());
			_serverDownload.Options.LogLevel = LogType.Warring;
			_serverDownload.Options.BufferSize = 1024 * 8;
			_serverDownload.Options.DefaultListen.Port = settings.UploadListenPort;
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