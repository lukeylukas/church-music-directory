﻿using System;
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

        private bool loggedIn = false;
        private bool songInfoReceived = false;
        private bool serviceRecordsReceived = false;

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
            if (Properties.Settings.Default.RememberLogin)
            {
                DataCtrlInit(Properties.Settings.Default.Username, Properties.Settings.Default.Password);
            }
            else
            {
                loginForm.Show();
            }
        }
        private void Setup()
        {
            dataCtrl = new DataCtrl();
            SetupLoginForm();
        }
        private void SetupSongTableForm()
        {
            songTableForm = new FormSongTables(this, dataCtrl);
            songTableForm.TopLevel = false;
            songTableForm.AutoScroll = true;
            songTableForm.FormBorderStyle = FormBorderStyle.None;
            songTableForm.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            songTableForm.Size = panelMain.Size;
            panelMain.Controls.Add(songTableForm);
        }
        private void SetupLoginForm()
        {
            loginForm = new FormLogin();
            loginForm.TopLevel = false;
            loginForm.AutoScroll = true;
            loginForm.Location = new System.Drawing.Point(100, 100);
            loginForm.FormBorderStyle = FormBorderStyle.None;
            panelMain.Controls.Add(loginForm);
        }
        private void SetupServicePlannerForm()
        {
            servicePlannerForm = new FormServicePlanner(dataCtrl);
            servicePlannerForm.TopLevel = false;
            servicePlannerForm.AutoScroll = true;
            servicePlannerForm.FormBorderStyle = FormBorderStyle.None;
            servicePlannerForm.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            servicePlannerForm.Location = new System.Drawing.Point(panelMain.Width - servicePlannerForm.Width, 0);
            panelMain.Controls.Add(servicePlannerForm);
        }

        public static void LoginToApplication(string userName, string password)
        {
            FormMain.getInstance().Invoke((MethodInvoker)delegate
            {
                FormMain.getInstance().DataCtrlInit(userName, password);
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

        private void DataResponseHandler()
        {
            LoginComplete();

            if (songInfoReceived && serviceRecordsReceived)
            {
                UseFilledDataCtrlObject();
            }
        }
        private void LoginComplete()
        {
            if (!loggedIn)
            {
                loggedIn = true;

                if (!loginForm.IsDisposed)
                {
                    loginForm.Dispose();
                }
            }
        }
        private void UseFilledDataCtrlObject()
        {
            SetupSongTableForm();
            songTableForm.ImportSongInfoTable(dataCtrl.songInfoTable);
            songTableForm.ImportServiceRecordsTable(dataCtrl.serviceRecordsTable);
            songTableForm.Show();

            SetupServicePlannerForm();
        }

        /************************************************************************************************************************
        ****************************************        DataCtrl        *********************************************************
        *************************************************************************************************************************/
        private void DataCtrlInit(string userName, string password)
        {
            Thread initDataCtrlThread = new Thread(() =>
            {
                dataCtrl.SetUserNameAndPassword(userName, password);
                if(dataCtrl.GetSongInfo(SongInfoCallback))
                {
                    dataCtrl.GetServiceRecords(ServiceRecordsCallback);
                    dataCtrl.GenerateCalculatedData();
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
                songInfoReceived = true;
                DataResponseHandler();
            }
            else
            {
                MessageBox.Show(message);
                loginForm.Show();
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
                serviceRecordsReceived = true;
                DataResponseHandler();
            }
            else
            {
                MessageBox.Show(message);
            }
        }
    }
}
