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
        string[]? songTitles;
        string[]? musicKeys;
        private int contextMenuRowIndex;
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
            songTitles = new string[dataCtrlInstance.titlesList.Count + 1];
            songTitles[0] = "";
            dataCtrlInstance.titlesList.ToArray().CopyTo(songTitles, 1);
            Array.Sort(songTitles);

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
                    SERVICE_RECORD_ATTRIBUTE.title,
                    new SERVICE_PLANNER_COLUMN
                    {
                        name="Title",
                        visible=true,
                        displayOrder=1,
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
                        displayOrder=2,
                        type=(new DataGridViewComboBoxColumn()).GetType(),
                        width=75,
                        dataSource = musicKeys
                    }
                },
                {
                    SERVICE_RECORD_ATTRIBUTE.notes,
                    new SERVICE_PLANNER_COLUMN
                    {
                        name="Notes",
                        visible=true,
                        displayOrder=3,
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
                        width=2,
                        dataSource = null
                    }
                },
                {
                    SERVICE_RECORD_ATTRIBUTE.orderInService,
                    new SERVICE_PLANNER_COLUMN
                    {
                        name="Order",
                        visible=false,
                        displayOrder=4,
                        type=(new DataGridViewComboBoxColumn()).GetType(),
                        width=2,
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
            for (int columnIndex = plannerColumns.Count - 1; columnIndex >= 0; columnIndex--)
            {
                if (dataGridViewServicePlanner.Columns[columnIndex].Visible)
                {
                    dataGridViewServicePlanner.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    break;
                }
            }
            dataGridViewServicePlanner.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewServicePlanner.DataError += new DataGridViewDataErrorEventHandler(dataGridViewServicePlanner_DataError);
            dataGridViewServicePlanner.CellEndEdit += new DataGridViewCellEventHandler(dataGridViewServicePlanner_CellEndEdit);
            calendarDatePicker.SetSelectionRange(dataCtrlInstance.GetNextServiceDate(), dataCtrlInstance.GetNextServiceDate());
            FormatDataGridView();
            labelServiceDate.Text = calendarDatePicker.SelectionStart.ToString("dddd, MMMM d, yyyy");
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
        }

        private void buttonCalendar_Click(object sender, EventArgs e)
        {
            calendarDatePicker.Visible = !calendarDatePicker.Visible;
        }

        private void calendarDatePicker_DateSelected(object sender, DateRangeEventArgs e)
        {
            calendarDatePicker.Visible = false;
            FormatDataGridView();
            // change the date in labelServiceDate
            labelServiceDate.Text = calendarDatePicker.SelectionStart.ToString("dddd, MMMM d, yyyy");
        }
        private void FormatDataGridView()
        {
            dataGridViewServicePlanner.DataSource = dataCtrlInstance.GetServiceInfo(calendarDatePicker.SelectionStart);
            if (dataGridViewServicePlanner.Rows.Count > 1)
            {
                dataGridViewServicePlanner.Sort(dataGridViewServicePlanner.Columns[(int)SERVICE_RECORD_ATTRIBUTE.orderInService], ListSortDirection.Ascending);
            }
            else
            {
                AddServicePlannerRows(4); // When I add this else block, the date column becomes visible. So the next line hides it again.
            }
            dataGridViewServicePlanner.Columns[(int)SERVICE_RECORD_ATTRIBUTE.date].Visible = false;
            // make datasource for the title column the list of song titles
            int titleColumnIndex = (int)SERVICE_RECORD_ATTRIBUTE.title;
            DataGridViewComboBoxColumn titleColumn = (DataGridViewComboBoxColumn)dataGridViewServicePlanner.Columns[titleColumnIndex];
            titleColumn.DataSource = songTitles;
            OrderOfServiceRefresh();
            this.dataGridViewServicePlanner.Focus();
        }

        private void FormServicePlanner_Shown(object sender, EventArgs e)
        {
        }

        private void dataGridViewServicePlanner_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var comboEditor = e.Control as DataGridViewComboBoxEditingControl;
            if (comboEditor != null)
            {
                comboEditor.DropDownStyle = ComboBoxStyle.DropDownList;
                comboEditor.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboEditor.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboEditor.AutoCompleteCustomSource = GetDataCollection();
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

        private void dataGridViewServicePlanner_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewComboBoxCell cell = dataGridViewServicePlanner.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
            if (cell != null)
            {
                cell.Value = tempComboBoxValue;
            }
            OrderOfServiceRefresh();
        }
        private void OrderOfServiceRefresh()
        {
            this.richTextBoxOrderOfService.Clear();
            for (int rowIdx = 0; rowIdx < this.dataGridViewServicePlanner.RowCount; rowIdx++)
            {
                if (rowIdx == 2)
                {
                    this.richTextBoxOrderOfService.AppendText("\n");
                    this.richTextBoxOrderOfService.SelectionFont = new System.Drawing.Font(this.richTextBoxOrderOfService.Font, System.Drawing.FontStyle.Italic);
                    this.richTextBoxOrderOfService.AppendText("Offering/Prayer");
                    this.richTextBoxOrderOfService.SelectionFont = new System.Drawing.Font(this.richTextBoxOrderOfService.Font, System.Drawing.FontStyle.Regular);
                }
                if (rowIdx == 4)
                {
                    this.richTextBoxOrderOfService.AppendText("\n");
                    this.richTextBoxOrderOfService.SelectionFont = new System.Drawing.Font(this.richTextBoxOrderOfService.Font, System.Drawing.FontStyle.Italic);
                    this.richTextBoxOrderOfService.AppendText("Sermon/Communion");
                    this.richTextBoxOrderOfService.SelectionFont = new System.Drawing.Font(this.richTextBoxOrderOfService.Font, System.Drawing.FontStyle.Regular);
                }
                DataGridViewRow row = this.dataGridViewServicePlanner.Rows[rowIdx];
                if (row.Cells[(int)SERVICE_RECORD_ATTRIBUTE.musicKey].Value is not DBNull
                    && row.Cells[(int)SERVICE_RECORD_ATTRIBUTE.title].Value is not DBNull
                    && row.Cells[(int)SERVICE_RECORD_ATTRIBUTE.musicKey].Value.ToString() != ""
                    && row.Cells[(int)SERVICE_RECORD_ATTRIBUTE.title].Value.ToString() != "")
                {
                    if (rowIdx != 0)
                    {
                        this.richTextBoxOrderOfService.AppendText("\n");
                    }
                    string key = row.Cells[(int)SERVICE_RECORD_ATTRIBUTE.musicKey].Value.ToString();
                    string songName = (string)row.Cells[(int)SERVICE_RECORD_ATTRIBUTE.title].Value;
                    int hymnalNumber = dataCtrlInstance.GetHymnalNumber(songName);
                    string hymnalNumberString = "";
                    if (hymnalNumber > 0)
                    {
                        hymnalNumberString = " - #" + hymnalNumber.ToString();
                    }
                    string orderOfServiceLine = songName + hymnalNumberString + " (" + key + ")";
                    this.richTextBoxOrderOfService.AppendText(orderOfServiceLine);
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
            if (dataGridViewServicePlanner.DataSource == dataCtrlInstance.GetServiceInfo(calendarDatePicker.SelectionStart))
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
            AddServicePlannerRows(1);
        }
        private void AddServicePlannerRows(int count)
        {
            while (count-- > 0)
            {
                DataRow newRow = ((DataTable)dataGridViewServicePlanner.DataSource).NewRow();
                newRow[(int)SERVICE_RECORD_ATTRIBUTE.date] = dataGridViewServicePlanner.Rows[0].Cells[(int)SERVICE_RECORD_ATTRIBUTE.date].Value;
                newRow[(int)SERVICE_RECORD_ATTRIBUTE.orderInService] = dataGridViewServicePlanner.RowCount + 1;
                ((DataTable)dataGridViewServicePlanner.DataSource).Rows.Add(newRow);
                // dataGridViewServicePlanner.Rows.Add(1);
                // dataGridViewServicePlanner.Rows[^1].Cells[(int)SERVICE_RECORD_ATTRIBUTE.date].Value = dataGridViewServicePlanner.Rows[0].Cells[(int)SERVICE_RECORD_ATTRIBUTE.date].Value;
                // dataGridViewServicePlanner.Rows[^1].Cells[(int)SERVICE_RECORD_ATTRIBUTE.orderInService].Value = dataGridViewServicePlanner.RowCount + 1;
            };
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewServicePlanner.Rows.RemoveAt(contextMenuRowIndex);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            DataGridView.HitTestInfo menuLocation = Utils.MousePositionInTable(dataGridViewServicePlanner, MousePosition);
            contextMenuRowIndex = menuLocation.RowIndex;
            e.Cancel = false;
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridViewServicePlanner.CurrentCell.RowIndex;
            DataRow newRow = ((DataTable)dataGridViewServicePlanner.DataSource).NewRow();
            newRow[(int)SERVICE_RECORD_ATTRIBUTE.date] = dataGridViewServicePlanner.Rows[0].Cells[(int)SERVICE_RECORD_ATTRIBUTE.date].Value;
            ((DataTable)dataGridViewServicePlanner.DataSource).Rows.InsertAt(newRow, selectedRowIndex);
            for (int i = selectedRowIndex; i < dataGridViewServicePlanner.RowCount; i++)
            {
                dataGridViewServicePlanner.Rows[i].Cells[(int)SERVICE_RECORD_ATTRIBUTE.orderInService].Value = i + 1;
            }
        }

        private void buttonCopyOrderOfService_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.richTextBoxOrderOfService.Text);
        }
    }
}
