namespace ChurchMusicDirectory
{
    partial class FormSongTables
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageSongInfo = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPageServiceRecords = new System.Windows.Forms.TabPage();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.helloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonToggleServicePlanner = new System.Windows.Forms.Button();
            this.dataGridViewServiceRecords = new System.Windows.Forms.DataGridView();
            this.tabControlMain.SuspendLayout();
            this.tabPageSongInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPageServiceRecords.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServiceRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageSongInfo);
            this.tabControlMain.Controls.Add(this.tabPageServiceRecords);
            this.tabControlMain.Location = new System.Drawing.Point(12, 12);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1023, 757);
            this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageSongInfo
            // 
            this.tabPageSongInfo.Controls.Add(this.dataGridView1);
            this.tabPageSongInfo.Location = new System.Drawing.Point(4, 24);
            this.tabPageSongInfo.Name = "tabPageSongInfo";
            this.tabPageSongInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSongInfo.Size = new System.Drawing.Size(1015, 729);
            this.tabPageSongInfo.TabIndex = 0;
            this.tabPageSongInfo.Text = "All Songs";
            this.tabPageSongInfo.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(1003, 717);
            this.dataGridView1.TabIndex = 2;
            // 
            // tabPageServiceRecords
            // 
            this.tabPageServiceRecords.Controls.Add(this.dataGridViewServiceRecords);
            this.tabPageServiceRecords.Location = new System.Drawing.Point(4, 24);
            this.tabPageServiceRecords.Name = "tabPageServiceRecords";
            this.tabPageServiceRecords.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServiceRecords.Size = new System.Drawing.Size(1015, 729);
            this.tabPageServiceRecords.TabIndex = 1;
            this.tabPageServiceRecords.Text = "Service Records";
            this.tabPageServiceRecords.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helloToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // helloToolStripMenuItem
            // 
            this.helloToolStripMenuItem.Name = "helloToolStripMenuItem";
            this.helloToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.helloToolStripMenuItem.Text = "Hello";
            // 
            // buttonToggleServicePlanner
            // 
            this.buttonToggleServicePlanner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToggleServicePlanner.Location = new System.Drawing.Point(890, 7);
            this.buttonToggleServicePlanner.Name = "buttonToggleServicePlanner";
            this.buttonToggleServicePlanner.Size = new System.Drawing.Size(111, 23);
            this.buttonToggleServicePlanner.TabIndex = 1;
            this.buttonToggleServicePlanner.Text = "Service Planner";
            this.buttonToggleServicePlanner.UseVisualStyleBackColor = true;
            this.buttonToggleServicePlanner.Click += new System.EventHandler(this.buttonToggleServicePlanner_Click);
            // 
            // dataGridViewServiceRecords
            // 
            this.dataGridViewServiceRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewServiceRecords.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewServiceRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewServiceRecords.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewServiceRecords.Name = "dataGridViewServiceRecords";
            this.dataGridViewServiceRecords.RowHeadersVisible = false;
            this.dataGridViewServiceRecords.RowTemplate.Height = 25;
            this.dataGridViewServiceRecords.Size = new System.Drawing.Size(1003, 717);
            this.dataGridViewServiceRecords.TabIndex = 3;
            // 
            // FormSongTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 781);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.buttonToggleServicePlanner);
            this.Controls.Add(this.tabControlMain);
            this.Name = "FormSongTables";
            this.Text = "Church Music Directory";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSongInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPageServiceRecords.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServiceRecords)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControlMain;
        private TabPage tabPageSongInfo;
        private TabPage tabPageServiceRecords;
        private DataGridView dataGridView1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem helloToolStripMenuItem;
        private Button buttonToggleServicePlanner;
        private DataGridView dataGridViewServiceRecords;
    }
}