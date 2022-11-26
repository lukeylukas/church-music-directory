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
        const string serverName = "ChurchMusicServer1";
        const string serverIpAddress = "localhost";
        const int serverPort = 1433;
        enum SONG_ATTRIBUTE
        {
            songName,
            musicKey,
            subject,
            tag,
            numPlays,
            COUNT
        }
        public struct TABLE_COLUMN
        {
            public bool allowFiltering;
            public List<string> values;
            public int width;
        };
        private TABLE_COLUMN[] songInfoColumns = new TABLE_COLUMN[(int)SONG_ATTRIBUTE.COUNT];
        public FormMain()
        {
            InitializeComponent();
            SongInfoSizeElements();
        }
        private void SongInfoSizeElements()
        {
            songInfoColumns[(int)SONG_ATTRIBUTE.songName].width = 100;
            songInfoColumns[(int)SONG_ATTRIBUTE.musicKey].width = 100;
            songInfoColumns[(int)SONG_ATTRIBUTE.subject].width = 150;
            songInfoColumns[(int)SONG_ATTRIBUTE.tag].width = 100;
            songInfoColumns[(int)SONG_ATTRIBUTE.numPlays].width = 100;
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
                builder.DataSource = serverIpAddress + "," + serverPort;
                builder.UserID = username;
                builder.Password = password;
                builder.InitialCatalog = "ProvidenceSongs";
                builder.TrustServerCertificate = true;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    string columns = "";
                    for (SONG_ATTRIBUTE attribute = 0; attribute < SONG_ATTRIBUTE.COUNT; attribute++)
                    {
                        columns += attribute + ", ";
                    }
                    columns = columns.Substring(0, columns.Length - 2);

                    String sql = "SELECT " + columns + " FROM songInfo";

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
                MessageBox.Show(e.Message);
            }
        }

        private static void ReadData(FormMain formPassedFromAbove, SqlDataReader reader)
        {
            formPassedFromAbove.dataGridView1.Rows.Clear();
            formPassedFromAbove.dataGridView1.ColumnCount = reader.FieldCount;
            for (int columnIndex = 0; columnIndex < reader.FieldCount; columnIndex++)
            {
                string colName = reader.GetName(columnIndex);
                formPassedFromAbove.dataGridView1.Columns[columnIndex].Width = formPassedFromAbove.songInfoColumns[columnIndex].width;
                formPassedFromAbove.dataGridView1.Columns[columnIndex].Name = colName;
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

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            switch (me.Button)
            {
                case MouseButtons.Left:  /* Automatically sorts */                             break;
                case MouseButtons.Right: ShowHeaderContextMenu((SONG_ATTRIBUTE)e.ColumnIndex); break;
            }
        }
        private void ShowHeaderContextMenu(SONG_ATTRIBUTE column)
        {
            //if it's a filterable attribute, show all the filterable types
            //create a struct enumerated by attributes containing lists of filter values for each attribute 
            ContextMenuFilterList(songInfoColumns[column].valueList);
        }
        private void ContextMenuFilterList(List values)
        {
            //for each value, create a menu entry with checkbox
        }
    }
}