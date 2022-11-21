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
            // show persistent login info if box is checked.
            // box will be a persistent boolean
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMain formMain = new FormMain();
            FormMain.getSongInfo(formMain, richTextBoxUsername.Text, richTextBoxPassword.Text);
            formMain.Show();
        }
    }
}
