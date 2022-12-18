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
            this.ColumnElement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMusicKey = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonCalendar = new System.Windows.Forms.Button();
            this.monthCalendarDatePicker = new System.Windows.Forms.MonthCalendar();
            this.comboBoxServiceDate = new System.Windows.Forms.ComboBox();
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
            this.dataGridViewServicePlanner.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnElement,
            this.ColumnTitle,
            this.ColumnMusicKey,
            this.ColumnInfo});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewServicePlanner.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewServicePlanner.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridViewServicePlanner.Location = new System.Drawing.Point(1, 62);
            this.dataGridViewServicePlanner.Name = "dataGridViewServicePlanner";
            this.dataGridViewServicePlanner.RowHeadersVisible = false;
            this.dataGridViewServicePlanner.RowTemplate.Height = 25;
            this.dataGridViewServicePlanner.Size = new System.Drawing.Size(605, 416);
            this.dataGridViewServicePlanner.TabIndex = 2;
            // 
            // ColumnElement
            // 
            this.ColumnElement.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnElement.HeaderText = "Element";
            this.ColumnElement.Name = "ColumnElement";
            // 
            // ColumnTitle
            // 
            this.ColumnTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnTitle.FillWeight = 178.7234F;
            this.ColumnTitle.HeaderText = "Title";
            this.ColumnTitle.MinimumWidth = 200;
            this.ColumnTitle.Name = "ColumnTitle";
            this.ColumnTitle.Width = 200;
            // 
            // ColumnMusicKey
            // 
            this.ColumnMusicKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnMusicKey.HeaderText = "Key";
            this.ColumnMusicKey.MinimumWidth = 50;
            this.ColumnMusicKey.Name = "ColumnMusicKey";
            this.ColumnMusicKey.Width = 50;
            // 
            // ColumnInfo
            // 
            this.ColumnInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnInfo.FillWeight = 21.2766F;
            this.ColumnInfo.HeaderText = "Info";
            this.ColumnInfo.MinimumWidth = 50;
            this.ColumnInfo.Name = "ColumnInfo";
            // 
            // buttonCalendar
            // 
            this.buttonCalendar.Location = new System.Drawing.Point(520, 14);
            this.buttonCalendar.Name = "buttonCalendar";
            this.buttonCalendar.Size = new System.Drawing.Size(75, 23);
            this.buttonCalendar.TabIndex = 4;
            this.buttonCalendar.Text = "Calendar";
            this.buttonCalendar.UseVisualStyleBackColor = true;
            this.buttonCalendar.Click += new System.EventHandler(this.buttonCalendar_Click);
            // 
            // monthCalendarDatePicker
            // 
            this.monthCalendarDatePicker.Location = new System.Drawing.Point(368, 35);
            this.monthCalendarDatePicker.MaxSelectionCount = 1;
            this.monthCalendarDatePicker.Name = "monthCalendarDatePicker";
            this.monthCalendarDatePicker.TabIndex = 5;
            this.monthCalendarDatePicker.Visible = false;
            this.monthCalendarDatePicker.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendarDatePicker_DateSelected);
            // 
            // comboBoxServiceDate
            // 
            this.comboBoxServiceDate.FormattingEnabled = true;
            this.comboBoxServiceDate.Location = new System.Drawing.Point(368, 9);
            this.comboBoxServiceDate.Name = "comboBoxServiceDate";
            this.comboBoxServiceDate.Size = new System.Drawing.Size(146, 23);
            this.comboBoxServiceDate.TabIndex = 6;
            // 
            // FormServicePlanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 479);
            this.Controls.Add(this.comboBoxServiceDate);
            this.Controls.Add(this.monthCalendarDatePicker);
            this.Controls.Add(this.buttonCalendar);
            this.Controls.Add(this.dataGridViewServicePlanner);
            this.Controls.Add(this.labelServicePlanner);
            this.Name = "FormServicePlanner";
            this.Text = "FormServicePlanner";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServicePlanner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label labelServicePlanner;
        private DataGridView dataGridViewServicePlanner;
        private DataGridViewTextBoxColumn ColumnElement;
        private DataGridViewTextBoxColumn ColumnTitle;
        private DataGridViewComboBoxColumn ColumnMusicKey;
        private DataGridViewTextBoxColumn ColumnInfo;
        private Button buttonCalendar;
        private MonthCalendar monthCalendarDatePicker;
        private ComboBox comboBoxServiceDate;
    }
}