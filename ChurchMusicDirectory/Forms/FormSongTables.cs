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
using static ChurchMusicDirectory.DataCtrl;

namespace ChurchMusicDirectory
{
    public partial class FormSongTables : Form
    {
        private FormMain formPassedFromAbove;
        const string serverName = "ChurchMusicServer1";
        const string serverIpAddress = "localhost";
        const int serverPort = 1433;
        private int contextMenuColumnIndex;
        const string contextMenuExclude = "Exclude";
        const string contextMenuClear = "Clear";
        private DataTable songInfoTable;
        private DataTable serviceRecordsTable;
        private const string cellNullString = "";

        public struct TABLE_COLUMN
        {
            public string name;
            public ColumnType columnType;
            public int displayOrder;
            public bool allowFiltering;
            public List<string> filterValues;
            public int width;
            public bool isDerived;
        };
        public static Dictionary<SONG_ATTRIBUTE, TABLE_COLUMN> songInfoColumns = new Dictionary<SONG_ATTRIBUTE, TABLE_COLUMN>()
        {
            {
                SONG_ATTRIBUTE.songName,
                new TABLE_COLUMN
                {
                    name = "Title",
                    columnType = ColumnType.String,
                    displayOrder = 0,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 200,
                    isDerived = false
                }
            },
            {
                SONG_ATTRIBUTE.musicKey,
                new TABLE_COLUMN
                {
                    name = "Key",
                    columnType = ColumnType.String,
                    displayOrder = 0,
                    allowFiltering = true,
                    filterValues = new List<string>(),
                    width = 75,
                    isDerived = false
                }
            },
            {
                SONG_ATTRIBUTE.subject,
                new TABLE_COLUMN
                {
                    name = "Subject",
                    columnType = ColumnType.String,
                    displayOrder = 0,
                    allowFiltering = true,
                    filterValues = new List<string>(),
                    width = 200,
                    isDerived = false
                }
            },
            {
                SONG_ATTRIBUTE.numPlays,
                new TABLE_COLUMN
                {
                    name = "Plays",
                    columnType = ColumnType.Int,
                    displayOrder = 0,
                    allowFiltering = true,
                    filterValues = new List<string>(),
                    width = 75,
                    isDerived = true
                }
            },
            {
                SONG_ATTRIBUTE.tag,
                new TABLE_COLUMN
                {
                    name = "Notes",
                    columnType = ColumnType.String,
                    displayOrder = 0,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 100,
                    isDerived = false
                }
            }
        };

