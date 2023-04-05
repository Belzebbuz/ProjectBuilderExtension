using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using XLoader.Contracts;
using XLoader.Services;

namespace XLoader
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			PathRegister.RegisterInPath();
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(RunOptions)
				.WithNotParsed(HandleParseErrors);
		}

		private static void RunOptions(Options options)
		{
			DI.Init(options);
			AccessToken.Value = DI.Services.GetRequiredService<IAuthorizeService>().GetTokenAsync(WaitPasswordFromUser()).Result;
			if (string.IsNullOrEmpty(AccessToken.Value))
			{
				LogError("Что-то пошло не так");
				return;
			}

			var fileService = DI.Services.GetRequiredService<IFileService>();
			var files = fileService.GetFilesAsync(options.ProjectName, options.IsTest).Result;
			if (!files!.Any())
			{
				LogError("Файлы не найдены, попробуйте запустить программу с другим параметром поиска.");
				return;
			}
			DownloadFilesAsync(files, fileService, options.IsTest).Wait();
			Console.WriteLine($"\nФайлы скачаны в: {options.OutputDirectory}\nОткрыть расположение? [Д/Н][y/n]");
			var openAnswer = Console.ReadKey();
			if (new[] { 'Д', 'Y', 'y', 'д' }.Contains(openAnswer.KeyChar))
				Process.Start("explorer.exe", options.OutputDirectory);
		}

		private static async Task DownloadFilesAsync(IDictionary<int, string> files, IFileService fileService, bool isTest)
		{
			foreach (var file in files)
			{
				Console.WriteLine($"{file.Key} : {file.Value}");
			}
			Console.Write("\nУкажите номера через пробел: ");
			var answer = Console.ReadLine();
			foreach (var indexString in answer.Split(' '))
			{
				if (int.TryParse(indexString, out var index))
				{
					if (files.TryGetValue(index, out var fileName))
					{
						await fileService.DownloadAsync(fileName, isTest);
						Console.WriteLine();
					}
					else
					{
						LogError($"\nТакого номера нет - {indexString}!");
					}
				}
				else
				{
					LogError($"\nВвидите число - {indexString}");
				}
			}
		}

		private static void LogError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		private static string WaitPasswordFromUser()
		{
			Console.Write("Введите пароль: ");
			string password = "";
			while (true)
			{
				var key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.Enter)
				{
					break;
				}

				password += key.KeyChar;
			}
			Console.WriteLine();
			return password;
		}

		private static void HandleParseErrors(IEnumerable<Error> errors)
		{
			foreach (var error in errors)
			{
				Console.WriteLine(error.ToString());
			}
		}
	}
}