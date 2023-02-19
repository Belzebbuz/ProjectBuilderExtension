using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
				Console.WriteLine("Что-то пошло не так");
				return;
			}

			var fileService = DI.Services.GetRequiredService<IFileService>();
			var files = fileService.GetFiles(options.ProjectName).Result;
			if (!files!.Any())
			{
				Console.WriteLine("Файлы не найдены, попробуйте запустить программу с другим параметром поиска.");
				return;
			}
			DownloadFile(files, fileService);
		}

		private static void DownloadFile(IDictionary<int, string> files, IFileService fileService)
		{
			foreach (var file in files)
			{
				Console.WriteLine($"{file.Key} : {file.Value}");
			}
			Console.Write("\nКакой файл скачать? Укажите номер: ");
			if (int.TryParse(Console.ReadLine(), out var index))
			{
				if (files.TryGetValue(index, out var fileName))
				{
					var result = fileService.DownloadAsync(fileName).Result;
					if (result == "Error") return;

					Console.WriteLine($"\nФайл скачан: {result}\nОткрыть расположение? [Д/Н]");
					var openAnswer = Console.ReadKey();
					if(openAnswer.KeyChar == 'Д' || openAnswer.KeyChar == 'Y' || openAnswer.KeyChar == 'д' || openAnswer.KeyChar == 'y')
						Process.Start("explorer.exe", Path.GetDirectoryName(result));
				}
				else
				{
					LogError("\nТакого номера нет!");
					DownloadFile(files, fileService);
				}
			}
			else
			{
				LogError("\nВвидите число");
				DownloadFile(files, fileService);
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
//for (int progress = 0; progress <= 100; progress++)
//{
//	Console.Write("Загрузка ");
//	Console.Write(new string('=', progress / 2));
//	Console.Write(new string(' ', 50 - progress / 2));
//	Console.Write($" {progress}%\r");
//	Thread.Sleep(100);
//}