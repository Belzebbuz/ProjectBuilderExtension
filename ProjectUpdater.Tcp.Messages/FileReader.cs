using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace ProjectUpdater.Tcp.Messages
{
	public class FileReader : IDisposable
	{
		public FileReader(string file, Guid appId)
		{
			_fileInfo = new FileInfo(file);
			AppId = appId;
			_blocksCount = (int)(_fileInfo.Length / _blockSize);
			if (_fileInfo.Length % _blockSize > 0)
				_blocksCount++;
			FileSize = _fileInfo.Length;
			_buffer = new byte[_blockSize];
			_memoryReader = _fileInfo.OpenRead();
		}

		private Guid AppId;

		private Stream _memoryReader;

		private byte[] _buffer;

		private FileInfo _fileInfo;

		private int _blocksCount;

		private int _blockSize = 1024 * 16;

		private int _blockIndex;

		public int Index => _blockIndex;

		public int Size => _blockSize;

		public int BlocksCount => _blocksCount;

		public long FileSize { get; private set; }

		public long CompletedSize { get; private set; }

		public bool Completed => _blockIndex == _blocksCount;

		public FileContentBlock Next()
		{
			FileContentBlock result = new FileContentBlock();
			result.FileName = _fileInfo.Name;
			result.AppId = AppId;
			byte[] data;
			if (_blockIndex == _blocksCount - 1)
			{
				data = new byte[_fileInfo.Length - _blockIndex * _blockSize];
				result.Eof = true;
			}
			else
			{
				data = _buffer;
			}
			CompletedSize += data.Length;

			if (_memoryReader.CanRead)
			{
				_memoryReader.Read(data, 0, data.Length);
			}
			else
			{
				return null;
			}
			result.Index = _blockIndex;
			result.Data = data;
			_blockIndex++;


			return result;
		}

		public void Dispose()
		{
			_memoryReader.Dispose();
		}
	}

}
