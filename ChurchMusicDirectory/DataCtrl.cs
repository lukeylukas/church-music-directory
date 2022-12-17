using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChurchMusicDirectory.DataCtrl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ChurchMusicDirectory
{
    public class DataCtrl
    {
        public DataTable songInfoTable;
        public DataTable theOtherTable;
        public delegate void DataCtrlResponseHandler(bool success, string message);
        public enum SONG_ATTRIBUTE
        {
            songName,
            musicKey,
            subject,
            numPlays,
            tag,
            COUNT
        }
        public struct TABLE_COLUMN
        {
            public bool allowFiltering;
            public List<string> filterValues;
            public int width;
            public string name;
        };
        public static TABLE_COLUMN[] songInfoColumns = new TABLE_COLUMN[(int)SONG_ATTRIBUTE.COUNT]
        {
            new TABLE_COLUMN
            {
                allowFiltering = false,
                filterValues = new List<string>(),
                width = 200,
                name = "Title"
            },
            new TABLE_COLUMN
            {
                allowFiltering = true,
                filterValues = new List<string>(),
                width = 75,
                name = "Key"
            },
            new TABLE_COLUMN
            {
                allowFiltering = true,
                filterValues = new List<string>(),
                width = 200,
                name = "Subject"
            },
            new TABLE_COLUMN
            {
                allowFiltering = true,
                filterValues = new List<string>(),
                width = 75,
                name = "Plays"
            },
            new TABLE_COLUMN
            {
                allowFiltering = false,
                filterValues = new List<string>(),
                width = 100,
                name = "Notes"
            },
        };

        public DataCtrl()
        {
            songInfoTable = new DataTable();
            theOtherTable = new DataTable();
        }

        public bool GetSongInfo(DataCtrlResponseHandler callback)
        {
            bool songInfoReceived = false;
            string statusMessage = "";

            string columns = "";
            for (SONG_ATTRIBUTE attribute = 0; attribute < SONG_ATTRIBUTE.COUNT; attribute++)
            {
                columns += attribute + ", ";
            }
            columns = columns.Substring(0, columns.Length - 2);

            string query = "SELECT " + columns + " FROM songInfo";

            DataTable tempTable = new DataTable();

            if (ServerCommunication.QuerySqlServer(query, out tempTable))
            {
                if (tempTable.Columns.Count == (int)SONG_ATTRIBUTE.COUNT)
                {
                    songInfoReceived = true;
                    songInfoTable = tempTable;
                }
                else
                {
                    statusMessage = "Server sent incorrect data size.";
                    MessageBox.Show(statusMessage);
                }
            }
            else
            {
                statusMessage = "Response not received";
                MessageBox.Show(statusMessage);
            }

            callback(songInfoReceived, statusMessage);
            return songInfoReceived;
        }
    }
}
