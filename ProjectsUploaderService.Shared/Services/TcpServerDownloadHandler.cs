using BeetleX;
using Microsoft.Extensions.Configuration;
using ProjectsUploaderService.Shared.Factories;
using ProjectUpdater.Tcp.Messages;
using Serilog.Core;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace ProjectsUploaderService.Shared.Services
{
	public class TcpServerDownloadHandler : ServerHandlerBase
	{
		private readonly string _releaseFolderPath;
		private readonly string _testDownloadFolderPath;
		private readonly ConcurrentDictionary<string, FileTransfer> _fileStreams = new ConcurrentDictionary<string, FileTransfer>(StringComparer.OrdinalIgnoreCase);
		private readonly IConfiguration _configuration;
		private readonly Logger _logger;
		public TcpServerDownloadHandler(IConfiguration configuration)
		{
			_releaseFolderPath = configuration["ReleasesFolder"];
			_testDownloadFolderPath = configuration["TestBuildsFolder"];
			_configuration = configuration;
			_logger = new LogFactory().CreateLogger(configuration["LogPath"]);
		}

		protected override void OnReceiveMessage(IServer server, ISession session, object message)
		{
			if (message is FileContentBlock block)
			{
				try
				{
					string path = GetDownloadPath(block);
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
					_logger.Error($"{ex.Message}\n{ex.StackTrace}");
					throw ex;
				}

			}
			base.OnReceiveMessage(server, session, message);
		}

		private string GetDownloadPath(FileContentBlock block)
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