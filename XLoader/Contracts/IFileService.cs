using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XLoader.Contracts
{
	public interface IFileService
	{
		public Task<IDictionary<int,string>> GetFilesAsync(string name, bool isTest);
		public Task<string> DownloadAsync(string name, bool isTest);
	}
}
