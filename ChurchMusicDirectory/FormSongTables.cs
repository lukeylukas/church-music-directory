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
    public partial class FormSongTables : Form
    {
        private FormMain formPassedFromAbove;
        const string serverName = "ChurchMusicServer1";
        const string serverIpAddress = "localhost";
        const int serverPort = 1433;
        private int contextMenuColumnIndex;
        const string contextMenuExclude = "Exclude";
        const string contextMenuClear = "Clear";
        private static DataTable songInfoTable;
        private const string cellNullString = "";

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
        private FILTER_INFO[] columnFilters = new FILTER_INFO[(int)DataCtrl.SONG_ATTRIBUTE.COUNT];

        public FormSongTables(FormMain parentForm)
        {
            InitializeComponent();
            InitializeDataGridView1ContextMenu();
            InitializeColumnFilters();
            songInfoTable = new DataTable();
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
            DataCtrl.SONG_ATTRIBUTE column = (DataCtrl.SONG_ATTRIBUTE)menuLocation.ColumnIndex;
            dataGridView1.ContextMenuStrip.Items.Clear();
            if (column >= 0 && column < DataCtrl.SONG_ATTRIBUTE.COUNT)
            {
                if (DataCtrl.songInfoColumns[(int)column].allowFiltering)
                {
                    SongInfoContextMenuPopulate(dataGridView1.ContextMenuStrip, (int)column);
                    contextMenuColumnIndex = (int)column;
                    e.Cancel = false;
                }
            }
        }
        private void SongInfoContextMenuPopulate(ContextMenuStrip contextMenu, int columnIndex)
        {
            AddExcludeFilter(contextMenu, columnIndex);
            AddClearFilter(contextMenu);

            for (int filterIndex = 0; filterIndex < DataCtrl.songInfoColumns[columnIndex].filterValues.Count; filterIndex++)
            {
                string testValue = DataCtrl.songInfoColumns[columnIndex].filterValues[filterIndex].ToString();
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
            for (DataCtrl.SONG_ATTRIBUTE attribute = 0; attribute < DataCtrl.SONG_ATTRIBUTE.COUNT; attribute++)
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
                dataGridView1.Columns[columnIndex].Width = DataCtrl.songInfoColumns[columnIndex].width;
                dataGridView1.Columns[columnIndex].HeaderText = DataCtrl.songInfoColumns[columnIndex].name;
                FillFilterList(DataCtrl.songInfoColumns[columnIndex], dataGridView1, columnIndex);
            }
            dataGridView1.Columns[^1].MinimumWidth = DataCtrl.songInfoColumns[^1].width;
            dataGridView1.Columns[^1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }
        static private void FillFilterList(DataCtrl.TABLE_COLUMN settings, DataGridView table, int columnIndex)
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
                            case (int)DataCtrl.SONG_ATTRIBUTE.musicKey: columnEntries.AddRange(cellValue.Split(",", StringSplitOptions.TrimEntries));   break;
                            default:                                    columnEntries.Add(cellValue);                                                   break;
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
            for (int columnIndex = 0; columnIndex < (int)DataCtrl.SONG_ATTRIBUTE.COUNT; columnIndex++)
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
                        query += EscapeSpecialCharacters(columnFilters[columnIndex].list[filterIndex]) + "%\'" + joinString;
                    }
                    query += songInfoTable.Columns[columnIndex].ColumnName + excludeString + " LIKE \'%";
                    query += EscapeSpecialCharacters(columnFilters[columnIndex].list[^1]) + "%\'";
                    query += ")";
                }
                if (!query.Equals("")
                    && columnIndex < (int)DataCtrl.SONG_ATTRIBUTE.COUNT - 1 
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
        private string EscapeSpecialCharacters(string input)
        {
            string safeString = input;
            char[] specialChars = new char[] { '\'' };
            for (int specialCharIndex = 0; specialCharIndex < specialChars.Length; specialCharIndex++)
            {
                int nextSpecialChar = safeString.IndexOf(specialChars[specialCharIndex]);
                while (nextSpecialChar != -1)
                {
                    if (specialChars[specialCharIndex] == '\'')
                    {
                        safeString = safeString.Insert(nextSpecialChar, "'");
                    }
                    else
                    {
                        safeString = safeString.Insert(nextSpecialChar, @"\");
                    }
                    if (nextSpecialChar == safeString.Length - 2)
                    {
                        break;
                    }
                    nextSpecialChar = safeString.IndexOf(specialChars[specialCharIndex], nextSpecialChar + 2);
                }
            }
            return safeString;
        }

        private void buttonToggleServicePlanner_Click(object sender, EventArgs e)
        {
            formPassedFromAbove.ToggleServicePlanner();
        }
    }
}