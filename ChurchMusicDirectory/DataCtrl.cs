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
        hymnalNumber,
        musicKey,
        hymnalKey,
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
        notes,
        orderInService,
        COUNT
    }
    public enum ColumnType
    {
        String,
        Int,
        Date
    }
    public class DataCtrl
    {
        private const string serviceRecordsTableName = "serviceRecords";
        private const string songInfoTableName = "songInfo";
        public DataTable songInfoTable;
        public DataTable serviceRecordsTable;
        private string serverUserName;
        private string serverPassword;
        private Dictionary<DateTime, Dictionary<int, DataRow>> serviceRecordsDictionary = new Dictionary<DateTime, Dictionary<int, DataRow>>();
        public delegate void DataCtrlResponseHandler(bool success, string message);
        
        static List<char> flatsList = new List<char>
        {
            'A',
            'B',
            'D',
            'E',
            'G'
        };

        public List<string> musicKeys;

        public List<string> titlesList;

        public DataCtrl()
        {
            songInfoTable = new DataTable();
            serviceRecordsTable = new DataTable();
            GenerateMusicKeysList();
            titlesList = new List<string>();
        }

        private void GenerateMusicKeysList()
        {
            musicKeys = new List<string>();
            for (char seedChar = 'A'; seedChar <= 'G'; seedChar++)
            {
                musicKeys.Add(seedChar.ToString());
                musicKeys.Add(seedChar.ToString() + "min");
                if (flatsList.Contains(seedChar))
                {
                    musicKeys.Add(seedChar.ToString() + Convert.ToChar(0x266D)); //musical flat
                    musicKeys.Add(seedChar.ToString() + Convert.ToChar(0x266D) + "min");
                }
            }
        }

        public void Refresh()
        {
            if (GetSongInfo(HandleDataCtrlResponse))
            {
                GetServiceRecords(HandleDataCtrlResponse);
            }
        }
        private void HandleDataCtrlResponse(bool success, string message)
        {
            if (!success)
            {
                MessageBox.Show(message);
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
                songInfoTable.Reset();
                for (SONG_ATTRIBUTE columnNum = 0; columnNum < SONG_ATTRIBUTE.COUNT; columnNum++)
                {
                    DataColumn column = new DataColumn();
                    switch (FormSongTables.songInfoColumns[columnNum].columnType)
                    {
                        case ColumnType.String: column.DataType = typeof(string);    break;
                        case ColumnType.Int:    column.DataType = typeof(int);       break;
                        case ColumnType.Date:   column.DataType = typeof(DateTime);  break;
                        default:                throw new InvalidEnumArgumentException();
                    }
                    songInfoTable.Columns.Add(column);
                    if (!FormSongTables.songInfoColumns[columnNum].isDerived)
                    {
                        songInfoTable.Columns[(int)columnNum].ColumnName = tempTable.Columns[0].ColumnName;
                        for (int rowNum = 0; rowNum < tempTable.Rows.Count; rowNum++)
                        {
                            if (songInfoTable.Rows.Count <= rowNum)
                            {
                                songInfoTable.Rows.Add();
                            }
                            songInfoTable.Rows[rowNum][(int)columnNum] = tempTable.Rows[rowNum][0];
                        }
                        tempTable.Columns.RemoveAt(0);
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
            for (SONG_ATTRIBUTE attribute = (SONG_ATTRIBUTE)0; attribute < SONG_ATTRIBUTE.COUNT; attribute++)
            {
                if (!FormSongTables.songInfoColumns[attribute].isDerived)
                {
                    columns += attribute.ToString() + connectorString;
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
                DataTable tempTable = ServerCommunication.QuerySqlServer(query, serverUserName, serverPassword);
                if (tempTable != null)
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
            titlesList.Clear();
            foreach (DataRow item in this.songInfoTable.Rows)
            {
                if (item.ItemArray[columnIndex] != null)
                {
                    titlesList.Add(item.ItemArray[columnIndex].ToString());
                }
            }
        }
        public int GetHymnalNumber(string title)
        {
            foreach (DataRow item in this.songInfoTable.Rows)
            {
                if (item.ItemArray[(int)SONG_ATTRIBUTE.songName] is not DBNull && item.ItemArray[(int)SONG_ATTRIBUTE.hymnalNumber] is not DBNull)
                {
                    if (item.ItemArray[(int)SONG_ATTRIBUTE.songName].ToString() == title)
                    {
                        return (int)item.ItemArray[(int)SONG_ATTRIBUTE.hymnalNumber];
                    }
                }
            }
            return -1;
        }

        public bool GetServiceRecords(DataCtrlResponseHandler callback)
        {
            string serviceRecordsQuery = BuildServiceRecordsQuery(out int expectedNumColumns);
            serviceRecordsTable.Reset();
            bool serviceRecordsReceived = GetTableData(out serviceRecordsTable, serviceRecordsQuery, expectedNumColumns, out string statusMessage);

            GenerateServiceRecordsDictionary();

            callback(serviceRecordsReceived, statusMessage);

            AddServiceRecordKeysToSongInfo();

            return serviceRecordsReceived;
        }
        private string BuildServiceRecordsQuery(out int numColumns)
        {
            string columns = "";
            string connectorString = ", ";
            numColumns = 0;
            for (SERVICE_RECORD_ATTRIBUTE columnName = 0; columnName < SERVICE_RECORD_ATTRIBUTE.COUNT; columnName++)
            {
                if (!FormSongTables.serviceRecordColumns[columnName].isDerived)
                {
                    columns += columnName.ToString() + connectorString;
                    numColumns++;
                }
            }
            columns = columns.Substring(0, columns.Length - connectorString.Length);

            return "SELECT " + columns + " FROM " + serviceRecordsTableName;
        }
        private void GenerateServiceRecordsDictionary()
        {
            serviceRecordsDictionary.Clear();

            for (int rowIndex = 0; rowIndex < serviceRecordsTable.Rows.Count; rowIndex++)
            {
                object serviceDate = serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.date];
                if (serviceDate != null)
                {
                    if (!serviceRecordsDictionary.ContainsKey((DateTime)serviceDate))
                    {
                        serviceRecordsDictionary.Add((DateTime)serviceDate, new Dictionary<int, DataRow>());
                    }
                    object orderInService = serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.orderInService];
                    if (orderInService != null)
                    {
                        if (!serviceRecordsDictionary[(DateTime)serviceDate].ContainsKey((int)orderInService))
                        {
                            serviceRecordsDictionary[(DateTime)serviceDate].Add((int)orderInService, serviceRecordsTable.Rows[rowIndex]);
                        }
                    }
                }
            }
        }

        private void AddServiceRecordKeysToSongInfo()
        {
            //also for every song in songInfo
            for (int rowIndex = 0; rowIndex < songInfoTable.Rows.Count; rowIndex++)
            {
                //if any record in serviceRecords has a key not in the songInfo table, add it to the songInfo table
                string songName = (string)songInfoTable.Rows[rowIndex][(int)SONG_ATTRIBUTE.songName];
                if (songName != null)
                {
                    //create empty list of musicKeys
                    List<string> musicKeys = new List<string>();
                    for (int serviceRowIndex = 0; serviceRowIndex < serviceRecordsTable.Rows.Count; serviceRowIndex++)
                    {
                        //if songName matches the songName in the serviceRecords table
                        if (songName == (string)serviceRecordsTable.Rows[serviceRowIndex][(int)SERVICE_RECORD_ATTRIBUTE.title])
                        {
                            // check whether the musicKey is represented in the songInfo table
                            object musicKey = serviceRecordsTable.Rows[serviceRowIndex][(int)SERVICE_RECORD_ATTRIBUTE.musicKey];
                            //check if musicKey is empty
                            if (musicKey != DBNull.Value)
                            {
                                //check if musicKey is in the songInfo table
                                if (!musicKeys.Contains((string)musicKey))
                                {
                                    //add musicKey to list of musicKeys
                                    musicKeys.Add((string)musicKey);
                                }
                            }
                        }
                    }
                    // if any of musicKeys is not in the songInfo table, add it
                    foreach (string musicKey in musicKeys)
                    {
                        object songInfoKeys = songInfoTable.Rows[rowIndex][(int)SONG_ATTRIBUTE.musicKey];
                        if (songInfoKeys == DBNull.Value)
                        {
                            songInfoTable.Rows[rowIndex][(int)SONG_ATTRIBUTE.musicKey] = musicKey;
                        }
                        else if (!((string)songInfoKeys).Contains(musicKey))
                        {
                            songInfoTable.Rows[rowIndex][(int)SONG_ATTRIBUTE.musicKey] += ", " + musicKey;
                        }
                    }
                }
            }
        }

        public DataTable GetServiceInfo(DateTime date)
        {
            DataTable serviceInfoTable = new DataTable();
            if (serviceRecordsDictionary.ContainsKey(date))
            {
                for (SERVICE_RECORD_ATTRIBUTE columnName = 0; columnName < SERVICE_RECORD_ATTRIBUTE.COUNT; columnName++)
                {;
                    serviceInfoTable.Columns.Add(columnName.ToString(), serviceRecordsTable.Columns[columnName.ToString()].DataType);
                }
                for (int orderInService = 1; orderInService <= serviceRecordsDictionary[date].Count; orderInService++)
                {
                    serviceInfoTable.Rows.Add(serviceRecordsDictionary[date][orderInService].ItemArray);
                }
            }
            else
            {
                DateTime sampleDate = serviceRecordsDictionary.Keys.First();
                serviceInfoTable = GetServiceInfo(sampleDate);
                serviceInfoTable = serviceInfoTable.AsEnumerable().Take(1).CopyToDataTable();
                for (int columnIndex = 0; columnIndex < serviceInfoTable.Columns.Count; columnIndex++)
                {
                    serviceInfoTable.Rows[0][columnIndex] = DBNull.Value;
                }
                serviceInfoTable.Rows[0][(int)SERVICE_RECORD_ATTRIBUTE.date] = date;
                serviceInfoTable.Rows[0][(int)SERVICE_RECORD_ATTRIBUTE.orderInService] = 1;
            }
            return serviceInfoTable;
        }
        public void SetServiceInfo(DataTable newServiceTable)
        {
            newServiceTable = TrimTable(newServiceTable);
            if (newServiceTable.Rows.Count > 0)
            {
                SaveToDictionary(newServiceTable);
                SaveToServiceRecordsTable(newServiceTable);
                string query = BuildServicePlannerQuery(newServiceTable, out string[] values);
                ServerCommunication.CommandSqlServer(query, values, serverUserName, serverPassword);
            }
        }

        private DataTable TrimTable(DataTable newServiceTable)
        {
            //make list for rows to remove
            List<int> rowsToRemove = new List<int>();
            for (int rowIndex = 0; rowIndex < newServiceTable.Rows.Count; rowIndex++)
            {
                if (newServiceTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.title].ToString() == "")
                {
                    rowsToRemove.Add(rowIndex);
                }
            }
            //remove rows
            for (int rowIndex = rowsToRemove.Count - 1; rowIndex >= 0; rowIndex--)
            {
                newServiceTable.Rows.RemoveAt(rowsToRemove[rowIndex]);
            }
            // sort the rows by orderInService
            newServiceTable.DefaultView.Sort = SERVICE_RECORD_ATTRIBUTE.orderInService.ToString() + " ASC";
            // re-number orderInService
            for (int rowIndex = 0; rowIndex < newServiceTable.Rows.Count; rowIndex++)
            {
                newServiceTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.orderInService] = rowIndex + 1;
            }
            return newServiceTable;
        }

        private void SaveToDictionary(DataTable newServiceTable)
        {
            // add new date to dict if not present
            if (!serviceRecordsDictionary.ContainsKey((DateTime)newServiceTable.Rows[0][(int)SERVICE_RECORD_ATTRIBUTE.date]))
            {
                serviceRecordsDictionary.Add((DateTime)newServiceTable.Rows[0][(int)SERVICE_RECORD_ATTRIBUTE.date], new Dictionary<int, DataRow>());
            }
            for (int rowIndex = 0; rowIndex < newServiceTable.Rows.Count; rowIndex++)
            {
                if (newServiceTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.date] == DBNull.Value
                    || newServiceTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.orderInService] == DBNull.Value)
                {
                    throw new Exception("Invalid service record.");
                }
                serviceRecordsDictionary[(DateTime)newServiceTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.date]][(int)newServiceTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.orderInService]] = newServiceTable.Rows[rowIndex];
            }
        }

        private void SaveToServiceRecordsTable(DataTable newServiceTable)
        {
            List<DataRow> rowsToRemove = new List<DataRow>();
            foreach (DataRow row in serviceRecordsTable.Rows)
            {
                if ((DateTime)row[(int)SERVICE_RECORD_ATTRIBUTE.date] == (DateTime)newServiceTable.Rows[0][(int)SERVICE_RECORD_ATTRIBUTE.date])
                {
                    rowsToRemove.Add(row);
                }
            }
            foreach (DataRow row in rowsToRemove)
            {
                serviceRecordsTable.Rows.Remove(row);
            }
            serviceRecordsTable.Merge(newServiceTable);
            serviceRecordsTable.AcceptChanges();
        }

        private string BuildServicePlannerQuery(DataTable newServiceTable, out string[] values)
        {
            string deletionQuery = "DELETE FROM " + serviceRecordsTableName + 
                                    " WHERE " + SERVICE_RECORD_ATTRIBUTE.date + " = '" + newServiceTable.Rows[0][(int)SERVICE_RECORD_ATTRIBUTE.date] + "'";

            string insertionQuery = "INSERT INTO " + serviceRecordsTableName + " VALUES ";
            values = new string[newServiceTable.Rows.Count * newServiceTable.Columns.Count];
            for (int rowIndex = 0; rowIndex < newServiceTable.Rows.Count; rowIndex++)
            {
                insertionQuery += "(";
                for (int columnIndex = 0; columnIndex < newServiceTable.Columns.Count; columnIndex++)
                {
                    int valueIndex = rowIndex * newServiceTable.Columns.Count + columnIndex;
                    insertionQuery += "@" + valueIndex;
                    if (columnIndex < newServiceTable.Columns.Count - 1)
                    {
                        insertionQuery += ", ";
                    }
                    values[valueIndex] = newServiceTable.Rows[rowIndex][columnIndex].ToString();
                }
                insertionQuery += ")";
                if (rowIndex < newServiceTable.Rows.Count - 1)
                {
                    insertionQuery += ", ";
                }
            }
            return deletionQuery + "; " + insertionQuery;
        }

        public void GenerateCalculatedData()
        {
            GenerateCalculatedSongInfo();
        }

        private void GenerateCalculatedSongInfo()
        {
            GenerateNumPlays( DateTime.Now.AddMonths(-24), DateTime.Now );
        }

        public void GenerateNumPlays(DateTime earliestDate, DateTime latestDate)
        {
            Dictionary<string, int> songNameNumPlaysDict = GetNumPlaysFromServiceRecords(earliestDate, latestDate);

            SaveNumPlaysToSongInfo(songNameNumPlaysDict);
        }

        private Dictionary<string, int> GetNumPlaysFromServiceRecords(DateTime earliestDate, DateTime latestDate)
        {
            Dictionary<string, int> numPlaysDict = new Dictionary<string, int>();

            for (int rowIndex = 0; rowIndex < serviceRecordsTable.Rows.Count; rowIndex++)
            {
                string songName = (string)serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.title];
                DateTime serviceDate = (DateTime)serviceRecordsTable.Rows[rowIndex][(int)SERVICE_RECORD_ATTRIBUTE.date];
                if (songName != null && serviceDate >= earliestDate && serviceDate <= latestDate)
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
        public void SetUserNameAndPassword(string userName, string password)
        {
            serverUserName = userName;
            serverPassword = password;
        }
        public DateTime GetNextServiceDate()
        {
            DateTime mostRecentDate = DateTime.MinValue;
            foreach (DateTime key in serviceRecordsDictionary.Keys)
            {
                if (key > mostRecentDate)
                {
                    mostRecentDate = key;
                }
            }
            return mostRecentDate.AddDays(7);
        }

        public void EditSong(string name, string hymnalNum, string hymnalKey, string key, string subject, string notes, bool withRefresh)
        {
            if (name is not null && name != "")
            {
                DeleteSong(name, false);
                AddSong(name, hymnalNum, hymnalKey, key, subject, notes);
                if (withRefresh)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBox.Show("Can't save song with no name");
            }
        }

        public void DeleteSong(string name, bool withRefresh)
        {
            if (name is not null && name != "")
            {
                if (titlesList.Contains(name))
                {
                    string delete_query = BuildSongDeleteQuery(out string[] delete_values, name);
                    ServerCommunication.CommandSqlServer(delete_query, delete_values, serverUserName, serverPassword);
                    if (withRefresh)
                    {
                        Refresh();
                    }
                }
                else
                {
                    MessageBox.Show("Can't delete song. Song name '" + name + "' not found.");
                }
            }
            else
            {
                MessageBox.Show("Can't delete song with no name");
            }
        }
        private string BuildSongDeleteQuery(out string[] values, string name)
        {
            values = new string[] {name};
            return "DELETE FROM " + songInfoTableName + " WHERE " + SONG_ATTRIBUTE.songName + " = '@0'";
        }

        private void AddSong(string name, string hymnalNum, string hymnalKey, string key, string subject, string notes)
        {
            string add_query = BuildSongAddQuery(out string[] add_values, name, hymnalNum, hymnalKey, key, subject, notes);
            ServerCommunication.CommandSqlServer(add_query, add_values, serverUserName, serverPassword);
        }
        private string BuildSongAddQuery(out string[] values, string name, string hymnalNum, string hymnalKey, string key, string subject, string notes)
        {
                var elements = new Dictionary<SONG_ATTRIBUTE, string>()
                {
                    {SONG_ATTRIBUTE.hymnalNumber, hymnalNum},
                    {SONG_ATTRIBUTE.hymnalKey, hymnalKey},
                    {SONG_ATTRIBUTE.musicKey, key},
                    {SONG_ATTRIBUTE.subject, subject},
                    {SONG_ATTRIBUTE.tag, notes}
                };
                values = new string[6];
                int valueCount = 0;
                string columnsString = "INSERT INTO " + songInfoTableName + " (" + SONG_ATTRIBUTE.songName;
                string valuesString = "VALUES (@" + valueCount;
                values[valueCount] = name;
                for (SONG_ATTRIBUTE i = 0; i < SONG_ATTRIBUTE.COUNT; i++)
                {
                    if (elements.ContainsKey(i) && elements[i] != "")
                    {
                        columnsString += ", " + i;
                        valueCount++;
                        valuesString += ", @" + valueCount;
                        values[valueCount] = elements[i];
                    }
                }
                values = values[..(valueCount + 1)];
                columnsString += ")";
                valuesString += ")";
                return columnsString + "\n" + valuesString;
        }
    }
}
