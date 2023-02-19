using System;
using System.Net.Http;
using System.Threading.Tasks;
using XLoader.Contracts;

namespace XLoader.Services
{
	internal class AuthorizeService : IAuthorizeService
	{
		private readonly HttpClient _client;

		public AuthorizeService(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient("Server");
		}
		public async Task<string> GetTokenAsync(string password)
		{
			try
			{
				var response = await _client.PostAsJsonAsync("token", new { Password = password });
				var result = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					return result;
				}

				LogError($"Response status code was: {response.StatusCode}");
				return string.Empty;
			}
			catch (Exception e)
			{
				LogError(e.Message);
				return string.Empty;
			}
		}

		private void LogError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}
	}
}
