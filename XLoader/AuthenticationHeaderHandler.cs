using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Text.Json;
using XLoader.Contracts;

namespace XLoader
{
	public class AuthenticationHeaderHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			if (request.Headers.Authorization?.Scheme != "Bearer")
			{

				if (!string.IsNullOrWhiteSpace(AccessToken.Value))
				{
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken.Value);
				}
			}
			try
			{
				return await base.SendAsync(request, cancellationToken);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}