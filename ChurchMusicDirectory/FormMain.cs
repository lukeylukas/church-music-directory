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
        private FormLogin loginForm;
        private FormSongTables songTableForm;
        private FormServicePlanner servicePlannerForm;
        public FormMain()
        {
            InitializeComponent();
            Setup();
        }
        private void Setup()
        {
            SetupLoginForm();
            SetupSongTableForm();
            SetupServicePlannerForm();
        }
        private void SetupLoginForm()
        {
            loginForm = new FormLogin();
            loginForm.TopLevel = false;
            loginForm.AutoScroll = true;
            loginForm.Location = new System.Drawing.Point(100,100);
            loginForm.FormBorderStyle = FormBorderStyle.None;
            loginForm.Disposed += LoginForm_Disposed;
            panelMain.Controls.Add(loginForm);
            loginForm.Show();
        }
        private void SetupSongTableForm()
        {
            songTableForm = new FormSongTables(this);
            songTableForm.TopLevel = false;
            songTableForm.AutoScroll = true;
            songTableForm.FormBorderStyle = FormBorderStyle.None;
            songTableForm.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            songTableForm.Size = panelMain.Size;
            panelMain.Controls.Add(songTableForm);
        }
        private void SetupServicePlannerForm()
        {
            servicePlannerForm = new FormServicePlanner();
            servicePlannerForm.TopLevel = false;
            servicePlannerForm.AutoScroll = true;
            servicePlannerForm.FormBorderStyle = FormBorderStyle.None;
            servicePlannerForm.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            servicePlannerForm.Location = new System.Drawing.Point(panelMain.Width - servicePlannerForm.Width, 0);
            panelMain.Controls.Add(servicePlannerForm);
        }

        private void LoginForm_Disposed(object? sender, EventArgs e)
        {

            FormSongTables.getSongInfo(songTableForm, Properties.Settings.Default.Username, Properties.Settings.Default.Password);

            songTableForm.Show();
        }

        public void ToggleServicePlanner()
        {
            if (servicePlannerForm.Visible)
            {
                servicePlannerForm.Hide();
                songTableForm.Width = panelMain.Width;
            }
            else
            {
                servicePlannerForm.Show();
                songTableForm.Width = panelMain.Width - servicePlannerForm.Width;
            }
        }
    }
}
