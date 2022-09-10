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
			this.serverPathLabel = new System.Windows.Forms.Label();
			this.projectNameLabel = new System.Windows.Forms.Label();
			this.sendButton = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.serverNameTextBox = new System.Windows.Forms.TextBox();
			this.projectNameTextBox = new System.Windows.Forms.TextBox();
			this.serverPortTextBox = new System.Windows.Forms.TextBox();
			this.portLabel = new System.Windows.Forms.Label();
			this.errorLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// serverPathLabel
			// 
			this.serverPathLabel.AutoSize = true;
			this.serverPathLabel.Location = new System.Drawing.Point(12, 27);
			this.serverPathLabel.Name = "serverPathLabel";
			this.serverPathLabel.Size = new System.Drawing.Size(44, 13);
			this.serverPathLabel.TabIndex = 0;
			this.serverPathLabel.Text = "Сервер";
			// 
			// projectNameLabel
			// 
			this.projectNameLabel.AutoSize = true;
			this.projectNameLabel.Location = new System.Drawing.Point(12, 54);
			this.projectNameLabel.Name = "projectNameLabel";
			this.projectNameLabel.Size = new System.Drawing.Size(101, 13);
			this.projectNameLabel.TabIndex = 1;
			this.projectNameLabel.Text = "Название проекта";
			// 
			// sendButton
			// 
			this.sendButton.Location = new System.Drawing.Point(179, 147);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(136, 23);
			this.sendButton.TabIndex = 2;
			this.sendButton.Text = "Собрать и отправить";
			this.sendButton.UseVisualStyleBackColor = true;
			this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(15, 108);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(459, 33);
			this.progressBar.TabIndex = 3;
			// 
			// serverNameTextBox
			// 
			this.serverNameTextBox.Location = new System.Drawing.Point(119, 25);
			this.serverNameTextBox.Name = "serverNameTextBox";
			this.serverNameTextBox.Size = new System.Drawing.Size(121, 20);
			this.serverNameTextBox.TabIndex = 4;
			this.serverNameTextBox.Text = "localhost";
			this.serverNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// projectNameTextBox
			// 
			this.projectNameTextBox.Location = new System.Drawing.Point(119, 50);
			this.projectNameTextBox.Name = "projectNameTextBox";
			this.projectNameTextBox.Size = new System.Drawing.Size(196, 20);
			this.projectNameTextBox.TabIndex = 5;
			// 
			// serverPortTextBox
			// 
			this.serverPortTextBox.Location = new System.Drawing.Point(262, 25);
			this.serverPortTextBox.Name = "serverPortTextBox";
			this.serverPortTextBox.Size = new System.Drawing.Size(53, 20);
			this.serverPortTextBox.TabIndex = 6;
			this.serverPortTextBox.Text = "2300";
			this.serverPortTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// portLabel
			// 
			this.portLabel.AutoSize = true;
			this.portLabel.Location = new System.Drawing.Point(246, 28);
			this.portLabel.Name = "portLabel";
			this.portLabel.Size = new System.Drawing.Size(10, 13);
			this.portLabel.TabIndex = 7;
			this.portLabel.Text = ":";
			// 
			// errorLabel
			// 
			this.errorLabel.AutoSize = true;
			this.errorLabel.Location = new System.Drawing.Point(12, 83);
			this.errorLabel.MaximumSize = new System.Drawing.Size(450, 0);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(0, 13);
			this.errorLabel.TabIndex = 8;
			// 
			// SendingSettingWindow
			// 
			this.AcceptButton = this.sendButton;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(486, 182);
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
			this.Name = "SendingSettingWindow";
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
	}
}