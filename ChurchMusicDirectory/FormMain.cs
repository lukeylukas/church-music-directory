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
        private int contextMenuColumnIndex;
        const string contextMenuExclude = "Exclude";
        const string contextMenuClear = "Clear";
        private static DataTable songInfoTable;
        private const string cellNullString = "";
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
            songInfoTable = new DataTable();
            contextMenuColumnIndex = 0;
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
                    SongInfoContextMenuPopulate(dataGridView1.ContextMenuStrip, (int)column);
                    contextMenuColumnIndex = (int)column;
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
        private void SongInfoContextMenuPopulate(ContextMenuStrip contextMenu, int columnIndex)
        {
            AddExcludeFilter(contextMenu, columnIndex);
            AddClearFilter(contextMenu);

            for (int filterIndex = 0; filterIndex < songInfoColumns[columnIndex].filterValues.Count; filterIndex++)
            {
                string testValue = songInfoColumns[columnIndex].filterValues[filterIndex].ToString();
                contextMenu.Items.Add(testValue);
                contextMenu.Items[^1].Name = testValue;
                contextMenu.Items[^1].Click += new System.EventHandler(ContextMenuFilterItem_Click);
                if (columnFilters[columnIndex].list.Contains(testValue))
                {
                    ((ToolStripMenuItem)contextMenu.Items[^1]).Checked = true;
                    ((ToolStripMenuItem)contextMenu.Items[^1]).CheckState = CheckState.Checked;
                }
            }
        }
        private void AddExcludeFilter(ContextMenuStrip contextMenu, int columnIndex)
        {
            contextMenu.Items.Add(contextMenuExclude);
            contextMenu.Items[0].Name = contextMenuExclude;
            contextMenu.Items[0].Click += new System.EventHandler(ContextMenuFilterExclude_Click);
            contextMenu.Items[0].BackColor = Color.LightSteelBlue;

            if (columnFilters[columnIndex].type == FILTER_TYPE.EXCLUDE)
            {
                ((ToolStripMenuItem)contextMenu.Items[0]).Checked = true;
                ((ToolStripMenuItem)contextMenu.Items[0]).CheckState = CheckState.Checked;
            }
            else
            {
                ((ToolStripMenuItem)contextMenu.Items[0]).Checked = false;
                ((ToolStripMenuItem)contextMenu.Items[0]).CheckState = CheckState.Unchecked;
            }
        }
        private void ContextMenuFilterExclude_Click(object? sender, EventArgs e)
        {
            ToolStripMenuItem senderItem = (ToolStripMenuItem)sender;
            if (senderItem.Checked)
            {
                senderItem.Checked = false;
                senderItem.CheckState = CheckState.Unchecked;

            }
            else
            {
                senderItem.Checked = true;
                senderItem.CheckState = CheckState.Checked;
            }

            FilterHandleExclude(senderItem.Checked, contextMenuColumnIndex);
        }
        private void FilterHandleExclude(bool excludeFilterValues, int columnIndex)
        {
            if (excludeFilterValues)
            {
                columnFilters[columnIndex].type = FILTER_TYPE.EXCLUDE;
            }
            else
            {
                columnFilters[columnIndex].type = FILTER_TYPE.INCLUDE;
            }
        }
        private void AddClearFilter(ContextMenuStrip contextMenu)
        {
            contextMenu.Items.Add(contextMenuClear);
            contextMenu.Items[1].Name = contextMenuClear;
            contextMenu.Items[1].Click += new System.EventHandler(ContextMenuFilterClear_Click);
            contextMenu.Items[1].BackColor = Color.LightSteelBlue;
        }
        private void ContextMenuFilterClear_Click(object? sender, EventArgs e)
        {
            columnFilters[contextMenuColumnIndex].list.Clear();

            ToolStrip parentMenu = ((ToolStripMenuItem)sender).Owner;
            for (int menuItemIndex = 0; menuItemIndex < parentMenu.Items.Count; menuItemIndex++)
            {
                ((ToolStripMenuItem)parentMenu.Items[menuItemIndex]).Checked = false;
                ((ToolStripMenuItem)parentMenu.Items[menuItemIndex]).CheckState = CheckState.Unchecked;
            }
            SongInfoFilter();
        }
        private void ContextMenuFilterItem_Click(object? sender, EventArgs e)
        {
            ToolStripMenuItem senderItem = (ToolStripMenuItem)sender;
            if (senderItem.Checked)
            {
                senderItem.Checked = false;
                senderItem.CheckState = CheckState.Unchecked;

            }
            else
            {
                senderItem.Checked = true;
                senderItem.CheckState = CheckState.Checked;
            }

            AddValueToFilter(sender.ToString(), contextMenuColumnIndex);
            
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
            else
            {
                SongInfoFilter();
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
            DataTable table = songInfoTable;
            table.Load(reader);

            DataGridView tableViewer = formPassedFromAbove.dataGridView1;
            tableViewer.DataSource = table;
            for (int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
            {
                tableViewer.Columns[columnIndex].Width = formPassedFromAbove.songInfoColumns[columnIndex].width;
                tableViewer.Columns[columnIndex].Name = table.Columns[columnIndex].ColumnName;
                FillFilterList(formPassedFromAbove.songInfoColumns[columnIndex], tableViewer, columnIndex);
            }
            tableViewer.Sort(tableViewer.Columns[0], ListSortDirection.Ascending);
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
                        switch (columnIndex)
                        {
                            case (int)SONG_ATTRIBUTE.musicKey:  columnEntries.AddRange(cellValue.Split(",", StringSplitOptions.TrimEntries));   break;
                            default:                            columnEntries.Add(cellValue);                                                   break;
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
            if (value.ToString() != cellNullString
                && (filterList.IsNullOrEmpty()
                    || filterList.Last() != value.ToString()))
            {
                filterList.Add(value.ToString());
            }
        }

        private void SongInfoFilter()
        {
            string query = "";
            for (int columnIndex = 0; columnIndex < (int)SONG_ATTRIBUTE.COUNT; columnIndex++)
            {

                if (columnFilters[columnIndex].list.Count > 0)
                {
                    string excludeString = "";
                    string joinString = " OR ";
                    if (columnFilters[columnIndex].type == FILTER_TYPE.EXCLUDE)
                    {
                        excludeString = " NOT";
                        joinString = " AND ";
                    }
                    query += "(";

                    for (int filterIndex = 0; filterIndex <= columnFilters[columnIndex].list.Count - 2; filterIndex++)
                    {
                        query += songInfoTable.Columns[columnIndex].ColumnName + excludeString + " LIKE \'%";
                        query += columnFilters[columnIndex].list[filterIndex] + "%\'" + joinString;
                    }
                    query += songInfoTable.Columns[columnIndex].ColumnName + excludeString + " LIKE \'%";
                    query += columnFilters[columnIndex].list[^1] + "%\'";
                    query += ")";
                }
                if (!query.Equals("")
                    && columnIndex < (int)SONG_ATTRIBUTE.COUNT - 1 
                    && columnFilters[columnIndex + 1].list.Count > 0)
                {
                    query += " AND ";
                }
            }

            if (!query.Equals(""))
            {
                songInfoTable.DefaultView.RowFilter = query;
            }
        }
    }
}