using BeetleX.Buffers;
using BeetleX;
using BeetleX.Clients;
using CliWrap;
using CliWrap.Buffered;
using ICSharpCode.SharpZipLib.Zip;
using ProjectBuilderExtension.Infrastructure;
using ProjectUpdater.Tcp.Messages;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProjectBuilderExtension
{
	public partial class SendingSettingWindow : Form
	{
		private readonly string _tempPath;
		private string _buildOutputPath = String.Empty;
		private string _zipFilePath = String.Empty;
		private const string _powerShell = "powershell";
		private const string _buildCommand = "dotnet build -c Release";
		private readonly SendingSettingWindowOptions _options;
		private TcpClientFileSender _tcpClient;
		public SendingSettingWindow(SendingSettingWindowOptions options)
		{
			InitializeComponent();
			_options = options;
			_tempPath = Path.Combine(Path.GetDirectoryName(_options.ProjectFullPath), @"bin\Temp");

			projectNameTextBox.Text = options.ProjectName;
			buildCommandTextBox.Text = _buildCommand;
		}

		protected override void OnClosed(EventArgs e)
		{
			if (_tcpClient != null)
				_tcpClient.Disconnect();

			base.OnClosed(e);
		}

		private void DeleteResources()
		{
			if (Directory.Exists(_tempPath))
			{
				Directory.Delete(_tempPath, true);
			}

			if (File.Exists(_zipFilePath))
			{
				File.Delete(_zipFilePath);
			}
		}

		private async Task<(bool Succeded, string BuildLog)> BuildProjectAsync(string buildPath, string sourcePath)
		{
			var cmdResult = await Cli.Wrap(_powerShell)
					.WithArguments(new[] { buildCommandTextBox.Text, $"\"{sourcePath}\"", "-o", $"\"{buildPath}\"" })
					.WithValidation(CommandResultValidation.None)
					.ExecuteBufferedAsync();
			return (cmdResult.StandardOutput.Contains("Build succeeded") || cmdResult.StandardOutput.Contains("Сборка успешно завершена") || cmdResult.ExitCode == 0,
					cmdResult.StandardOutput);
		}

		private void ZipFiles(string zipFileName, string sourceName)
		{
			new FastZip().CreateZip(zipFileName, sourceName, true, fileFilter: null);
		}

		private async void sendButton_Click(object sender, EventArgs e)
		{
			try
			{
				this.Text = "Проверка доступности сервера";

				SetEnableUIElements(false);
				_tcpClient = ConfigureClient();
				if (!await _tcpClient.IsConnectAsync())
					throw new Exception("Сервер недоступен!");

				this.Text = "Сборка проекта";

				_buildOutputPath = Path.Combine(_tempPath, _options.ProjectName);
				var buildResult = await BuildProjectAsync(_buildOutputPath, _options.ProjectFullPath);
				if (!buildResult.Succeded)
					throw new Exception($"Произошла ошибка при сборке\n{buildResult.BuildLog}");

				var assemblyVersion = GetFileVersion(_buildOutputPath, _options.ProjectName);

				this.Text = "Архивация сборки";

				var finalProjectName = string.IsNullOrEmpty(projectNameTextBox.Text) ? _options.ProjectName : projectNameTextBox.Text;
				finalProjectName = string.IsNullOrEmpty(assemblyVersion) ? finalProjectName : $"{finalProjectName} - v{assemblyVersion}";
				_zipFilePath = Path.Combine(_tempPath, $"{finalProjectName}.zip");
				ZipFiles(_zipFilePath, _buildOutputPath);

				this.Text = "Отправка на сервер";

				progressBar.Maximum = _tcpClient.StartSendFile(_zipFilePath);
			}
			catch (Exception ex)
			{
				DeleteResources();
				SetEnableUIElements(true);
				await VS.MessageBox.ShowErrorAsync(ex.Message);
			}
		}

		private void SetEnableUIElements(bool isEnable)
		{
			serverNameTextBox.Enabled = isEnable;
			serverPortTextBox.Enabled = isEnable;
			projectNameTextBox.Enabled = isEnable;
			buildCommandTextBox.Enabled = isEnable;
			sendButton.Enabled = isEnable;
		}

		private TcpClientFileSender ConfigureClient()
		{
			var client = new TcpClientFileSender(serverNameTextBox.Text, serverPortTextBox.Text);
			client.OnClientError += (o, err) =>
			{
				this.BeginInvoke(new Action(() =>
				{
					errorLabel.ForeColor = Color.Red;
					errorLabel.Text = err.Error.Message;
					progressBar.Value = 0;
					SetEnableUIElements(true);
					DeleteResources();
				}));
			};
			client.OnBlockSended += (o, args) =>
			{
				this.BeginInvoke(new Action(() =>
				{
					errorLabel.ForeColor = Color.Green;
					errorLabel.Text = "Загрузка...";
					progressBar.Value = args.BlockIndex;
				}));
			};
			client.OnSendingComplite += (o, args) =>
			{
				this.BeginInvoke(new Action(() =>
				{
					DeleteResources();
					errorLabel.Text = $"Сборка загружена！[{args.CompliteTime}]";
					this.Text = "Сборка загружена на сервер";
					progressBar.Value = args.BlockIndex;
				}));
			};
			return client;
		}

		public string GetFileVersion(string folderPath, string exeFileName)
		{
			var exePath = Path.Combine(folderPath, $"{exeFileName}.exe");
			if (File.Exists(exePath))
				return FileVersionInfo.GetVersionInfo(exePath).FileVersion;
			return String.Empty;
		}
	}
}
