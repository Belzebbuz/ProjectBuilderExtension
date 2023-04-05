using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ProjectsUploaderService.Shared.Settings;

namespace ProjectsUploaderService3._1.Controllers
{
	[Route("files")]
	[Authorize]
	[ApiController]
	public class FilesController : ControllerBase
	{
		private readonly UploadSettings _settings;

		public FilesController(UploadSettings settings)
		{
			_settings = settings;
		}
		[HttpGet("search/{name}/{isTest}")]
		public IEnumerable<string> SearchFiles(string name, bool isTest)
		{
			var folderPath = isTest ? _settings.TestPath : _settings.ReleasesPath;
			var files = Directory.GetFiles(folderPath)
				.Select(Path.GetFileName);
			return files.Where(file => file.Contains(name, StringComparison.CurrentCultureIgnoreCase));
		}

		[HttpGet("download/{name}/{isTest}")]
		public async Task<IActionResult> DownloadFileAsync(string name, bool isTest)
		{
			var folderPath = isTest ? _settings.TestPath : _settings.ReleasesPath;
			var filePath = Path.Combine(folderPath, name);
			if (!System.IO.File.Exists(filePath))
			{
				return NotFound();
			}

			var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

			var contentType = "application/octet-stream";

			Response.Headers.ContentLength = fileStream.Length;

			return File(fileStream, contentType);
		}


	}

}
