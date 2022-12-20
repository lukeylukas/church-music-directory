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
        public DataTable serviceRecordsTable;
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
        public enum SERVICE_RECORD_ATTRIBUTE
        {
            date,
            serviceNumber,
            orderInService,
            elementName,
            title,
            musicKey,
            passage,
            notes,
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

        public List<string> worshipElements = new List<string>
        {
            "Song",
            "Sermon",
            "Offering",
            "Prayer",
            "Pastoral Prayer",
            "Communion",
            "Invocation",
            "Benediction",
            "Responsive Reading",
            "Scripture Reading",
            "Announcements",
            "Greeting",
        };

        public DataCtrl()
        {
            songInfoTable = new DataTable();
            serviceRecordsTable = new DataTable();
        }

        public bool GetSongInfo(DataCtrlResponseHandler callback)
        {
            string songInfoQuery = BuildSongInfoQuery();
            int expectedNumColumns = (int)SONG_ATTRIBUTE.COUNT;

            bool songInfoReceived = GetTableData(out songInfoTable, songInfoQuery, expectedNumColumns, out string statusMessage);
            if (statusMessage.Length != 0)
            {
                MessageBox.Show(statusMessage);
            }

            callback(songInfoReceived, statusMessage);
            return songInfoReceived;
        }

        private string BuildSongInfoQuery()
        {
            string columns = "";
            string connectorString = ", ";
            for (SONG_ATTRIBUTE attribute = 0; attribute < SONG_ATTRIBUTE.COUNT; attribute++)
            {
                columns += attribute + connectorString;
            }
            columns = columns.Substring(0, columns.Length - connectorString.Length);

            return "SELECT " + columns + " FROM songInfo";
        }

        public bool GetTableData(out DataTable destTable, string query, int expectedNumColumns, out string message)
        {
            bool receivedCorrectTable = false;
            message = "";
            destTable = new DataTable();

            if (ServerCommunication.QuerySqlServer(query, out DataTable tempTable))
            {
                if (tempTable.Columns.Count == expectedNumColumns)
                {
                    receivedCorrectTable = true;
                    destTable = tempTable;
                }
                else
                {
                    message = "Server sent incorrect data size.";
                }
            }
            else
            {
                message = "Response not received.";
            }

            return receivedCorrectTable;
        }

        public bool GetServiceRecords(DataCtrlResponseHandler callback)
        {
            string serviceRecordsQuery = BuildServiceRecordsQuery();
            int expectedNumColumns = (int)SERVICE_RECORD_ATTRIBUTE.COUNT;

            bool serviceRecordsReceived = GetTableData(out serviceRecordsTable, serviceRecordsQuery, expectedNumColumns, out string statusMessage);
            if (statusMessage.Length != 0)
            {
                MessageBox.Show(statusMessage);
            }

            callback(serviceRecordsReceived, statusMessage);
            return serviceRecordsReceived;
        }
        private string BuildServiceRecordsQuery()
        {
            string columns = "";
            string connectorString = ", ";
            for (SERVICE_RECORD_ATTRIBUTE attribute = 0; attribute < SERVICE_RECORD_ATTRIBUTE.COUNT; attribute++)
            {
                columns += attribute + connectorString;
            }
            columns = columns.Substring(0, columns.Length - connectorString.Length);

            return "SELECT " + columns + " FROM serviceRecords";
        }
    }
}
