using CommandLine;

namespace XLoader
{
	internal class Options
	{
		[Option('o', "output", Required = true, HelpText = "Output directory of zip")]
		public string OutputDirectory { get; set; }

		[Option('p', "project", Required = true, HelpText = "Name of project")]
		public string ProjectName { get; set; }

		[Option('s', "server", Required = false, HelpText = "default server is 127.0.0.1:3300")]
		public string ServerUrl { get; set; } = "http://127.0.0.1:3300";
	}
}