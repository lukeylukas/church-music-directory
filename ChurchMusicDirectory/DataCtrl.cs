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
            title,
            musicKey,
            passage,
            elementName,
            serviceNumber,
            orderInService,
            notes,
            COUNT
        }
        static List<char> flatsList = new List<char>
        {
            'A',
            'B',
            'D',
            'E',
            'G'
        };

        public List<string> worshipElements;

        public List<string> musicKeys;

        public List<string> titlesList;

        public DataCtrl()
        {
            songInfoTable = new DataTable();
            serviceRecordsTable = new DataTable();
            worshipElements = new List<string>
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
            GenerateMusicKeysList();
            titlesList = new List<string>();
        }

        private void GenerateMusicKeysList()
        {
            musicKeys = new List<string>();
            for (char seedChar = 'A'; seedChar <= 'G'; seedChar++)
            {
                if (flatsList.Contains(seedChar))
                {
                    musicKeys.Add(seedChar.ToString() + Convert.ToChar(0x266D)); //musical flat
                    musicKeys.Add(seedChar.ToString() + Convert.ToChar(0x266D) + "min");
                }
                musicKeys.Add(seedChar.ToString());
                musicKeys.Add(seedChar.ToString() + "min");
            }
        }
        private void GenerateTitlesList()
        {
            int columnIndex = (int)SONG_ATTRIBUTE.songName;
            foreach (DataRow item in this.songInfoTable.Rows)
            {
                if (item.ItemArray[columnIndex] != null)
                {
                    titlesList.Add(item.ItemArray[columnIndex].ToString());
                }
            }
        }
        public List<DateTime> GetServiceDatesList()
        {
            List < DateTime > serviceDatesList = new List < DateTime >();

            for (int rowIndex = 0; rowIndex < serviceRecordsTable.Rows.Count; rowIndex++)
            {
                object serviceDate = serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.date];
                if (serviceDate != null)
                {
                    if (!serviceDatesList.Contains((DateTime)serviceDate))
                    {
                        serviceDatesList.Add((DateTime)serviceDate);
                    }
                }
            }
            serviceDatesList.Sort();
            serviceDatesList.Reverse();
            return serviceDatesList;
        }

        public bool GetSongInfo(DataCtrlResponseHandler callback)
        {
            string songInfoQuery = BuildSongInfoQuery();
            int expectedNumColumns = (int)SONG_ATTRIBUTE.COUNT;

            bool songInfoReceived = GetTableData(out songInfoTable, songInfoQuery, expectedNumColumns, out string statusMessage);

            GenerateTitlesList();
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

        public bool GetServiceRecords(DataCtrlResponseHandler callback)
        {
            string serviceRecordsQuery = BuildServiceRecordsQuery();
            int expectedNumColumns = (int)SERVICE_RECORD_ATTRIBUTE.COUNT;

            bool serviceRecordsReceived = GetTableData(out serviceRecordsTable, serviceRecordsQuery, expectedNumColumns, out string statusMessage);

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
    }
}
