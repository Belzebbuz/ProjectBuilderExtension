using CommandLine;

namespace XLoader
{
	internal class Options
	{
		[Option('o', "output", Required = true, HelpText = "Output directory")]
		public string OutputDirectory { get; set; }

		[Option('p', "project", Required = true, HelpText = "Name of project")]
		public string ProjectName { get; set; }

		[Option('s', "server", Required = false, HelpText = "Default server is http://192.168.133.73:3300")]
		public string ServerUrl { get; set; } = "http://192.168.133.73:3300";

		[Option('t', "test", Required = false, HelpText = "From tests folder")]
		public bool IsTest { get; set; } = false;
	}
}