        public static Dictionary<SERVICE_RECORD_ATTRIBUTE, TABLE_COLUMN> serviceRecordColumns = new Dictionary<SERVICE_RECORD_ATTRIBUTE, TABLE_COLUMN>()
        {
            {
                SERVICE_RECORD_ATTRIBUTE.date,
                new TABLE_COLUMN
                {
                    name = "Date",
                    columnType = ColumnType.Date,
                    displayOrder = 0,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 75,
                    isDerived = false
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.title,
                new TABLE_COLUMN
                {
                    name = "Title",
                    columnType = ColumnType.String,
                    displayOrder = 1,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 200,
                    isDerived = false
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.musicKey,
                new TABLE_COLUMN
                {
                    name = "Key",
                    columnType = ColumnType.String,
                    displayOrder = 2,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 75,
                    isDerived = false
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.passage,
                new TABLE_COLUMN
                {
                    name = "Scripture Passage",
                    columnType = ColumnType.String,
                    displayOrder = 3,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 100,
                    isDerived = false
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.elementName,
                new TABLE_COLUMN
                {
                    name = "Type",
                    columnType = ColumnType.String,
                    displayOrder = 4,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 75,
                    isDerived = false
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.serviceNumber,
                new TABLE_COLUMN
                {
                    name = "Service Number",
                    columnType = ColumnType.Int,
                    displayOrder = 5,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 50,
                    isDerived = false
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.orderInService,
                new TABLE_COLUMN
                {
                    name = "Order in Service",
                    columnType = ColumnType.Int,
                    displayOrder = 6,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 50,
                    isDerived = false
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.notes,
                new TABLE_COLUMN
                {
                    name = "Notes",
                    columnType = ColumnType.String,
                    displayOrder = 7,
                    allowFiltering = false,
                    filterValues = new List<string>(),
                    width = 100,
                    isDerived = false
                }
            }
        };

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

        public FormSongTables(FormMain parentForm)
        {
            InitializeComponent();
            InitializeDataGridView1ContextMenu();
            InitializeColumnFilters();
            songInfoTable = new DataTable();
            serviceRecordsTable = new DataTable();
            contextMenuColumnIndex = 0;
            formPassedFromAbove = parentForm;
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
            DataGridView.HitTestInfo menuLocation = Utils.MousePositionInTable(dataGridView1, MousePosition);
            SONG_ATTRIBUTE column = (SONG_ATTRIBUTE)menuLocation.ColumnIndex;
            dataGridView1.ContextMenuStrip.Items.Clear();
            if (column >= 0 && column < SONG_ATTRIBUTE.COUNT)
            {
                if (column == SONG_ATTRIBUTE.numPlays)
                {
                    AddTimeSpanSubmenu(dataGridView1.ContextMenuStrip);
                }
                if (songInfoColumns[column].allowFiltering)
                {
                    SongInfoContextMenuAddFiltering(dataGridView1.ContextMenuStrip, column);
                    contextMenuColumnIndex = (int)column;
                    e.Cancel = false;
                }
            }
        }
        private void SongInfoContextMenuAddFiltering(ContextMenuStrip contextMenu, SONG_ATTRIBUTE columnIndex)
        {
            AddExcludeFilter(contextMenu, (int)columnIndex);
            AddClearFilter(contextMenu);

            for (int filterIndex = 0; filterIndex < songInfoColumns[columnIndex].filterValues.Count; filterIndex++)
            {
                string testValue = songInfoColumns[columnIndex].filterValues[filterIndex].ToString();
                contextMenu.Items.Add(testValue);
                contextMenu.Items[^1].Name = testValue;
                contextMenu.Items[^1].Click += new System.EventHandler(ContextMenuFilterItem_Click);
                if (columnFilters[(int)columnIndex].list.Contains(testValue))
                {
                    ((ToolStripMenuItem)contextMenu.Items[^1]).Checked = true;
                    ((ToolStripMenuItem)contextMenu.Items[^1]).CheckState = CheckState.Checked;
                }
            }
        }
        private void AddTimeSpanSubmenu(ContextMenuStrip contextMenu)
        {
            ToolStripItem[] timespanSubMenuItems = new ToolStripItem[] 
            {
                new ToolStripMenuItem ("This Year"),
                new ToolStripMenuItem ("Last Year"),
                new ToolStripMenuItem ("2 Years Ago"),
                new ToolStripMenuItem ("Last 12 months"),
                new ToolStripMenuItem ("Last 24 months"),
                new ToolStripMenuItem ("Last 36 months"),
            };
            contextMenu.Items.Add("Timespan");
            ((ToolStripMenuItem)contextMenu.Items[0]).DropDownItems.AddRange( timespanSubMenuItems );
            ((ToolStripMenuItem)contextMenu.Items[0]).DropDown.Click += new System.EventHandler(ContextMenuTimespanSubMenu_Click);
        }
        private void ContextMenuTimespanSubMenu_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("you did it!");
        }

        private void AddExcludeFilter(ContextMenuStrip contextMenu, int columnIndex)
        {
            contextMenu.Items.Add(contextMenuExclude);
            contextMenu.Items[^1].Name = contextMenuExclude;
            contextMenu.Items[^1].Click += new System.EventHandler(ContextMenuFilterExclude_Click);
            contextMenu.Items[^1].BackColor = Color.LightSteelBlue;

            if (columnFilters[columnIndex].type == FILTER_TYPE.EXCLUDE)
            {
                ((ToolStripMenuItem)contextMenu.Items[^1]).Checked = true;
                ((ToolStripMenuItem)contextMenu.Items[^1]).CheckState = CheckState.Checked;
            }
            else
            {
                ((ToolStripMenuItem)contextMenu.Items[^1]).Checked = false;
                ((ToolStripMenuItem)contextMenu.Items[^1]).CheckState = CheckState.Unchecked;
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
            contextMenu.Items[^1].Name = contextMenuClear;
            contextMenu.Items[^1].Click += new System.EventHandler(ContextMenuFilterClear_Click);
            contextMenu.Items[^1].BackColor = Color.LightSteelBlue;
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

        public void ImportSongInfoTable(DataTable table)
        {
            songInfoTable = table;
            DisplaySongInfo();
        }
        private void DisplaySongInfo()
        {
            dataGridView1.DataSource = songInfoTable;
            for (int columnIndex = 0; columnIndex < dataGridView1.Columns.Count; columnIndex++)
            {
                dataGridView1.Columns[columnIndex].Width = songInfoColumns[(SONG_ATTRIBUTE)columnIndex].width;
                dataGridView1.Columns[columnIndex].HeaderText = songInfoColumns[(SONG_ATTRIBUTE)columnIndex].name;
                FillFilterList(songInfoColumns[(SONG_ATTRIBUTE)columnIndex], dataGridView1, columnIndex);
            }

            dataGridView1.Columns[^1].MinimumWidth = songInfoColumns[(SONG_ATTRIBUTE)((int)SONG_ATTRIBUTE.COUNT - 1)].width;
            dataGridView1.Columns[^1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }
        static private void FillFilterList(TABLE_COLUMN settings, DataGridView table, int columnIndex)
        {
            if (settings.allowFiltering)
            {
                List<string> columnEntries = new List<string>();
                
                for (int rowIndex = 0; rowIndex < table.RowCount; rowIndex++)
                {
                    if (table.Rows[rowIndex].Cells[columnIndex].Value != null)
                    {
                        string cellValue = table.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                        switch (columnIndex)
                        {
                            case (int)SONG_ATTRIBUTE.musicKey: columnEntries.AddRange(cellValue.Split(",", StringSplitOptions.TrimEntries)); break;
                            default: columnEntries.Add(cellValue); break;
                        }
                    }
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
                        query += Utils.EscapeSpecialCharacters(columnFilters[columnIndex].list[filterIndex]) + "%\'" + joinString;
                    }
                    query += songInfoTable.Columns[columnIndex].ColumnName + excludeString + " LIKE \'%";
                    query += Utils.EscapeSpecialCharacters(columnFilters[columnIndex].list[^1]) + "%\'";
                    query += ")";
                }
                if (!query.Equals("")
                    && columnIndex < (int)SONG_ATTRIBUTE.COUNT - 1 
                    && columnFilters[columnIndex + 1].list.Count > 0)
                {
                    query += " AND ";
                }
            }

            if (!query.Equals(songInfoTable.DefaultView.RowFilter))
            {
                songInfoTable.DefaultView.RowFilter = query;
            }
        }

        private void buttonToggleServicePlanner_Click(object sender, EventArgs e)
        {
            formPassedFromAbove.ToggleServicePlanner();
        }

        /**********************************************************************************************************************
         * ***************************************      Service Records Tab     ***********************************************
         * *******************************************************************************************************************/
        public void ImportServiceRecordsTable(DataTable table)
        {
            serviceRecordsTable = table;
            DisplayServiceRecords();
        }
        private void DisplayServiceRecords()
        {
            dataGridViewServiceRecords.DataSource = serviceRecordsTable;
            
            int lastDisplayedColumnIndex = 0;
            for (int columnIndex = 0; columnIndex < dataGridViewServiceRecords.Columns.Count; columnIndex++)
            {
                SERVICE_RECORD_ATTRIBUTE serviceRecordAttribute = (SERVICE_RECORD_ATTRIBUTE)Enum.Parse(typeof(SERVICE_RECORD_ATTRIBUTE), dataGridViewServiceRecords.Columns[columnIndex].Name);
                //find the service record column with that index (has to be a dict)
                // if exists, format the column. If not, hide it.
                if (serviceRecordColumns.ContainsKey(serviceRecordAttribute))
                {
                    dataGridViewServiceRecords.Columns[columnIndex].Width = serviceRecordColumns[serviceRecordAttribute].width;
                    dataGridViewServiceRecords.Columns[columnIndex].HeaderText = serviceRecordColumns[serviceRecordAttribute].name;
                    dataGridViewServiceRecords.Columns[columnIndex].DisplayIndex = serviceRecordColumns[serviceRecordAttribute].displayOrder;
                    if (serviceRecordColumns[serviceRecordAttribute].displayOrder > lastDisplayedColumnIndex)
                    {
                        lastDisplayedColumnIndex = serviceRecordColumns[serviceRecordAttribute].displayOrder;
                    }
                }
                else
                {
                    dataGridViewServiceRecords.Columns[columnIndex].Visible = false;
                }
                //FillFilterList(serviceRecordColumns[columnIndex], dataGridViewServiceRecords, columnIndex);
            }
            SERVICE_RECORD_ATTRIBUTE lastDisplayedColumn = 0;
            for (SERVICE_RECORD_ATTRIBUTE columnIndex = 0; columnIndex < SERVICE_RECORD_ATTRIBUTE.COUNT; columnIndex++)
            {
                if (serviceRecordColumns[columnIndex].displayOrder > serviceRecordColumns[lastDisplayedColumn].displayOrder)
                {
                    lastDisplayedColumn = columnIndex;
                }
            }
            dataGridViewServiceRecords.Columns[serviceRecordColumns[lastDisplayedColumn].displayOrder].MinimumWidth = serviceRecordColumns[lastDisplayedColumn].width;
            dataGridViewServiceRecords.Columns[serviceRecordColumns[lastDisplayedColumn].displayOrder].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewServiceRecords.Sort(dataGridViewServiceRecords.Columns[(int)SERVICE_RECORD_ATTRIBUTE.date], ListSortDirection.Descending);
        }
    }
}