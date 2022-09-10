using CliWrap;
using CliWrap.Buffered;
using ICSharpCode.SharpZipLib.Zip;
using ProjectBuilderExtension.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ProjectBuilderExtension
{
	[Command(PackageIds.BuildAndSendProjectCommand)]
	internal sealed class BuildAndSendProjectCommand : BaseCommand<BuildAndSendProjectCommand>
	{

		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			var project = await VS.Solutions.GetActiveItemAsync();
			SendingWindowFactory.Build(new SendingSettingWindowOptions
			{
				ProjectFullPath = project.FullPath,
				ProjectName = project.Name,
			}).Show();
		}
	}
}
