using BeetleX.Buffers;
using BeetleX;
using BeetleX.Clients;
using ProjectUpdater.Tcp.Messages;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ProjectBuilderExtension.Infrastructure
{
	public class TcpClientFileSender
	{
		public event EventClientError OnClientError;
		public event EventHandler<FileSendedEventArgs> OnBlockSended;
		public event EventHandler<FileSendedEventArgs> OnSendingComplite;

		private readonly ConcurrentDictionary<string, FileReader> _files = new (StringComparer.OrdinalIgnoreCase);
		private readonly AsyncTcpClient _tcpClient;
		private DateTime _startUploadTime;
		private readonly string _sessionGuid = Guid.NewGuid().ToString();
		public TcpClientFileSender(string address, string port)
		{
			BufferPool.BUFFER_SIZE = 1024 * 8;
			_tcpClient = SocketFactory.CreateClient<AsyncTcpClient, ProtobufClientPacket>(address, int.Parse(port));
			_tcpClient.ClientError = (o, err) =>
			{
				var reader = _files[_sessionGuid];
				reader?.Dispose();
				OnClientError?.Invoke(o, err);
			};
		}

		public async Task<bool> IsConnectAsync()
		{
			var connectResult = await _tcpClient.Connect();
			return connectResult.Connected;
		}

		public void Disconnect()
		{
			try
			{
				_tcpClient.DisConnect();
			}
			catch { }
		}

		/// <summary>
		/// Начинает отправку файла
		/// </summary>
		/// <param name="filePath">Путь к файлу</param>
		/// <returns>Количество блоков в файле</returns>
		public int StartSendFile(string filePath)
		{
			var reader = new FileReader(filePath, Guid.Parse(_sessionGuid));
			_files[_sessionGuid] = reader;
			var block = reader.Next();
			block.Completed = OnCompleted;
			_tcpClient.Send(block);
			_startUploadTime = DateTime.Now;
			return reader.BlocksCount;
		}

		private void OnCompleted(FileContentBlock e)
		{
			var reader = _files[_sessionGuid];
			if (!reader.Completed)
			{
				OnBlockSended?.Invoke(this, new FileSendedEventArgs(reader.Index));
				Task.Run(() =>
				{
					var block = reader.Next();
					if (block == null)
						return;
					block.Completed = OnCompleted;
					_tcpClient.Send(block);
				});
			}
			else
			{
				reader.Dispose();
				OnSendingComplite?.Invoke(this, new FileSendedEventArgs(reader.Index) { CompliteTime = DateTime.Now - _startUploadTime});
			}
		}
	}

	public class FileSendedEventArgs : EventArgs
	{
		public FileSendedEventArgs(int blockIndex)
		{
			BlockIndex = blockIndex;
		}

		public int BlockIndex { get; private set; }
		public TimeSpan CompliteTime { get; set; }
	}
}
