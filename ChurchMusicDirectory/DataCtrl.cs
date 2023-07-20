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
    public class DataCtrl
    {
        public DataTable songInfoTable;
        public DataTable serviceRecordsTable;
        private Dictionary<DateTime, Dictionary<int, Dictionary<int, DataRow>>> serviceRecordsDictionary;
        public delegate void DataCtrlResponseHandler(bool success, string message);
        
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
            serviceDatesList.Reverse(); // puts most recent date first
            return serviceDatesList;
        }

        public bool GetSongInfo(DataCtrlResponseHandler callback)
        {
            string songInfoQuery = BuildSongInfoQuery(out int expectedNumColumns);

            bool songInfoReceived = GetTableData(out DataTable tempTable, songInfoQuery, expectedNumColumns, out string statusMessage);
            if (songInfoReceived)
            {
                for (int columnNum = 0; columnNum < FormSongTables.songInfoColumns.Length; columnNum++)
                {
                    if (!FormSongTables.songInfoColumns[columnNum].isDerived)
                    {
                        songInfoTable.Columns.Add(tempTable.Columns[0].ColumnName);
                        for (int rowNum = 0; rowNum < tempTable.Rows.Count; rowNum++)
                        {
                            if (songInfoTable.Rows.Count <= rowNum)
                            {
                                songInfoTable.Rows.Add();
                            }
                            songInfoTable.Rows[rowNum][columnNum] = tempTable.Rows[rowNum][0];
                        }
                        tempTable.Columns.RemoveAt(0);
                    }
                    else
                    {
                        songInfoTable.Columns.Add();
                    }
                }
            }

            GenerateTitlesList();
            callback(songInfoReceived, statusMessage);
            return songInfoReceived;
        }
        private string BuildSongInfoQuery(out int numColumns)
        {
            string columns = "";
            string connectorString = ", ";
            numColumns = 0;
            for (int columnNum = 0; columnNum < FormSongTables.songInfoColumns.Length; columnNum++)
            {
                SONG_ATTRIBUTE source = (SONG_ATTRIBUTE)FormSongTables.songInfoColumns[columnNum].id;
                if (!FormSongTables.songInfoColumns[columnNum].isDerived)
                {
                    columns += source + connectorString;
                    numColumns++;
                }
            }

            columns = columns.Substring(0, columns.Length - connectorString.Length);

            return "SELECT " + columns + " FROM songInfo";
        }
        private bool GetTableData(out DataTable destTable, string query, int expectedNumColumns, out string message)
        {
            bool receivedCorrectTable = false;
            message = "";
            destTable = new DataTable();

            try
            {
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
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return receivedCorrectTable;
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

        public bool GetServiceRecords(DataCtrlResponseHandler callback)
        {
            string serviceRecordsQuery = BuildServiceRecordsQuery(out int expectedNumColumns);

            bool serviceRecordsReceived = GetTableData(out serviceRecordsTable, serviceRecordsQuery, expectedNumColumns, out string statusMessage);

            GenerateServiceRecordsDictionary();

            callback(serviceRecordsReceived, statusMessage);

            return serviceRecordsReceived;
        }
        private string BuildServiceRecordsQuery(out int numColumns)
        {
            string columns = "";
            string connectorString = ", ";
            numColumns = 0;
            for (int columnNum = 0; columnNum < FormSongTables.serviceRecordColumns.Length; columnNum++)
            {
                SERVICE_RECORD_ATTRIBUTE source = (SERVICE_RECORD_ATTRIBUTE)FormSongTables.serviceRecordColumns[columnNum].id;
                if (!FormSongTables.serviceRecordColumns[columnNum].isDerived)
                {
                    columns += source + connectorString;
                    numColumns++;
                }
            }
            columns = columns.Substring(0, columns.Length - connectorString.Length);

            return "SELECT " + columns + " FROM serviceRecords";
        }
        private void GenerateServiceRecordsDictionary()
        {
            serviceRecordsDictionary = new Dictionary<DateTime, Dictionary<int, Dictionary<int, DataRow>>>();

            for (int rowIndex = 0; rowIndex < serviceRecordsTable.Rows.Count; rowIndex++)
            {
                object serviceDate = serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.date];
                if (serviceDate != null)
                {
                    if (!serviceRecordsDictionary.ContainsKey((DateTime)serviceDate))
                    {
                        serviceRecordsDictionary.Add((DateTime)serviceDate, new Dictionary<int, Dictionary<int, DataRow>>());
                    }
                    object serviceNumber = serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.serviceNumber];
                    if (serviceNumber != null)
                    {
                        if (!serviceRecordsDictionary[(DateTime)serviceDate].ContainsKey((int)serviceNumber))
                        {
                            serviceRecordsDictionary[(DateTime)serviceDate].Add((int)serviceNumber, new Dictionary<int, DataRow>());
                        }
                        object orderInService = serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.orderInService];
                        if (orderInService != null)
                        {
                            if (!serviceRecordsDictionary[(DateTime)serviceDate][(int)serviceNumber].ContainsKey((int)orderInService))
                            {
                                serviceRecordsDictionary[(DateTime)serviceDate][(int)serviceNumber].Add((int)orderInService, serviceRecordsTable.Rows[rowIndex]);
                            }
                        }
                    }
                }
            }
        }

        public DataTable GetServiceInfo(DateTime date, int serviceNumber)
        {
            DataTable serviceInfoTable = new DataTable();
            if (serviceRecordsDictionary.ContainsKey(date))
            {
                if (serviceRecordsDictionary[date].ContainsKey(serviceNumber))
                {
                    for (int columnNum = 0; columnNum < FormSongTables.serviceRecordColumns.Length; columnNum++)
                    {
                        SERVICE_RECORD_ATTRIBUTE source = (SERVICE_RECORD_ATTRIBUTE)FormSongTables.serviceRecordColumns[columnNum].id;
                        serviceInfoTable.Columns.Add(source.ToString());
                    }
                    for (int orderInService = 1; orderInService <= serviceRecordsDictionary[date][serviceNumber].Count; orderInService++)
                    {
                        serviceInfoTable.Rows.Add(serviceRecordsDictionary[date][serviceNumber][orderInService].ItemArray);
                    }
                }
            }
            return serviceInfoTable;
        }

        public void GenerateCalculatedData()
        {
            GenerateCalculatedSongInfo();
        }

        private void GenerateCalculatedSongInfo()
        {
            GenerateNumPlays();
        }
        private void GenerateNumPlays()
        {
            Dictionary<string, int> songNameNumPlaysDict = GetNumPlaysFromServiceRecords();

            SaveNumPlaysToSongInfo(songNameNumPlaysDict);
        }

        private Dictionary<string, int> GetNumPlaysFromServiceRecords()
        {
            Dictionary<string, int> numPlaysDict = new Dictionary<string, int>();

            for (int rowIndex = 0; rowIndex < serviceRecordsTable.Rows.Count; rowIndex++)
            {
                string songName = (string)serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.title];
                if (songName != null)
                {
                    if (!numPlaysDict.ContainsKey(songName))
                    {
                        numPlaysDict.Add(songName, 1);
                    }
                    else
                    {
                        numPlaysDict[songName]++;
                    }
                }
            }
            return numPlaysDict;
        }
        private void SaveNumPlaysToSongInfo(Dictionary<string, int> numPlaysDict)
        {
            for (int rowIndex = 0; rowIndex < songInfoTable.Rows.Count; rowIndex++)
            {
                if (songInfoTable.Rows[rowIndex][(int)SONG_ATTRIBUTE.songName] != DBNull.Value)
                {
                    string songName = (string)songInfoTable.Rows[rowIndex][(int)SONG_ATTRIBUTE.songName];
                    if (numPlaysDict.ContainsKey(songName))
                    {
                        songInfoTable.Rows[rowIndex][(int)SONG_ATTRIBUTE.numPlays] = numPlaysDict[songName];
                    }
                    else
                    {
                        songInfoTable.Rows[rowIndex][(int)SONG_ATTRIBUTE.numPlays] = 0;
                    }
                }
            }
        }
    }
}
