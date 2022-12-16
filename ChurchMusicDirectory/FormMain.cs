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
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Setup();
        }
        private void Setup()
        {
            FormLogin loginForm = new FormLogin();
            loginForm.TopLevel = false;
            loginForm.AutoScroll = true;
            loginForm.FormBorderStyle = FormBorderStyle.None;
            loginForm.Disposed += LoginForm_Disposed;
            panelMain.Controls.Add(loginForm);
            loginForm.Show();
        }

        private void LoginForm_Disposed(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();

            FormSongTables songTableForm = new FormSongTables();
            songTableForm.TopLevel = false;
            songTableForm.AutoScroll = true;
            songTableForm.FormBorderStyle = FormBorderStyle.None;
            songTableForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(songTableForm);

            FormSongTables.getSongInfo(songTableForm, Properties.Settings.Default.Username, Properties.Settings.Default.Password);

            songTableForm.Show();
        }
    }
}
