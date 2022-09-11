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
			mInfo = new FileInfo(file);
			AppId = appId;
			mPages = (int)(mInfo.Length / mSize);
			if (mInfo.Length % mSize > 0)
				mPages++;
			FileSize = mInfo.Length;
			mBuffer = new byte[mSize];
			mReader = mInfo.OpenRead();
		}

		private Guid AppId;

		private Stream mReader;

		private byte[] mBuffer;

		private FileInfo mInfo;

		private int mPages;

		private int mSize = 1024 * 16;

		private int mIndex;

		public int Index => mIndex;

		public int Size => mSize;

		public int Pages => mPages;

		public long FileSize { get; private set; }

		public long CompletedSize { get; private set; }

		public bool Completed => mIndex == mPages;

		public FileContentBlock Next()
		{
			FileContentBlock result = new FileContentBlock();
			result.FileName = mInfo.Name;
			result.AppId = AppId;
			byte[] data;
			if (mIndex == mPages - 1)
			{
				data = new byte[mInfo.Length - mIndex * mSize];
				result.Eof = true;
			}
			else
			{
				data = mBuffer;
			}
			CompletedSize += data.Length;

			if (mReader.CanRead)
			{
				mReader.Read(data, 0, data.Length);
			}
			else
			{
				return null;
			}
			result.Index = mIndex;
			result.Data = data;
			mIndex++;


			return result;
		}

		public void Dispose()
		{
			mReader.Dispose();
		}
	}

}
