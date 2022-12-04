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
using System.Linq;
using Microsoft.IdentityModel.Tokens;

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
            public List<string> filterValues;
            public int width;
        };
        private TABLE_COLUMN[] songInfoColumns = new TABLE_COLUMN[(int)SONG_ATTRIBUTE.COUNT];

        public enum FILTER_TYPE
        {
            INCLUDE,
            EXCLUDE
        }
        public struct FILTER_INFO
        {
            public FILTER_TYPE type;
            public List<string> list;
        };
        private FILTER_INFO[] columnFilters = new FILTER_INFO[(int)SONG_ATTRIBUTE.COUNT];

        public FormMain()
        {
            InitializeComponent();
            InitializeSongInfoSettings();
            InitializeDataGridView1ContextMenu();
            InitializeColumnFilters();
        }
        private void InitializeSongInfoSettings()
        {
            songInfoColumns[(int)SONG_ATTRIBUTE.songName] = new TABLE_COLUMN
            {
                allowFiltering = false,
                filterValues = new List<string>(),
                width = 200
            };
            songInfoColumns[(int)SONG_ATTRIBUTE.musicKey] = new TABLE_COLUMN
            {
                allowFiltering = true,
                filterValues = new List<string>(),
                width = 75
            };
            songInfoColumns[(int)SONG_ATTRIBUTE.subject] = new TABLE_COLUMN
            {
                allowFiltering = true,
                filterValues = new List<string>(),
                width = 200
            };
            songInfoColumns[(int)SONG_ATTRIBUTE.tag] = new TABLE_COLUMN
            {
                allowFiltering = false,
                filterValues = new List<string>(),
                width = 100
            };
            songInfoColumns[(int)SONG_ATTRIBUTE.numPlays] = new TABLE_COLUMN
            {
                allowFiltering = true,
                filterValues = new List<string>(),
                width = 75
            };
            CheckColumnSettingsInitialization();
        }
        private void CheckColumnSettingsInitialization()
        {
            for (SONG_ATTRIBUTE attributeIndex = 0; attributeIndex < SONG_ATTRIBUTE.COUNT; attributeIndex++)
            {
                try
                {
                    bool initCheck = (songInfoColumns[(int)attributeIndex].width != 1);
                }
                catch
                {
                    MessageBox.Show("Settings for " + attributeIndex.ToString() + " not initialized");
                }
            }
        }
        private void InitializeDataGridView1ContextMenu()
        {
            dataGridView1.ContextMenuStrip = new ContextMenuStrip();
            dataGridView1.ContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(DataGridViewContextMenuOpen);
            dataGridView1.ContextMenuStrip.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(ContextMenuStrip_Closing);
            dataGridView1.ContextMenuStrip.ShowCheckMargin = true;
        }
        void DataGridViewContextMenuOpen(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            DataGridView.HitTestInfo menuLocation = MousePositionInTable();
            SONG_ATTRIBUTE column = (SONG_ATTRIBUTE)menuLocation.ColumnIndex;
            dataGridView1.ContextMenuStrip.Items.Clear();
            if (column >= 0 && column < SONG_ATTRIBUTE.COUNT)
            {
                if (songInfoColumns[(int)column].allowFiltering)
                {
                    SongInfoContextMenuPopulate(dataGridView1.ContextMenuStrip, songInfoColumns[(int)column].filterValues);
                    e.Cancel = false;
                }
            }
        }
        private DataGridView.HitTestInfo MousePositionInTable()
        {
            Point clickLocationInTable = dataGridView1.PointToClient(MousePosition);
            DataGridView.HitTestInfo cellInfo = dataGridView1.HitTest(clickLocationInTable.X, clickLocationInTable.Y);
            return cellInfo;
        }
        private void SongInfoContextMenuPopulate(ContextMenuStrip contextMenu, List<string> filterValues)
        {
            for (int filterIndex = 0; filterIndex < filterValues.Count; filterIndex++)
            {
                contextMenu.Items.Add(filterValues[filterIndex].ToString());
                contextMenu.Items[contextMenu.Items.Count - 1].Name = filterValues[filterIndex].ToString();
                contextMenu.Items[contextMenu.Items.Count - 1].Click += new System.EventHandler(ContextMenuFilterItem_Click);
            }
        }
        private void ContextMenuFilterItem_Click(object? sender, EventArgs e)
        {
            DataGridView.HitTestInfo menuLocation = MousePositionInTable();
            AddValueToFilter(sender.ToString(), menuLocation.ColumnIndex);
            ToolStripItem[] senderItem = dataGridView1.ContextMenuStrip.Items.Find(sender.ToString(), true);
            foreach (ToolStripMenuItem item in senderItem)
            {
                item.Checked = true;
                item.CheckState = System.Windows.Forms.CheckState.Checked;
            }
            
        }
        private void AddValueToFilter(string filterValue, int columnIndex)
        {
            if (columnFilters[columnIndex].list.Contains(filterValue))
            {
                columnFilters[columnIndex].list.Remove(filterValue);
            }
            else
            {
                columnFilters[columnIndex].list.Add(filterValue);
            }
        }
        private void ContextMenuStrip_Closing(object? sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true;
            }
        }

        private void InitializeColumnFilters()
        {
            for (SONG_ATTRIBUTE attribute = 0; attribute < SONG_ATTRIBUTE.COUNT; attribute++)
            {
                columnFilters[(int)attribute].type = FILTER_TYPE.INCLUDE;
                columnFilters[(int)attribute].list = new List<string>();
            }
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
                            if (reader.FieldCount == (int)SONG_ATTRIBUTE.COUNT)
                            {
                                formPassedFromAbove.Invoke(new Action(() => ReadData(formPassedFromAbove, reader)));
                            }
                            else
                            {
                                MessageBox.Show("Server sent incorrect data size.");
                            }
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
            DataGridView table = formPassedFromAbove.dataGridView1;
            table.Rows.Clear();
            table.ColumnCount = reader.FieldCount;
            while (reader.Read())
            {
                object[] row = new object[reader.FieldCount];
                reader.GetProviderSpecificValues(row);
                table.Rows.Add(row);
            }
            for (int columnIndex = 0; columnIndex < reader.FieldCount; columnIndex++)
            {
                table.Columns[columnIndex].Width = formPassedFromAbove.songInfoColumns[columnIndex].width;
                table.Columns[columnIndex].Name = reader.GetName(columnIndex);
                FillFilterList(formPassedFromAbove.songInfoColumns[columnIndex], table, columnIndex);
            }
            table.Sort(table.Columns[0], ListSortDirection.Ascending);
        }
        static private void FillFilterList(TABLE_COLUMN settings, DataGridView table, int columnIndex)
        {
            if (settings.allowFiltering)
            {
                List<string> columnEntries = new List<string>();
                
                for (int rowIndex = 0; rowIndex < table.RowCount; rowIndex++)
                {
                    try
                    {
                        string cellValue = table.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                        if (columnIndex == (int)SONG_ATTRIBUTE.musicKey)
                        {
                            string[] cellValues = cellValue.Split(",", StringSplitOptions.TrimEntries);
                            if (!cellValues.Contains("Null"))
                            {
                                columnEntries.AddRange(cellValues);
                            }
                        }
                        else
                        {
                            if (!cellValue.Contains("Null"))
                            {
                                columnEntries.Add(cellValue);
                            }
                        }
                    }
                    catch { /* Do nothing */ }
                }

                columnEntries.Sort();

                for (int rowIndex = 0; rowIndex < columnEntries.Count; rowIndex++)
                {
                    AddUniqueValueToFilter(columnEntries[rowIndex], settings.filterValues);
                }
            }
        }
        static private void AddUniqueValueToFilter(object value, List<string> filterList)
        {
            if (value.ToString() != "Null"
                && (filterList.IsNullOrEmpty()
                    || filterList.Last() != value.ToString()))
            {
                filterList.Add(value.ToString());
            }
        }
    }
}