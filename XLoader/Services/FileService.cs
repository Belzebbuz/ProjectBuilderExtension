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
		private object _locker = new object();
		public FileService(IHttpClientFactory httpClientFactory, Options options)
		{
			_options = options;
			_client = httpClientFactory.CreateClient("Server");
		}
		public async Task<IDictionary<int, string>> GetFilesAsync(string name, bool isTest)
		{
			try
			{
				var response = await _client.GetAsync($"files/search/{name}/{isTest}");
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

		public async Task<string> DownloadAsync(string name, bool isTest)
		{
			CreateFolder();
			var fullFilePath = Path.Combine(_options.OutputDirectory, name);
			var response = await _client.GetAsync($"files/download/{name}/{isTest}", HttpCompletionOption.ResponseHeadersRead);
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine($"Status code: {response.StatusCode}");
				return "Error";
			}
			var contentLength = response.Content.Headers.ContentLength ?? -1L;
			await using var stream = await response.Content.ReadAsStreamAsync();
			await using var fileStream = File.OpenWrite(fullFilePath);
			IProgress<long> progress = new Progress<long>(value => HandleProgressReport(name, value, contentLength));
			var buffer = new byte[81920];
			var bytesDownloaded = 0L;
			while (true)
			{
				var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
				if (bytesRead == 0)
					break;

				await fileStream.WriteAsync(buffer, 0, bytesRead);
				bytesDownloaded += bytesRead;
				progress.Report(bytesDownloaded);
			}
			return fullFilePath;
		}

		private static void HandleProgressReport(string name, long value, long contentLength)
		{
			var progress = (double)value / contentLength;
			if (progress >= 0.95)
				progress = 1;
			Console.Write($"\r{name} - Скачано ({progress:P2})");
		}

		private void CreateFolder()
		{
			if (!Directory.Exists(_options.OutputDirectory))
				Directory.CreateDirectory(_options.OutputDirectory);
		}
	}
}
