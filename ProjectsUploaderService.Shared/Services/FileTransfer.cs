﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsUploaderService.Shared.Services
{
	public class FileTransfer : IDisposable
	{
		public FileTransfer(string name)
		{
			Name = name;
			Stream = File.Open(name, FileMode.OpenOrCreate);
		}

		public string Name { get; set; }

		public Stream Stream { get; private set; }

		public void Dispose()
		{
			Stream?.Flush();
			Stream?.Dispose();

		}
	}
}
