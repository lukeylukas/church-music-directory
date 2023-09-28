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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelServicePlanner = new System.Windows.Forms.Label();
            this.dataGridViewServicePlanner = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonCalendar = new System.Windows.Forms.Button();
            this.calendarDatePicker = new System.Windows.Forms.MonthCalendar();
            this.buttonDiscardChanges = new System.Windows.Forms.Button();
            this.buttonSaveChanges = new System.Windows.Forms.Button();
            this.buttonAddRow = new System.Windows.Forms.Button();
            this.labelServiceDate = new System.Windows.Forms.Label();
            this.buttonInsert = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServicePlanner)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
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
            this.dataGridViewServicePlanner.AllowUserToAddRows = false;
            this.dataGridViewServicePlanner.AllowUserToDeleteRows = false;
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
            this.dataGridViewServicePlanner.ContextMenuStrip = this.contextMenuStrip1;
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
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
            // buttonDiscardChanges
            // 
            this.buttonDiscardChanges.Location = new System.Drawing.Point(120, 49);
            this.buttonDiscardChanges.Name = "buttonDiscardChanges";
            this.buttonDiscardChanges.Size = new System.Drawing.Size(124, 23);
            this.buttonDiscardChanges.TabIndex = 9;
            this.buttonDiscardChanges.Text = "Discard Changes";
            this.buttonDiscardChanges.UseVisualStyleBackColor = true;
            this.buttonDiscardChanges.Click += new System.EventHandler(this.buttonDiscardChanges_Click);
            // 
            // buttonSaveChanges
            // 
            this.buttonSaveChanges.Location = new System.Drawing.Point(12, 49);
            this.buttonSaveChanges.Name = "buttonSaveChanges";
            this.buttonSaveChanges.Size = new System.Drawing.Size(102, 23);
            this.buttonSaveChanges.TabIndex = 10;
            this.buttonSaveChanges.Text = "Save Changes";
            this.buttonSaveChanges.UseVisualStyleBackColor = true;
            this.buttonSaveChanges.Click += new System.EventHandler(this.buttonSaveChanges_Click);
            // 
            // buttonAddRow
            // 
            this.buttonAddRow.Location = new System.Drawing.Point(291, 49);
            this.buttonAddRow.Name = "buttonAddRow";
            this.buttonAddRow.Size = new System.Drawing.Size(45, 23);
            this.buttonAddRow.TabIndex = 11;
            this.buttonAddRow.Text = "Add";
            this.buttonAddRow.UseVisualStyleBackColor = true;
            this.buttonAddRow.Click += new System.EventHandler(this.buttonAddRow_Click);
            // 
            // labelServiceDate
            // 
            this.labelServiceDate.AutoSize = true;
            this.labelServiceDate.Location = new System.Drawing.Point(222, 23);
            this.labelServiceDate.Name = "labelServiceDate";
            this.labelServiceDate.Size = new System.Drawing.Size(97, 15);
            this.labelServiceDate.TabIndex = 12;
            this.labelServiceDate.Text = "No Date Selected";
            // 
            // buttonInsert
            // 
            this.buttonInsert.Location = new System.Drawing.Point(342, 49);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(45, 23);
            this.buttonInsert.TabIndex = 13;
            this.buttonInsert.Text = "Insert";
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click);
            // 
            // FormServicePlanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(680, 479);
            this.Controls.Add(this.buttonInsert);
            this.Controls.Add(this.labelServiceDate);
            this.Controls.Add(this.buttonAddRow);
            this.Controls.Add(this.buttonSaveChanges);
            this.Controls.Add(this.buttonDiscardChanges);
            this.Controls.Add(this.calendarDatePicker);
            this.Controls.Add(this.buttonCalendar);
            this.Controls.Add(this.dataGridViewServicePlanner);
            this.Controls.Add(this.labelServicePlanner);
            this.Name = "FormServicePlanner";
            this.Text = "FormServicePlanner";
            this.Shown += new System.EventHandler(this.FormServicePlanner_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServicePlanner)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label labelServicePlanner;
        private DataGridView dataGridViewServicePlanner;
        private Button buttonCalendar;
        private MonthCalendar calendarDatePicker;
        private Button buttonDiscardChanges;
        private Button buttonSaveChanges;
        private Button buttonAddRow;
        private Label labelServiceDate;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private Button buttonInsert;
    }
}