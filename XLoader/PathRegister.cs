using System;
using System.IO;
using System.Linq;
using System.Security;
using Microsoft.Win32;

namespace XLoader
{
	public static class PathRegister
	{
		public static void RegisterInPath()
		{
			try
			{
				string appName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
				string appPath = AppDomain.CurrentDomain.BaseDirectory;
				var	pathEntries = Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator);
				bool isRegistered = pathEntries.Contains(appPath.Remove(appPath.Length-1)) || pathEntries.Contains(appPath);
				if (!isRegistered)
				{
					var newPath = appPath + Path.PathSeparator + Environment.GetEnvironmentVariable("PATH");
					Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Machine);

					using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + appName))
					{
						key.SetValue("", Path.Combine(appPath, appName + ".exe"));
					}

					Console.WriteLine($"Программа успешно зарегистрирована в PATH.");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.ReadKey();
			}
		}
	}
}