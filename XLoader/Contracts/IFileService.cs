using System.Collections.Generic;
using System.Threading.Tasks;

namespace XLoader.Contracts
{
	public interface IFileService
	{
		public Task<IDictionary<int,string>> GetFiles(string name);
		public Task<string> DownloadAsync(string name);
	}
}
