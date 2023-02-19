using System.Threading.Tasks;

namespace XLoader.Contracts
{
	internal interface IAuthorizeService
	{
		public Task<string> GetTokenAsync(string password);
	}
}
