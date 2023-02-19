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
		[HttpGet("search/{name}")]
		public IEnumerable<string> SearchFiles(string name)
		{
			var files = Directory.GetFiles(_settings.ReleasesPath)
				.Select(Path.GetFileName);
			return files.Where(file => file.Contains(name, StringComparison.CurrentCultureIgnoreCase));
		}

		[HttpGet("download/{name}")]
		public async Task<IActionResult> DownloadFile(string name)
		{
			var filePath = Path.Combine(_settings.ReleasesPath, name);
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
