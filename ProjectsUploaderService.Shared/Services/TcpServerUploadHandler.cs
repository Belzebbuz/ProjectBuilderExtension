using BeetleX;
using ProjectUpdater.Tcp.Messages;
using System;
using System.Collections.Concurrent;
using System.IO;
using ProjectsUploaderService.Shared.Settings;
using Microsoft.Extensions.Logging;

namespace ProjectsUploaderService.Shared.Services
{
	public class TcpServerUploadHandler : ServerHandlerBase
	{
		private readonly ILogger<TcpServerUploadHandler> _logger;
		private readonly string _releaseFolderPath;
		private readonly string _testDownloadFolderPath;
		private readonly ConcurrentDictionary<string, FileTransfer> _fileStreams = new ConcurrentDictionary<string, FileTransfer>(StringComparer.OrdinalIgnoreCase);
		public TcpServerUploadHandler(UploadSettings settings, ILogger<TcpServerUploadHandler> logger)
		{
			_logger = logger;
			_releaseFolderPath = settings.ReleasesPath;
			_testDownloadFolderPath = settings.TestPath;
		}

		protected override void OnReceiveMessage(IServer server, ISession session, object message)
		{
			if (message is FileContentBlock block)
			{
				try
				{
					string path = GetUploadPath(block);
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
				catch (Exception ex)
				{
					session?.Dispose();
					_logger.LogError(ex.Message);
				}

			}
			base.OnReceiveMessage(server, session, message);
		}

		private string GetUploadPath(FileContentBlock block)
		{
			return block.FileName.ToLower().StartsWith("test") 
				? Path.Combine(_testDownloadFolderPath, block.FileName)
				: Path.Combine(_releaseFolderPath, block.FileName);
		}

		private FileTransfer HandleFirstBlock(FileContentBlock block, string path, FileTransfer value)
		{
			if (value != null)
			{
				value.Dispose();
			}
			var destinationFolder = Path.GetDirectoryName(path);
			if (!Directory.Exists(destinationFolder))
				Directory.CreateDirectory(destinationFolder);

			value = new FileTransfer(path);
			_fileStreams[block.AppId.ToString()] = value;
			return value;
		}
	}
}