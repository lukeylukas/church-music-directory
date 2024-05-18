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
            this.dataGridViewServiceRecords = new System.Windows.Forms.DataGridView();
            this.tabEditSong = new System.Windows.Forms.TabPage();
            this.textBoxSongKeys = new System.Windows.Forms.TextBox();
            this.textBoxHymnalKey = new System.Windows.Forms.TextBox();
            this.textBoxSongNotes = new System.Windows.Forms.TextBox();
            this.textBoxHymnalNumber = new System.Windows.Forms.TextBox();
            this.textBoxSongName = new System.Windows.Forms.TextBox();
            this.labelSongNotes = new System.Windows.Forms.Label();
            this.labelSongHymnalNumber = new System.Windows.Forms.Label();
            this.labelHymnalKey = new System.Windows.Forms.Label();
            this.labelSongKeys = new System.Windows.Forms.Label();
            this.labelSongName = new System.Windows.Forms.Label();
            this.buttonRemoveSong = new System.Windows.Forms.Button();
            this.buttonClearSongChange = new System.Windows.Forms.Button();
            this.buttonSaveSong = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.helloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonToggleServicePlanner = new System.Windows.Forms.Button();
            this.textBoxSubject = new System.Windows.Forms.TextBox();
            this.labelSubject = new System.Windows.Forms.Label();
            this.tabControlMain.SuspendLayout();
            this.tabPageSongInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPageServiceRecords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServiceRecords)).BeginInit();
            this.tabEditSong.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageSongInfo);
            this.tabControlMain.Controls.Add(this.tabPageServiceRecords);
            this.tabControlMain.Controls.Add(this.tabEditSong);
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
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
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
            // dataGridViewServiceRecords
            // 
            this.dataGridViewServiceRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewServiceRecords.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewServiceRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewServiceRecords.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewServiceRecords.Name = "dataGridViewServiceRecords";
            this.dataGridViewServiceRecords.ReadOnly = true;
            this.dataGridViewServiceRecords.RowHeadersVisible = false;
            this.dataGridViewServiceRecords.RowTemplate.Height = 25;
            this.dataGridViewServiceRecords.Size = new System.Drawing.Size(1003, 717);
            this.dataGridViewServiceRecords.TabIndex = 3;
            // 
            // tabEditSong
            // 
            this.tabEditSong.Controls.Add(this.textBoxSubject);
            this.tabEditSong.Controls.Add(this.labelSubject);
            this.tabEditSong.Controls.Add(this.textBoxSongKeys);
            this.tabEditSong.Controls.Add(this.textBoxHymnalKey);
            this.tabEditSong.Controls.Add(this.textBoxSongNotes);
            this.tabEditSong.Controls.Add(this.textBoxHymnalNumber);
            this.tabEditSong.Controls.Add(this.textBoxSongName);
            this.tabEditSong.Controls.Add(this.labelSongNotes);
            this.tabEditSong.Controls.Add(this.labelSongHymnalNumber);
            this.tabEditSong.Controls.Add(this.labelHymnalKey);
            this.tabEditSong.Controls.Add(this.labelSongKeys);
            this.tabEditSong.Controls.Add(this.labelSongName);
            this.tabEditSong.Controls.Add(this.buttonRemoveSong);
            this.tabEditSong.Controls.Add(this.buttonClearSongChange);
            this.tabEditSong.Controls.Add(this.buttonSaveSong);
            this.tabEditSong.Location = new System.Drawing.Point(4, 24);
            this.tabEditSong.Name = "tabEditSong";
            this.tabEditSong.Padding = new System.Windows.Forms.Padding(3);
            this.tabEditSong.Size = new System.Drawing.Size(1015, 729);
            this.tabEditSong.TabIndex = 2;
            this.tabEditSong.Text = "Song Edit";
            this.tabEditSong.UseVisualStyleBackColor = true;
            // 
            // textBoxSongKeys
            // 
            this.textBoxSongKeys.Location = new System.Drawing.Point(174, 101);
            this.textBoxSongKeys.Name = "textBoxSongKeys";
            this.textBoxSongKeys.Size = new System.Drawing.Size(100, 23);
            this.textBoxSongKeys.TabIndex = 12;
            // 
            // textBoxHymnalKey
            // 
            this.textBoxHymnalKey.Location = new System.Drawing.Point(174, 71);
            this.textBoxHymnalKey.Name = "textBoxHymnalKey";
            this.textBoxHymnalKey.Size = new System.Drawing.Size(100, 23);
            this.textBoxHymnalKey.TabIndex = 11;
            // 
            // textBoxSongNotes
            // 
            this.textBoxSongNotes.Location = new System.Drawing.Point(174, 161);
            this.textBoxSongNotes.Name = "textBoxSongNotes";
            this.textBoxSongNotes.Size = new System.Drawing.Size(100, 23);
            this.textBoxSongNotes.TabIndex = 10;
            // 
            // textBoxHymnalNumber
            // 
            this.textBoxHymnalNumber.Location = new System.Drawing.Point(174, 39);
            this.textBoxHymnalNumber.Name = "textBoxHymnalNumber";
            this.textBoxHymnalNumber.Size = new System.Drawing.Size(100, 23);
            this.textBoxHymnalNumber.TabIndex = 9;
            // 
            // textBoxSongName
            // 
            this.textBoxSongName.Location = new System.Drawing.Point(174, 10);
            this.textBoxSongName.Name = "textBoxSongName";
            this.textBoxSongName.Size = new System.Drawing.Size(100, 23);
            this.textBoxSongName.TabIndex = 8;
            // 
            // labelSongNotes
            // 
            this.labelSongNotes.AutoSize = true;
            this.labelSongNotes.Location = new System.Drawing.Point(130, 164);
            this.labelSongNotes.Name = "labelSongNotes";
            this.labelSongNotes.Size = new System.Drawing.Size(38, 15);
            this.labelSongNotes.TabIndex = 7;
            this.labelSongNotes.Text = "Notes";
            // 
            // labelSongHymnalNumber
            // 
            this.labelSongHymnalNumber.AutoSize = true;
            this.labelSongHymnalNumber.Location = new System.Drawing.Point(72, 42);
            this.labelSongHymnalNumber.Name = "labelSongHymnalNumber";
            this.labelSongHymnalNumber.Size = new System.Drawing.Size(96, 15);
            this.labelSongHymnalNumber.TabIndex = 6;
            this.labelSongHymnalNumber.Text = "Hymnal Number";
            // 
            // labelHymnalKey
            // 
            this.labelHymnalKey.AutoSize = true;
            this.labelHymnalKey.Location = new System.Drawing.Point(97, 74);
            this.labelHymnalKey.Name = "labelHymnalKey";
            this.labelHymnalKey.Size = new System.Drawing.Size(71, 15);
            this.labelHymnalKey.TabIndex = 5;
            this.labelHymnalKey.Text = "Hymnal Key";
            // 
            // labelSongKeys
            // 
            this.labelSongKeys.AutoSize = true;
            this.labelSongKeys.Location = new System.Drawing.Point(137, 104);
            this.labelSongKeys.Name = "labelSongKeys";
            this.labelSongKeys.Size = new System.Drawing.Size(31, 15);
            this.labelSongKeys.TabIndex = 4;
            this.labelSongKeys.Text = "Keys";
            // 
            // labelSongName
            // 
            this.labelSongName.AutoSize = true;
            this.labelSongName.Location = new System.Drawing.Point(129, 13);
            this.labelSongName.Name = "labelSongName";
            this.labelSongName.Size = new System.Drawing.Size(39, 15);
            this.labelSongName.TabIndex = 3;
            this.labelSongName.Text = "Name";
            // 
            // buttonRemoveSong
            // 
            this.buttonRemoveSong.Location = new System.Drawing.Point(173, 201);
            this.buttonRemoveSong.Name = "buttonRemoveSong";
            this.buttonRemoveSong.Size = new System.Drawing.Size(101, 23);
            this.buttonRemoveSong.TabIndex = 2;
            this.buttonRemoveSong.Text = "Remove Song";
            this.buttonRemoveSong.UseVisualStyleBackColor = true;
            this.buttonRemoveSong.Click += new System.EventHandler(this.buttonRemoveSong_Click);
            // 
            // buttonClearSongChange
            // 
            this.buttonClearSongChange.Location = new System.Drawing.Point(92, 201);
            this.buttonClearSongChange.Name = "buttonClearSongChange";
            this.buttonClearSongChange.Size = new System.Drawing.Size(75, 23);
            this.buttonClearSongChange.TabIndex = 1;
            this.buttonClearSongChange.Text = "Clear";
            this.buttonClearSongChange.UseVisualStyleBackColor = true;
            this.buttonClearSongChange.Click += new System.EventHandler(this.buttonClearSongChange_Click);
            // 
            // buttonSaveSong
            // 
            this.buttonSaveSong.Location = new System.Drawing.Point(11, 201);
            this.buttonSaveSong.Name = "buttonSaveSong";
            this.buttonSaveSong.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSong.TabIndex = 0;
            this.buttonSaveSong.Text = "Save";
            this.buttonSaveSong.UseVisualStyleBackColor = true;
            this.buttonSaveSong.Click += new System.EventHandler(this.buttonSaveSong_Click);
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
            // textBoxSubject
            // 
            this.textBoxSubject.Location = new System.Drawing.Point(174, 131);
            this.textBoxSubject.Name = "textBoxSubject";
            this.textBoxSubject.Size = new System.Drawing.Size(100, 23);
            this.textBoxSubject.TabIndex = 14;
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(122, 134);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(46, 15);
            this.labelSubject.TabIndex = 13;
            this.labelSubject.Text = "Subject";
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServiceRecords)).EndInit();
            this.tabEditSong.ResumeLayout(false);
            this.tabEditSong.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
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
        private TabPage tabEditSong;
        private Button buttonRemoveSong;
        private Button buttonClearSongChange;
        private Button buttonSaveSong;
        private Label labelSongNotes;
        private Label labelSongHymnalNumber;
        private Label labelHymnalKey;
        private Label labelSongKeys;
        private Label labelSongName;
        private TextBox textBoxSongKeys;
        private TextBox textBoxHymnalKey;
        private TextBox textBoxSongNotes;
        private TextBox textBoxHymnalNumber;
        private TextBox textBoxSongName;
        private TextBox textBoxSubject;
        private Label labelSubject;
    }
}