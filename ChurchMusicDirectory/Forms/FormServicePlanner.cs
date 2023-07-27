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
        Dictionary<SERVICE_RECORD_ATTRIBUTE, SERVICE_PLANNER_COLUMN> plannerColumns;
        static private DataCtrl dataCtrlInstance = new DataCtrl();

        class SERVICE_PLANNER_COLUMN
        {
            public string name { get; set; }
            public bool visible { get; set; }
            public int displayOrder { get; set; }
            public Type type { get; set; }
            public int width { get; set; }
            public string[]? dataSource { get; set; }
        };

        public FormServicePlanner(DataCtrl dataCtrlPassedIn)
        {
            dataCtrlInstance = dataCtrlPassedIn;
            InitializeComponent();
            Setup();
        }
        private void Setup()
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

            InitializeServiceDates();
        }
        private void InitializeDataGridView()
        {
            plannerColumns = new Dictionary<SERVICE_RECORD_ATTRIBUTE, SERVICE_PLANNER_COLUMN>()
        {
            {
                SERVICE_RECORD_ATTRIBUTE.elementName,
                new SERVICE_PLANNER_COLUMN
                {
                    name="Element",
                    visible=true,
                    displayOrder=1,
                    type=(new DataGridViewComboBoxColumn()).GetType(),
                    width=100,
                        dataSource = worshipElements
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.title,
                new SERVICE_PLANNER_COLUMN
                {
                    name="Title",
                    visible=true,
                    displayOrder=2,
                    type=(new DataGridViewComboBoxColumn()).GetType(),
                    width=200,
                        dataSource = songTitles
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.musicKey,
                new SERVICE_PLANNER_COLUMN
                {
                    name="Key",
                    visible=true,
                    displayOrder=3,
                    type=(new DataGridViewComboBoxColumn()).GetType(),
                    width=75,
                        dataSource = musicKeys
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.passage,
                new SERVICE_PLANNER_COLUMN
                {
                    name="Passage",
                    visible=true,
                    displayOrder=4,
                    type=(new DataGridViewTextBoxColumn()).GetType(),
                    width=150,
                    dataSource = null
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.notes,
                new SERVICE_PLANNER_COLUMN
                {
                    name="Notes",
                    visible=true,
                    displayOrder=5,
                    type=(new DataGridViewTextBoxColumn()).GetType(),
                    width=100,
                    dataSource = null
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.date,
                new SERVICE_PLANNER_COLUMN
                {
                    name="Date",
                    visible=false,
                    displayOrder=0,
                    type=(new DataGridViewComboBoxColumn()).GetType(),
                    width=100,
                    dataSource = null
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.serviceNumber,
                new SERVICE_PLANNER_COLUMN
                {
                    name="ServiceNumber",
                    visible=false,
                    displayOrder=6,
                    type=(new DataGridViewComboBoxColumn()).GetType(),
                    width=100,
                    dataSource = null
                }
            },
            {
                SERVICE_RECORD_ATTRIBUTE.orderInService,
                new SERVICE_PLANNER_COLUMN
                {
                    name="Order",
                    visible=false,
                    displayOrder=7,
                    type=(new DataGridViewComboBoxColumn()).GetType(),
                    width=100,
                    dataSource = null
                }
            }
        };

            for (int columnIndex = 0; columnIndex < plannerColumns.Count; columnIndex++)
            {
                InitializeServicePlannerColumn(plannerColumns[(SERVICE_RECORD_ATTRIBUTE)columnIndex]);
                dataGridViewServicePlanner.Columns[columnIndex].Visible = plannerColumns[(SERVICE_RECORD_ATTRIBUTE)columnIndex].visible;
                dataGridViewServicePlanner.Columns[columnIndex].DataPropertyName = ((SERVICE_RECORD_ATTRIBUTE)columnIndex).ToString();
            }
            // separate loop for setting display order because it needs to be done after all columns are added
            for (int columnIndex = 0; columnIndex < plannerColumns.Count; columnIndex++)
            {
                dataGridViewServicePlanner.Columns[columnIndex].DisplayIndex = plannerColumns[(SERVICE_RECORD_ATTRIBUTE)columnIndex].displayOrder;
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
                if (columnInfo.dataSource != null)
                {
                    ((DataGridViewComboBoxColumn)column).DataSource = columnInfo.dataSource;
                }
            }
            else
            {
                column = new DataGridViewTextBoxColumn(); //this is for all the non-visible columns. They still need to be added to the dataGridView
            }

            column.Name = columnInfo.name;
            column.HeaderText = columnInfo.name;
            column.MinimumWidth = columnInfo.width;
            column.Width = columnInfo.width;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewServicePlanner.Columns.Add(column);
        }

        private void InitializeServiceDates()
        {
            serviceDatesList = dataCtrlInstance.GetServiceDatesList();
            comboBoxServiceDate.DataSource = Array.ConvertAll(serviceDatesList.ToArray(), x => x.ToLongDateString());
            //DateTime defaultDate = GetDefaultServiceDate();
            //calendarDatePicker.SetSelectionRange(defaultDate, defaultDate);
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
            FormatDataGridView();
        }
        private void SyncWithCalendar()
        {
            comboBoxServiceDate.Text = calendarDatePicker.SelectionStart.ToLongDateString();
            // every time, add date to combobox, refresh the combobox data, select the date in combobox
            // figure out how to ensure only populated dates stay in combobox
        }
        private void FormatDataGridView()
        {
            dataGridViewServicePlanner.DataSource = dataCtrlInstance.GetServiceInfo(calendarDatePicker.SelectionStart, 0);
            dataGridViewServicePlanner.Sort(dataGridViewServicePlanner.Columns[(int)SERVICE_RECORD_ATTRIBUTE.orderInService], ListSortDirection.Ascending);
        }

        private void FormServicePlanner_Shown(object sender, EventArgs e)
        {
            SyncWithCalendar();
        }

        private void comboBoxServiceDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            calendarDatePicker.SetSelectionRange(DateTime.Parse(comboBoxServiceDate.Text), DateTime.Parse(comboBoxServiceDate.Text));
            FormatDataGridView();
        }

        private void dataGridViewServicePlanner_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var comboEditor = e.Control as DataGridViewComboBoxEditingControl;
            if (comboEditor != null)
            {
                if (ElementIsMusical(dataGridViewServicePlanner.CurrentCell.RowIndex)
                    || (dataGridViewServicePlanner.CurrentCell.ColumnIndex == (int)SERVICE_RECORD_ATTRIBUTE.elementName))
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
            if (plannerColumns[(SERVICE_RECORD_ATTRIBUTE)dataGridViewServicePlanner.CurrentCell.ColumnIndex].dataSource != null)
            {
                DataCollection.AddRange(plannerColumns[(SERVICE_RECORD_ATTRIBUTE)dataGridViewServicePlanner.CurrentCell.ColumnIndex].dataSource);
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
                    int elementColumnIndex = (int)SERVICE_RECORD_ATTRIBUTE.elementName;
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

                if (cell.OwningColumn.Name == SERVICE_RECORD_ATTRIBUTE.elementName.ToString())
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
            int titleColumnIndex = (int)SERVICE_RECORD_ATTRIBUTE.title;
            SetTitleComboBoxDataSource(dataGridViewServicePlanner.Rows[rowIndex].Cells[titleColumnIndex], elementIsMusical);
            int musicKeyColumnIndex = (int)SERVICE_RECORD_ATTRIBUTE.musicKey;
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
                if (dataGridViewServicePlanner.CurrentCell.ColumnIndex == (int)SERVICE_RECORD_ATTRIBUTE.musicKey)
                {
                    dataGridViewServicePlanner.EditingControl.Text = dataGridViewServicePlanner.EditingControl.Text.Replace("b", "♭");
                }
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
            FormatDataGridView();
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            //save dataGridView state to table for that day
            if (dataGridViewServicePlanner.DataSource == dataCtrlInstance.GetServiceInfo(calendarDatePicker.SelectionStart, 0))
            {
                MessageBox.Show("No changes to save");
            }
            else
            {
                dataCtrlInstance.SetServiceInfo((DataTable)dataGridViewServicePlanner.DataSource);
                MessageBox.Show("Changes saved");
            }
        }

        private void buttonAddRow_Click(object sender, EventArgs e)
        {
            DataRow newRow = ((DataTable)dataGridViewServicePlanner.DataSource).NewRow();
            newRow[(int)SERVICE_RECORD_ATTRIBUTE.date] = dataGridViewServicePlanner.Rows[0].Cells[(int)SERVICE_RECORD_ATTRIBUTE.date].Value;
            newRow[(int)SERVICE_RECORD_ATTRIBUTE.serviceNumber] = dataGridViewServicePlanner.Rows[0].Cells[(int)SERVICE_RECORD_ATTRIBUTE.serviceNumber].Value;
            newRow[(int)SERVICE_RECORD_ATTRIBUTE.orderInService] = dataGridViewServicePlanner.RowCount;
            ((DataTable)dataGridViewServicePlanner.DataSource).Rows.Add(newRow);
        }
    }
}
