using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace ChurchMusicDirectory
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public static void getSongInfo(FormMain formPassedFromAbove, string username, string password)
        {
            Thread databaseThread = new Thread(() =>
            {
                dataGrabber(formPassedFromAbove, username, password);
            });
            databaseThread.Start();
        }
        public static void dataGrabber(FormMain formPassedFromAbove, string username, string password)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = ".\\SQLExpress";
                builder.UserID = username;
                builder.Password = password;
                builder.InitialCatalog = "ProvidenceSongs";
                builder.TrustServerCertificate = true;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {

                    String sql = "SELECT * FROM songInfo";
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            formPassedFromAbove.Invoke(new Action(() => ReadData(formPassedFromAbove, reader)));
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private static void ReadData(FormMain formPassedFromAbove, SqlDataReader reader)
        {
            formPassedFromAbove.dataGridView1.Rows.Clear();
            formPassedFromAbove.dataGridView1.ColumnCount = reader.FieldCount;
            for (int idx = 0; idx < reader.FieldCount; idx++)
            {
                string colName = reader.GetName(idx);
                formPassedFromAbove.dataGridView1.Columns[idx].Name = colName;
            }
            while (reader.Read())
            {
                object[] row = new object[reader.FieldCount];
                reader.GetProviderSpecificValues(row);
                formPassedFromAbove.dataGridView1.Rows.Add(row);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPageSongInfo_Click(object sender, EventArgs e)
        {

        }
    }
}