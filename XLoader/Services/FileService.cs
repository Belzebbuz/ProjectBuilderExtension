using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using XLoader.Contracts;

namespace XLoader.Services
{
	internal class FileService : IFileService
	{
		private readonly Options _options;
		private readonly HttpClient _client;

		public FileService(IHttpClientFactory httpClientFactory, Options options)
		{
			_options = options;
			_client = httpClientFactory.CreateClient("Server");
		}
		public async Task<IDictionary<int,string>> GetFiles(string name)
		{
			try
			{
				var response = await _client.GetAsync($"files/search/{name}");
				var result = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					var files = JsonSerializer.Deserialize<IEnumerable<string>>(result);
					return files.Select((str, index) => new { Index = ++index, Value = str })
						.ToDictionary(x => x.Index, x => x.Value);
				}
				else
				{
					Console.WriteLine($"Код ответа: {response.StatusCode}");
					return new Dictionary<int, string>();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return new Dictionary<int, string>();
			}
		}

		public async Task<string> DownloadAsync(string name)
		{
			if(!Directory.Exists(_options.OutputDirectory))
				Directory.CreateDirectory(_options.OutputDirectory);

			var fullFilePath = Path.Combine(_options.OutputDirectory, name);

			var response = await _client.GetAsync($"files/download/{name}", HttpCompletionOption.ResponseHeadersRead);
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"Status code: {response.StatusCode}");
				return "Error";
			}
			var contentLength = response.Content.Headers.ContentLength ?? -1L;
			var buffer = new byte[81920];
			var bytesDownloaded = 0L;
			await using var stream = await response.Content.ReadAsStreamAsync();
			await using var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write);
			var progress = new Progress<long>(value =>
			{
				bytesDownloaded = value;
				Console.Write($"\rСкачано {bytesDownloaded} байт из {contentLength} ({(double)bytesDownloaded / contentLength:P2})");
			});

			while (true)
			{
				var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
				if (bytesRead == 0)
				{
					break;
				}

				await fileStream.WriteAsync(buffer, 0, bytesRead);
				bytesDownloaded += bytesRead;
				((IProgress<long>)progress).Report(bytesDownloaded);
			}
			return fullFilePath;
		}
	}
}
