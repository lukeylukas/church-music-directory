namespace ChurchMusicDirectory
{
    partial class FormLogin
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
            this.buttonLogin = new System.Windows.Forms.Button();
            this.richTextBoxUsername = new System.Windows.Forms.RichTextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.richTextBoxPassword = new System.Windows.Forms.RichTextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.checkBoxRememberLogin = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(196, 89);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonLogin.TabIndex = 2;
            this.buttonLogin.Text = "Connect";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // richTextBoxUsername
            // 
            this.richTextBoxUsername.Location = new System.Drawing.Point(102, 27);
            this.richTextBoxUsername.Multiline = false;
            this.richTextBoxUsername.Name = "richTextBoxUsername";
            this.richTextBoxUsername.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBoxUsername.Size = new System.Drawing.Size(169, 25);
            this.richTextBoxUsername.TabIndex = 0;
            this.richTextBoxUsername.Text = "";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(36, 30);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(60, 15);
            this.labelUsername.TabIndex = 17;
            this.labelUsername.Text = "Username";
            // 
            // richTextBoxPassword
            // 
            this.richTextBoxPassword.Location = new System.Drawing.Point(102, 58);
            this.richTextBoxPassword.Multiline = false;
            this.richTextBoxPassword.Name = "richTextBoxPassword";
            this.richTextBoxPassword.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBoxPassword.Size = new System.Drawing.Size(169, 25);
            this.richTextBoxPassword.TabIndex = 1;
            this.richTextBoxPassword.Text = "";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(36, 61);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 15);
            this.labelPassword.TabIndex = 18;
            this.labelPassword.Text = "Password";
            // 
            // checkBoxRememberLogin
            // 
            this.checkBoxRememberLogin.AutoSize = true;
            this.checkBoxRememberLogin.Location = new System.Drawing.Point(36, 89);
            this.checkBoxRememberLogin.Name = "checkBoxRememberLogin";
            this.checkBoxRememberLogin.Size = new System.Drawing.Size(117, 19);
            this.checkBoxRememberLogin.TabIndex = 3;
            this.checkBoxRememberLogin.Text = "Remember Login";
            this.checkBoxRememberLogin.UseVisualStyleBackColor = true;
            this.checkBoxRememberLogin.Click += new System.EventHandler(this.checkBoxRememberLogin_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 178);
            this.Controls.Add(this.checkBoxRememberLogin);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.richTextBoxPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.richTextBoxUsername);
            this.Controls.Add(this.buttonLogin);
            this.Name = "FormLogin";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button buttonLogin;
        private RichTextBox richTextBoxUsername;
        private Label labelUsername;
        private RichTextBox richTextBoxPassword;
        private Label labelPassword;
        private CheckBox checkBoxRememberLogin;
    }
}