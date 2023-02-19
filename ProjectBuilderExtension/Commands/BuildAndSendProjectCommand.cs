namespace ProjectBuilderExtension;

[Command(PackageIds.BuildAndSendProjectCommand)]
internal sealed class BuildAndSendProjectCommand : BaseCommand<BuildAndSendProjectCommand>
{

	protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
	{
		var project = await VS.Solutions.GetActiveItemAsync();
		if (project == null)
			return;
		var window = new SendingSettingWindow(new()
			{ ProjectFullPath = project.FullPath, ProjectName = project.Name });
		window.Show();
	}
}