using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ChurchMusicDirectory
{
    public partial class FormMain : Form
    {
        private FormLogin loginForm;
        private FormSongTables songTableForm;
        private FormServicePlanner servicePlannerForm;
        private DataCtrl dataCtrl;

        //Instance of FormMain for getInstance()
        private static FormMain self;
        //used to get the current instance of the form
        public static FormMain getInstance()
        {
            return self;
        }
        public FormMain()
        {
            //Set FormMain instance to be this instance
            self = this;
            InitializeComponent();
            Setup();
        }
        private void Setup()
        {
            SetupLoginForm();
            SetupSongTableForm();
            SetupServicePlannerForm();
            dataCtrl = new DataCtrl();
            loginForm.Show();
        }
        private void SetupLoginForm()
        {
            loginForm = new FormLogin();
            loginForm.TopLevel = false;
            loginForm.AutoScroll = true;
            loginForm.Location = new System.Drawing.Point(100,100);
            loginForm.FormBorderStyle = FormBorderStyle.None;
            panelMain.Controls.Add(loginForm);
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
            servicePlannerForm.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            servicePlannerForm.Location = new System.Drawing.Point(panelMain.Width - servicePlannerForm.Width, 0);
            panelMain.Controls.Add(servicePlannerForm);
        }

        public static void LoginToApplication()
        {
            FormMain.getInstance().Invoke((MethodInvoker)delegate
            {
                FormMain.getInstance().DataCtrlInit();
            });
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

        /************************************************************************************************************************
        ****************************************        DataCtrl        *********************************************************
        *************************************************************************************************************************/
        private void DataCtrlInit()
        {
            Thread initDataCtrlThread = new Thread(() =>
            {
                if(dataCtrl.GetSongInfo(SongInfoCallback))
                {
                    dataCtrl.GetServiceRecords(ServiceRecordsCallback);
                }
            });
            initDataCtrlThread.Start();
        }
        private static void SongInfoCallback(bool success, string message)
        {
            FormMain.getInstance().Invoke((MethodInvoker)delegate
            {
                FormMain.getInstance().HandleResponseSongInfo(success, message);
            });
        }
        private void HandleResponseSongInfo(bool success, string message)
        {
            if (success)
            {
                if (!loginForm.IsDisposed)
                {
                    loginForm.Dispose();
                }
                songTableForm.ImportSongInfoTable(dataCtrl.songInfoTable);
                songTableForm.Show();
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        private static void ServiceRecordsCallback(bool success, string message)
        {
            FormMain.getInstance().Invoke((MethodInvoker)delegate
            {
                FormMain.getInstance().HandleServiceRecordsResponse(success, message);
            });
        }
        private void HandleServiceRecordsResponse(bool success, string message)
        {

            if (success)
            {
                if (!loginForm.IsDisposed)
                {
                    loginForm.Dispose();
                }
                songTableForm.ImportServiceRecordsTable(dataCtrl.serviceRecordsTable);
                songTableForm.Show();
            }
            else
            {
                MessageBox.Show(message);
            }
        }
    }
}
