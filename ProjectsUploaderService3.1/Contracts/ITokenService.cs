using System.Threading.Tasks;

namespace ProjectsUploaderService3._1.Contracts
{
	public interface ITokenService
	{
		public Task<string> GetTokenAsync(string password);
	}
}
