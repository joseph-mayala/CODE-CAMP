namespace WinFormsApp
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblDemo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		private void InitializeComponent()
		{
			txtUsername = new TextBox();
			txtPassword = new TextBox();
			btnLogin = new Button();
			lblTitle = new Label();
			lblUsername = new Label();
			lblPassword = new Label();
			lblDemo = new Label();
			SuspendLayout();
			// 
			// txtUsername
			// 
			txtUsername.Location = new Point(62, 156);
			txtUsername.Margin = new Padding(4, 4, 4, 4);
			txtUsername.Name = "txtUsername";
			txtUsername.PlaceholderText = "Enter username";
			txtUsername.Size = new Size(374, 31);
			txtUsername.TabIndex = 4;
			// 
			// txtPassword
			// 
			txtPassword.Location = new Point(62, 238);
			txtPassword.Margin = new Padding(4, 4, 4, 4);
			txtPassword.Name = "txtPassword";
			txtPassword.PasswordChar = '*';
			txtPassword.PlaceholderText = "Enter password";
			txtPassword.Size = new Size(374, 31);
			txtPassword.TabIndex = 2;
			// 
			// btnLogin
			// 
			btnLogin.BackColor = Color.FromArgb(0, 120, 215);
			btnLogin.FlatStyle = FlatStyle.Flat;
			btnLogin.ForeColor = Color.White;
			btnLogin.Location = new Point(156, 300);
			btnLogin.Margin = new Padding(4, 4, 4, 4);
			btnLogin.Name = "btnLogin";
			btnLogin.Size = new Size(188, 44);
			btnLogin.TabIndex = 1;
			btnLogin.Text = "Login";
			btnLogin.UseVisualStyleBackColor = false;
			btnLogin.Click += btnLogin_Click;
			// 
			// lblTitle
			// 
			lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
			lblTitle.Location = new Point(62, 38);
			lblTitle.Margin = new Padding(4, 0, 4, 0);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new Size(375, 50);
			lblTitle.TabIndex = 6;
			lblTitle.Text = "APU CodeCamp Management";
			lblTitle.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lblUsername
			// 
			lblUsername.AutoSize = true;
			lblUsername.Location = new Point(62, 125);
			lblUsername.Margin = new Padding(4, 0, 4, 0);
			lblUsername.Name = "lblUsername";
			lblUsername.Size = new Size(95, 25);
			lblUsername.TabIndex = 5;
			lblUsername.Text = "Username:";
			// 
			// lblPassword
			// 
			lblPassword.AutoSize = true;
			lblPassword.Location = new Point(62, 206);
			lblPassword.Margin = new Padding(4, 0, 4, 0);
			lblPassword.Name = "lblPassword";
			lblPassword.Size = new Size(91, 25);
			lblPassword.TabIndex = 3;
			lblPassword.Text = "Password:";
			// 
			// lblDemo
			// 
			lblDemo.AutoSize = true;
			lblDemo.Font = new Font("Segoe UI", 8F);
			lblDemo.ForeColor = Color.Gray;
			lblDemo.Location = new Point(62, 362);
			lblDemo.Margin = new Padding(4, 0, 4, 0);
			lblDemo.Name = "lblDemo";
			lblDemo.Size = new Size(614, 21);
			lblDemo.TabIndex = 0;
			lblDemo.Text = "Demo: admin, trainer1, trainer2, lecturer1, student1, student2 (password: password123)";
			lblDemo.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// LoginForm
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(690, 438);
			Controls.Add(lblDemo);
			Controls.Add(btnLogin);
			Controls.Add(txtPassword);
			Controls.Add(lblPassword);
			Controls.Add(txtUsername);
			Controls.Add(lblUsername);
			Controls.Add(lblTitle);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Margin = new Padding(4, 4, 4, 4);
			MaximizeBox = false;
			Name = "LoginForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Login - APU CodeCamp";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}