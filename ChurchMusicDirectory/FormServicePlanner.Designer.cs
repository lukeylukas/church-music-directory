namespace ChurchMusicDirectory
{
    partial class FormServicePlanner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelServicePlanner = new System.Windows.Forms.Label();
            this.dataGridViewServicePlanner = new System.Windows.Forms.DataGridView();
            this.buttonCalendar = new System.Windows.Forms.Button();
            this.calendarDatePicker = new System.Windows.Forms.MonthCalendar();
            this.comboBoxServiceDate = new System.Windows.Forms.ComboBox();
            this.comboBoxServiceNumber = new System.Windows.Forms.ComboBox();
            this.labelServiceNumber = new System.Windows.Forms.Label();
            this.buttonDiscardChanges = new System.Windows.Forms.Button();
            this.buttonSaveChanges = new System.Windows.Forms.Button();
            this.buttonAddRow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServicePlanner)).BeginInit();
            this.SuspendLayout();
            // 
            // labelServicePlanner
            // 
            this.labelServicePlanner.AutoSize = true;
            this.labelServicePlanner.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelServicePlanner.Location = new System.Drawing.Point(12, 9);
            this.labelServicePlanner.Name = "labelServicePlanner";
            this.labelServicePlanner.Size = new System.Drawing.Size(196, 37);
            this.labelServicePlanner.TabIndex = 1;
            this.labelServicePlanner.Text = "Service Planner";
            // 
            // dataGridViewServicePlanner
            // 
            this.dataGridViewServicePlanner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewServicePlanner.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewServicePlanner.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewServicePlanner.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewServicePlanner.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewServicePlanner.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.dataGridViewServicePlanner.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewServicePlanner.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewServicePlanner.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewServicePlanner.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewServicePlanner.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridViewServicePlanner.Location = new System.Drawing.Point(1, 78);
            this.dataGridViewServicePlanner.Name = "dataGridViewServicePlanner";
            this.dataGridViewServicePlanner.RowHeadersVisible = false;
            this.dataGridViewServicePlanner.RowTemplate.Height = 25;
            this.dataGridViewServicePlanner.Size = new System.Drawing.Size(678, 400);
            this.dataGridViewServicePlanner.TabIndex = 2;
            this.dataGridViewServicePlanner.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewServicePlanner_EditingControlShowing);
            // 
            // buttonCalendar
            // 
            this.buttonCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCalendar.Location = new System.Drawing.Point(587, 9);
            this.buttonCalendar.Name = "buttonCalendar";
            this.buttonCalendar.Size = new System.Drawing.Size(75, 31);
            this.buttonCalendar.TabIndex = 4;
            this.buttonCalendar.Text = "Calendar";
            this.buttonCalendar.UseVisualStyleBackColor = true;
            this.buttonCalendar.Click += new System.EventHandler(this.buttonCalendar_Click);
            // 
            // calendarDatePicker
            // 
            this.calendarDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.calendarDatePicker.Location = new System.Drawing.Point(435, 38);
            this.calendarDatePicker.MaxSelectionCount = 1;
            this.calendarDatePicker.Name = "calendarDatePicker";
            this.calendarDatePicker.TabIndex = 5;
            this.calendarDatePicker.Visible = false;
            this.calendarDatePicker.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarDatePicker_DateSelected);
            // 
            // comboBoxServiceDate
            // 
            this.comboBoxServiceDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxServiceDate.FormattingEnabled = true;
            this.comboBoxServiceDate.Location = new System.Drawing.Point(485, 9);
            this.comboBoxServiceDate.Name = "comboBoxServiceDate";
            this.comboBoxServiceDate.Size = new System.Drawing.Size(96, 23);
            this.comboBoxServiceDate.TabIndex = 6;
            // 
            // comboBoxServiceNumber
            // 
            this.comboBoxServiceNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxServiceNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxServiceNumber.FormattingEnabled = true;
            this.comboBoxServiceNumber.Location = new System.Drawing.Point(495, 38);
            this.comboBoxServiceNumber.Name = "comboBoxServiceNumber";
            this.comboBoxServiceNumber.Size = new System.Drawing.Size(49, 23);
            this.comboBoxServiceNumber.TabIndex = 7;
            // 
            // labelServiceNumber
            // 
            this.labelServiceNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelServiceNumber.AutoSize = true;
            this.labelServiceNumber.Location = new System.Drawing.Point(435, 41);
            this.labelServiceNumber.Name = "labelServiceNumber";
            this.labelServiceNumber.Size = new System.Drawing.Size(54, 15);
            this.labelServiceNumber.TabIndex = 8;
            this.labelServiceNumber.Text = "Service #";
            // 
            // buttonDiscardChanges
            // 
            this.buttonDiscardChanges.Location = new System.Drawing.Point(171, 49);
            this.buttonDiscardChanges.Name = "buttonDiscardChanges";
            this.buttonDiscardChanges.Size = new System.Drawing.Size(124, 23);
            this.buttonDiscardChanges.TabIndex = 9;
            this.buttonDiscardChanges.Text = "Discard Changes";
            this.buttonDiscardChanges.UseVisualStyleBackColor = true;
            this.buttonDiscardChanges.Click += new System.EventHandler(this.buttonDiscardChanges_Click);
            // 
            // buttonSaveChanges
            // 
            this.buttonSaveChanges.Location = new System.Drawing.Point(63, 49);
            this.buttonSaveChanges.Name = "buttonSaveChanges";
            this.buttonSaveChanges.Size = new System.Drawing.Size(102, 23);
            this.buttonSaveChanges.TabIndex = 10;
            this.buttonSaveChanges.Text = "Save Changes";
            this.buttonSaveChanges.UseVisualStyleBackColor = true;
            this.buttonSaveChanges.Click += new System.EventHandler(this.buttonSaveChanges_Click);
            // 
            // buttonAddRow
            // 
            this.buttonAddRow.Location = new System.Drawing.Point(12, 49);
            this.buttonAddRow.Name = "buttonAddRow";
            this.buttonAddRow.Size = new System.Drawing.Size(45, 23);
            this.buttonAddRow.TabIndex = 11;
            this.buttonAddRow.Text = "Add";
            this.buttonAddRow.UseVisualStyleBackColor = true;
            this.buttonAddRow.Click += new System.EventHandler(this.buttonAddRow_Click);
            // 
            // FormServicePlanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(680, 479);
            this.Controls.Add(this.buttonAddRow);
            this.Controls.Add(this.buttonSaveChanges);
            this.Controls.Add(this.buttonDiscardChanges);
            this.Controls.Add(this.labelServiceNumber);
            this.Controls.Add(this.comboBoxServiceNumber);
            this.Controls.Add(this.comboBoxServiceDate);
            this.Controls.Add(this.calendarDatePicker);
            this.Controls.Add(this.buttonCalendar);
            this.Controls.Add(this.dataGridViewServicePlanner);
            this.Controls.Add(this.labelServicePlanner);
            this.Name = "FormServicePlanner";
            this.Text = "FormServicePlanner";
            this.Shown += new System.EventHandler(this.FormServicePlanner_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServicePlanner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label labelServicePlanner;
        private DataGridView dataGridViewServicePlanner;
        private Button buttonCalendar;
        private MonthCalendar calendarDatePicker;
        private ComboBox comboBoxServiceDate;
        private ComboBox comboBoxServiceNumber;
        private Label labelServiceNumber;
        private Button buttonDiscardChanges;
        private Button buttonSaveChanges;
        private Button buttonAddRow;
    }
}