namespace ProjectBuilderExtension
{
	partial class SendingSettingWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendingSettingWindow));
			this.serverPathLabel = new System.Windows.Forms.Label();
			this.projectNameLabel = new System.Windows.Forms.Label();
			this.sendButton = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.serverNameTextBox = new System.Windows.Forms.TextBox();
			this.projectNameTextBox = new System.Windows.Forms.TextBox();
			this.serverPortTextBox = new System.Windows.Forms.TextBox();
			this.portLabel = new System.Windows.Forms.Label();
			this.errorLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.buildCommandTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// serverPathLabel
			// 
			this.serverPathLabel.AutoSize = true;
			this.serverPathLabel.Location = new System.Drawing.Point(69, 24);
			this.serverPathLabel.Name = "serverPathLabel";
			this.serverPathLabel.Size = new System.Drawing.Size(44, 13);
			this.serverPathLabel.TabIndex = 0;
			this.serverPathLabel.Text = "Сервер";
			// 
			// projectNameLabel
			// 
			this.projectNameLabel.AutoSize = true;
			this.projectNameLabel.Location = new System.Drawing.Point(12, 49);
			this.projectNameLabel.Name = "projectNameLabel";
			this.projectNameLabel.Size = new System.Drawing.Size(101, 13);
			this.projectNameLabel.TabIndex = 1;
			this.projectNameLabel.Text = "Название проекта";
			// 
			// sendButton
			// 
			this.sendButton.Location = new System.Drawing.Point(147, 183);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(136, 23);
			this.sendButton.TabIndex = 2;
			this.sendButton.Text = "Собрать и отправить";
			this.sendButton.UseVisualStyleBackColor = true;
			this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(12, 144);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(396, 33);
			this.progressBar.TabIndex = 3;
			// 
			// serverNameTextBox
			// 
			this.serverNameTextBox.Location = new System.Drawing.Point(119, 21);
			this.serverNameTextBox.Name = "serverNameTextBox";
			this.serverNameTextBox.Size = new System.Drawing.Size(121, 20);
			this.serverNameTextBox.TabIndex = 4;
			this.serverNameTextBox.Text = "192.168.133.73";
			this.serverNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// projectNameTextBox
			// 
			this.projectNameTextBox.Location = new System.Drawing.Point(119, 46);
			this.projectNameTextBox.Name = "projectNameTextBox";
			this.projectNameTextBox.Size = new System.Drawing.Size(289, 20);
			this.projectNameTextBox.TabIndex = 5;
			this.projectNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// serverPortTextBox
			// 
			this.serverPortTextBox.Location = new System.Drawing.Point(262, 21);
			this.serverPortTextBox.Name = "serverPortTextBox";
			this.serverPortTextBox.Size = new System.Drawing.Size(53, 20);
			this.serverPortTextBox.TabIndex = 6;
			this.serverPortTextBox.Text = "2300";
			this.serverPortTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// portLabel
			// 
			this.portLabel.AutoSize = true;
			this.portLabel.Location = new System.Drawing.Point(246, 24);
			this.portLabel.Name = "portLabel";
			this.portLabel.Size = new System.Drawing.Size(10, 13);
			this.portLabel.TabIndex = 7;
			this.portLabel.Text = ":";
			// 
			// errorLabel
			// 
			this.errorLabel.AutoSize = true;
			this.errorLabel.Location = new System.Drawing.Point(12, 112);
			this.errorLabel.MaximumSize = new System.Drawing.Size(450, 0);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(0, 13);
			this.errorLabel.TabIndex = 8;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "Команда сборки";
			// 
			// buildCommandTextBox
			// 
			this.buildCommandTextBox.Location = new System.Drawing.Point(119, 73);
			this.buildCommandTextBox.Name = "buildCommandTextBox";
			this.buildCommandTextBox.Size = new System.Drawing.Size(289, 20);
			this.buildCommandTextBox.TabIndex = 10;
			this.buildCommandTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// SendingSettingWindow
			// 
			this.AcceptButton = this.sendButton;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.ClientSize = new System.Drawing.Size(420, 218);
			this.Controls.Add(this.buildCommandTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.errorLabel);
			this.Controls.Add(this.portLabel);
			this.Controls.Add(this.serverPortTextBox);
			this.Controls.Add(this.projectNameTextBox);
			this.Controls.Add(this.serverNameTextBox);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.sendButton);
			this.Controls.Add(this.projectNameLabel);
			this.Controls.Add(this.serverPathLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SendingSettingWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Загрузка проекта на сервер";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label serverPathLabel;
		private System.Windows.Forms.Label projectNameLabel;
		private System.Windows.Forms.Button sendButton;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.TextBox serverNameTextBox;
		private System.Windows.Forms.TextBox projectNameTextBox;
		private System.Windows.Forms.TextBox serverPortTextBox;
		private System.Windows.Forms.Label portLabel;
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox buildCommandTextBox;
	}
}