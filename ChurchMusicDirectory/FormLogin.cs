using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChurchMusicDirectory
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            Setup();
        }
        private void Setup()
        {
            richTextBoxUsername.Text = Properties.Settings.Default.Username;
            richTextBoxPassword.Text = Properties.Settings.Default.Password;
            checkBoxRememberLogin.Checked = Properties.Settings.Default.RememberLogin;
            checkBoxRememberLogin.CheckState = (Properties.Settings.Default.RememberLogin? CheckState.Checked: CheckState.Unchecked);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (checkBoxRememberLogin.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.Username = richTextBoxUsername.Text;
                Properties.Settings.Default.Password = richTextBoxPassword.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Reset();
            }
            this.Hide();
            FormMain formMain = new FormMain();
            FormMain.getSongInfo(formMain, richTextBoxUsername.Text, richTextBoxPassword.Text);
            formMain.Show();
        }

        private void checkBoxRememberLogin_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter: CheckBoxChangeCheck(); break;
                default: break;
            }
        }
        private void CheckBoxChangeCheck()
        {
            if (checkBoxRememberLogin.Checked)
            {
                checkBoxRememberLogin.Checked = false;
                checkBoxRememberLogin.CheckState = CheckState.Unchecked;
            }
            else
            {
                checkBoxRememberLogin.Checked = true;
                checkBoxRememberLogin.CheckState = CheckState.Checked;
            }
            Properties.Settings.Default.RememberLogin = (checkBoxRememberLogin.CheckState == CheckState.Checked);
        }
    }
}
