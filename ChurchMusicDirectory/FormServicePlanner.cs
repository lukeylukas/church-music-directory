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
        string[]? worshipElements;
        string[]? songTitles;
        string[]? musicKeys;
        List<DateTime> serviceDatesList;
        Dictionary<SERVICE_PLANNER_COLUMN_ID, SERVICE_PLANNER_COLUMN> plannerColumns;

        enum SERVICE_PLANNER_COLUMN_ID
        {
            Element,
            Title,
            Key,
            Passage,
            Notes,
        }

        class SERVICE_PLANNER_COLUMN
        {
            public string name { get; set; }
            public Type type { get; set; }
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
            worshipElements = new string[dataCtrlInstance.worshipElements.Count + 1];
            worshipElements[0] = "";
            dataCtrlInstance.worshipElements.ToArray().CopyTo(worshipElements, 1);

            songTitles = new string[dataCtrlInstance.titlesList.Count + 1];
            songTitles[0] = "";
            dataCtrlInstance.titlesList.ToArray().CopyTo(songTitles, 1);

            musicKeys = new string[dataCtrlInstance.musicKeys.Count + 1];
            musicKeys[0] = "";
            dataCtrlInstance.musicKeys.ToArray().CopyTo(musicKeys, 1);

            InitializeDataGridView();

            calendarDatePicker.BringToFront();

            InitializeServiceDates(dataCtrlInstance);
        }
        private void InitializeDataGridView()
        {
            plannerColumns = new Dictionary<SERVICE_PLANNER_COLUMN_ID, SERVICE_PLANNER_COLUMN>()
            {
                {
                    SERVICE_PLANNER_COLUMN_ID.Element,
                    new SERVICE_PLANNER_COLUMN
                    {
                        name=SERVICE_PLANNER_COLUMN_ID.Element.ToString(),
                        type=(new DataGridViewComboBoxColumn()).GetType(),
                        width=100,
                        dataSource = worshipElements
                    }
                },
                {
                    SERVICE_PLANNER_COLUMN_ID.Title,
                    new SERVICE_PLANNER_COLUMN
                    {
                        name=SERVICE_PLANNER_COLUMN_ID.Title.ToString(),
                        type=(new DataGridViewComboBoxColumn()).GetType(),
                        width=200,
                        dataSource = songTitles
                    }
                },
                {
                    SERVICE_PLANNER_COLUMN_ID.Key,
                    new SERVICE_PLANNER_COLUMN
                    {
                        name=SERVICE_PLANNER_COLUMN_ID.Key.ToString(),
                        type=(new DataGridViewComboBoxColumn()).GetType(),
                        width=75,
                        dataSource = musicKeys
                    }
                },
                {
                    SERVICE_PLANNER_COLUMN_ID.Passage,
                    new SERVICE_PLANNER_COLUMN
                    {
                        name=SERVICE_PLANNER_COLUMN_ID.Passage.ToString(),
                        type=(new DataGridViewTextBoxColumn()).GetType(),
                        width=150,
                        dataSource = (new string[] { "" })
                    }
                },
                {
                    SERVICE_PLANNER_COLUMN_ID.Notes,
                    new SERVICE_PLANNER_COLUMN
                    {
                        name=SERVICE_PLANNER_COLUMN_ID.Notes.ToString(),
                        type=(new DataGridViewTextBoxColumn()).GetType(),
                        width=100,
                        dataSource = (new string[] { "" })
                    }
                },
            };

            for (int columnIndex = 0; columnIndex < plannerColumns.Count; columnIndex++)
            {
                InitializeServicePlannerColumn(plannerColumns[(SERVICE_PLANNER_COLUMN_ID)columnIndex]);
            }

            dataGridViewServicePlanner.Columns[plannerColumns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewServicePlanner.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewServicePlanner.DataError += new DataGridViewDataErrorEventHandler(dataGridViewServicePlanner_DataError);
            dataGridViewServicePlanner.CellEndEdit += new DataGridViewCellEventHandler(dataGridViewServicePlanner_CellEndEdit);
        }
        private void InitializeServicePlannerColumn(SERVICE_PLANNER_COLUMN columnInfo)
        {
            DataGridViewColumn column;
            if (columnInfo.type == new DataGridViewTextBoxColumn().GetType())
            {
                column = new DataGridViewTextBoxColumn();
            }
            else if (columnInfo.type == new DataGridViewComboBoxColumn().GetType())
            {
                column = new DataGridViewComboBoxColumn();
                ((DataGridViewComboBoxColumn)column).DataSource = columnInfo.dataSource;
            }
            else
            {
                MessageBox.Show("service planner column is unsupported type");
                return; //TODO: throw actual error instead
            }

            column.Name = columnInfo.name;
            column.HeaderText = columnInfo.name;
            column.MinimumWidth = columnInfo.width;
            column.Width = columnInfo.width;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewServicePlanner.Columns.Add(column);
        }

        private void InitializeServiceDates(DataCtrl dataCtrlInstance)
        {
            serviceDatesList = dataCtrlInstance.GetServiceDatesList();
            comboBoxServiceDate.DataSource = Array.ConvertAll(serviceDatesList.ToArray(), x => x.ToLongDateString());
            DateTime defaultDate = GetDefaultServiceDate();
            calendarDatePicker.SelectionRange = new SelectionRange(defaultDate, defaultDate);
        }
        private DateTime GetDefaultServiceDate()
        {
            int addWeeks = 0;
            double days = (DayOfWeek.Sunday - DateTime.Now.DayOfWeek) % 7;
            if (days < 0)
            {
                days += 7;
            }
            DateTime defaultDate = DateTime.Now.AddDays(days + addWeeks * 7);
            return defaultDate;
        }

        private void buttonCalendar_Click(object sender, EventArgs e)
        {
            calendarDatePicker.Visible = !calendarDatePicker.Visible;
        }

        private void calendarDatePicker_DateSelected(object sender, DateRangeEventArgs e)
        {
            SyncWithCalendar();
            calendarDatePicker.Visible = false;
        }
        private void SyncWithCalendar()
        {
            comboBoxServiceDate.Text = calendarDatePicker.SelectionStart.ToLongDateString();
            // every time, add date to combobox, refresh the combobox data, select the date in combobox
            // figure out how to ensure only populated dates stay in combobox
        }

        private void dataGridViewServicePlanner_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var comboEditor = e.Control as DataGridViewComboBoxEditingControl;
            if (comboEditor != null)
            {
                if (ElementIsMusical(dataGridViewServicePlanner.CurrentCell.RowIndex)
                    || (dataGridViewServicePlanner.CurrentCell.ColumnIndex == (int)SERVICE_PLANNER_COLUMN_ID.Element))
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
            switch (dataGridViewServicePlanner.CurrentCell.ColumnIndex)
            {
                case (int)SERVICE_PLANNER_COLUMN_ID.Element:   DataCollection.AddRange(worshipElements); break;
                case (int)SERVICE_PLANNER_COLUMN_ID.Title:     DataCollection.AddRange(songTitles);      break;
                case (int)SERVICE_PLANNER_COLUMN_ID.Key:       DataCollection.AddRange(musicKeys);       break;
                default:                                                                                 break;
            }
            return DataCollection;
        }
        private bool ElementIsMusical(int rowIndex)
        {
            bool isMusical = false;
            if (rowIndex <= dataGridViewServicePlanner.RowCount)
            {
                if (dataGridViewServicePlanner.Rows[dataGridViewServicePlanner.CurrentCell.RowIndex].Cells[0].Value != null)
                {
                    int elementColumnIndex = (int)SERVICE_PLANNER_COLUMN_ID.Element;
                    string element = dataGridViewServicePlanner.Rows[rowIndex].Cells[elementColumnIndex].Value.ToString();
                    if (element == "Song" 
                        || element == "Offering" 
                        || element == "Communion")
                    {
                        isMusical = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Row index invalid");
                //TODO: throw error
            }
            return isMusical;
        }

        private void dataGridViewServicePlanner_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewComboBoxCell cell = dataGridViewServicePlanner.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
            if (cell != null)
            {
                cell.Value = tempComboBoxValue;

                if (cell.OwningColumn.Name == SERVICE_PLANNER_COLUMN_ID.Element.ToString())
                {
                    SetupRowBasedOnWorshipElement(cell.RowIndex);
                }
                else if (!ElementIsMusical(cell.RowIndex))
                {
                    cell.DataSource = new string[] { tempComboBoxValue };
                }
            }
        }
        private void SetupRowBasedOnWorshipElement(int rowIndex)
        {
            bool elementIsMusical = ElementIsMusical(rowIndex);
            int titleColumnIndex = (int)SERVICE_PLANNER_COLUMN_ID.Title;
            SetTitleComboBoxDataSource(dataGridViewServicePlanner.Rows[rowIndex].Cells[titleColumnIndex], elementIsMusical);
            int musicKeyColumnIndex = (int)SERVICE_PLANNER_COLUMN_ID.Key;
            dataGridViewServicePlanner.Rows[rowIndex].Cells[musicKeyColumnIndex].ReadOnly = !elementIsMusical;
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

        private void buttonDiscardChanges_Click(object sender, EventArgs e)
        {
            //update the dataGridView with the original data; discard current state of dataGridView
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            //save dataGridView state to table for that day
        }

        private void FormServicePlanner_Shown(object sender, EventArgs e)
        {
            SyncWithCalendar();
        }

        private void buttonAddRow_Click(object sender, EventArgs e)
        {
            dataGridViewServicePlanner.Rows.Add();
        }
    }
}
