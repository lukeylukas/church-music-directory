using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChurchMusicDirectory
{
    public partial class FormServicePlanner : Form
    {
        private string tempComboBoxValue = "";

        const string columnElementName = "ColumnElement";
        const string columnTitleName = "ColumnTitle";
        const string columnMusicKeyName = "ColumnMusicKey";
        const string columnPassageName = "ColumnPassage";
        const string columnNotesName = "ColumnNotes";

        string[]? worshipElements;
        string[]? songTitles;
        string[]? musicKeys;
        Dictionary<int, SERVICE_PLANNER_COLUMN> plannerColumns;
        Dictionary<string, int> inversePlannerColumns;

        class SERVICE_PLANNER_COLUMN
        {
            public string name { get; set; }
            public int width { get; set; }
            public string[] dataSource { get; set; }
        };

        public FormServicePlanner(DataCtrl dataCtrlInstance)
        {
            InitializeComponent();
            Setup(dataCtrlInstance);
        }
        private void Setup(DataCtrl dataCtrlInstance)
        {
            worshipElements = dataCtrlInstance.worshipElements.ToArray();
            songTitles = dataCtrlInstance.titlesList.ToArray();
            musicKeys = dataCtrlInstance.musicKeys.ToArray();
            InitializeDataGridView();

            SelectDefaultServiceDate();
            monthCalendarDatePicker.BringToFront();
        }
        private void InitializeDataGridView()
        {
            plannerColumns = new Dictionary<int, SERVICE_PLANNER_COLUMN>()
            {
                { 0, new SERVICE_PLANNER_COLUMN { name=columnElementName,     width=100, dataSource = worshipElements } },
                { 1, new SERVICE_PLANNER_COLUMN { name=columnTitleName,       width=200, dataSource = songTitles } },
                { 2, new SERVICE_PLANNER_COLUMN { name=columnMusicKeyName,    width=75,  dataSource = musicKeys } },
                { 3, new SERVICE_PLANNER_COLUMN { name=columnPassageName,     width=150, dataSource = (new string[] { "" }) } },
                { 4, new SERVICE_PLANNER_COLUMN { name=columnNotesName,       width=100, dataSource = (new string[] { "" }) } },
            };
            inversePlannerColumns = new Dictionary<string, int>();
            for (int columnIndex = 0; columnIndex < plannerColumns.Count; columnIndex++)
            {
                inversePlannerColumns.Add(plannerColumns[columnIndex].name, columnIndex);

                DataGridViewColumn column = dataGridViewServicePlanner.Columns[columnIndex];
                column.Name = plannerColumns[columnIndex].name;
                column.MinimumWidth = plannerColumns[columnIndex].width;
                column.Width = plannerColumns[columnIndex].width;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                if (column.CellType == (new DataGridViewComboBoxCell()).GetType())
                {
                    ((DataGridViewComboBoxColumn)column).DataSource = plannerColumns[columnIndex].dataSource;
                }
            }
            dataGridViewServicePlanner.Columns[plannerColumns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewServicePlanner.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewServicePlanner.DataError += new DataGridViewDataErrorEventHandler(dataGridViewServicePlanner_DataError);
            dataGridViewServicePlanner.CellEndEdit += new DataGridViewCellEventHandler(dataGridViewServicePlanner_CellEndEdit);
        }

        private void SelectDefaultServiceDate()
        {
            int addWeeks = 0;
            double days = (DayOfWeek.Sunday - DateTime.Now.DayOfWeek) % 7;
            if (days < 0)
            {
                days += 7;
            }
            DateTime selectedDate = DateTime.Now.AddDays(days + addWeeks * 7);
            monthCalendarDatePicker.SelectionRange = new SelectionRange(selectedDate, selectedDate);
            SyncWithCalendar();
        }

        private void buttonCalendar_Click(object sender, EventArgs e)
        {
            monthCalendarDatePicker.Visible = !monthCalendarDatePicker.Visible;
        }

        private void monthCalendarDatePicker_DateSelected(object sender, DateRangeEventArgs e)
        {
            SyncWithCalendar();
            monthCalendarDatePicker.Visible = false;
        }
        private void SyncWithCalendar()
        {
            comboBoxServiceDate.Text = monthCalendarDatePicker.SelectionStart.ToLongDateString();
        }

        private void dataGridViewServicePlanner_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var comboEditor = e.Control as DataGridViewComboBoxEditingControl;
            if (comboEditor != null)
            {
                if (ElementIsMusical() ||
                    (dataGridViewServicePlanner.CurrentCell.ColumnIndex == inversePlannerColumns[columnElementName]))
                {
                    comboEditor.DropDownStyle = ComboBoxStyle.DropDown;
                    comboEditor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboEditor.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    comboEditor.AutoCompleteCustomSource = GetDataCollection();
                }
                else
                {
                    comboEditor.DropDownStyle = ComboBoxStyle.Simple;
                    comboEditor.AutoCompleteMode = AutoCompleteMode.None;
                }
                comboEditor.Leave += new EventHandler(ComboBoxEditingControl_Leave); 

            }
            e.CellStyle.BackColor = dataGridViewServicePlanner.DefaultCellStyle.BackColor;
        }

        private AutoCompleteStringCollection GetDataCollection()
        {
            AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
            switch (dataGridViewServicePlanner.CurrentCell.OwningColumn.Name)
            {
                case columnElementName:   DataCollection.AddRange(worshipElements); break;
                case columnTitleName:     DataCollection.AddRange(songTitles);      break;
                case columnMusicKeyName:  DataCollection.AddRange(musicKeys);       break;
                default:                                                            break;
            }
            return DataCollection;
        }
        private bool ElementIsMusical()
        {
            bool isMusical = false;
            if (dataGridViewServicePlanner.Rows[dataGridViewServicePlanner.CurrentCell.RowIndex].Cells[0].Value != null)
            {
                int elementColumnIndex = inversePlannerColumns[columnElementName];
                string element = dataGridViewServicePlanner.Rows[dataGridViewServicePlanner.CurrentCell.RowIndex].Cells[elementColumnIndex].Value.ToString();
                if (element == "Song" || element == "Offering" || element == "Communion")
                {
                    isMusical = true;
                }
            }
            return isMusical;
        }

        private void dataGridViewServicePlanner_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewComboBoxCell cell = dataGridViewServicePlanner.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
            if (cell != null)
            {
                cell.Value = tempComboBoxValue;

                if (cell.OwningColumn.Name == columnElementName)
                {
                    SetupRowBasedOnWorshipElement(cell.RowIndex);
                }
                else if (ElementIsMusical() == false)
                {
                    cell.DataSource = new string[] { tempComboBoxValue };
                }
            }
        }
        private void SetupRowBasedOnWorshipElement(int rowIndex)
        {
            bool elementIsMusical = ElementIsMusical();
            int titleColumnIndex = inversePlannerColumns[columnTitleName];
            SetTitleComboBoxDataSource(dataGridViewServicePlanner.Rows[rowIndex].Cells[titleColumnIndex], elementIsMusical);
            int musicKeyColumnIndex = inversePlannerColumns[columnMusicKeyName];
            dataGridViewServicePlanner.Rows[rowIndex].Cells[musicKeyColumnIndex].ReadOnly = !elementIsMusical; //enables musicKey column
        }
        private void SetTitleComboBoxDataSource(DataGridViewCell titleCell, bool elementIsMusical)
        {
            DataGridViewComboBoxCell cell = titleCell as DataGridViewComboBoxCell;
            if (cell != null)
            {
                if (elementIsMusical)
                {
                    cell.DataSource = songTitles;
                }
                else
                {
                    cell.DataSource = new string[] { "" };
                }
            }
        }

        private void ComboBoxEditingControl_Leave(object sender, EventArgs e)
        {
            if (dataGridViewServicePlanner.EditingControl.Text != null)
            {
                tempComboBoxValue = dataGridViewServicePlanner.EditingControl.Text;
            }
            else
            {
                tempComboBoxValue = "";
            }
        }

        void dataGridViewServicePlanner_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // (No need to write anything in here)
        }
    }
}
