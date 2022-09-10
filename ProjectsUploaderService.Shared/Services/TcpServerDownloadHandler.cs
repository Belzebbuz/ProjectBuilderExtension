using BeetleX;
using ProjectUpdater.Tcp.Messages;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace ProjectsUploaderService.Shared.Services
{
	public class TcpServerDownloadHandler : ServerHandlerBase
	{
		private readonly string _downloadFolderPath;
		private readonly ConcurrentDictionary<string, FileTransfer> _fileStreams = new ConcurrentDictionary<string, FileTransfer>(StringComparer.OrdinalIgnoreCase);
		public TcpServerDownloadHandler(string downloadFolderPath)
		{
			_downloadFolderPath = downloadFolderPath;
		}

		protected override void OnReceiveMessage(IServer server, BeetleX.ISession session, object message)
		{
			if (message is FileContentBlock block)
			{
				if (Path.GetExtension(block.FileName) != ".zip")
				{
					session.Dispose();
					return;
				}

				var path = Path.Combine(_downloadFolderPath, $"{block.FileName}");
				_fileStreams.TryGetValue(block.AppId.ToString(), out FileTransfer value);

				if (block.Index == 0)
				{
					value = HandleFirstBlock(block, path, value);
				}

				value?.Stream.Write(block.Data, 0, block.Data.Length);

				if (block.Eof)
				{
					value?.Dispose();
					_fileStreams.TryRemove(block.AppId.ToString(), out value);
				}
			}
			base.OnReceiveMessage(server, session, message);
		}

		private FileTransfer HandleFirstBlock(FileContentBlock block, string path, FileTransfer value)
		{
			if (value != null)
			{
				value.Dispose();
			}
			if (!Directory.Exists(_downloadFolderPath))
				Directory.CreateDirectory(_downloadFolderPath);

			value = new FileTransfer(path);
			_fileStreams[block.AppId.ToString()] = value;
			return value;
		}
	}
}