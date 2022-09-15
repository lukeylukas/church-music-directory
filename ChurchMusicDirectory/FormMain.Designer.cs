namespace ChurchMusicDirectory
{
    partial class FormMain
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSongInfo = new System.Windows.Forms.TabPage();
            this.tabPageWorshipPlanner = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageSongInfo.SuspendLayout();
            this.tabPageWorshipPlanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSongInfo);
            this.tabControl1.Controls.Add(this.tabPageWorshipPlanner);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(855, 712);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageSongInfo
            // 
            this.tabPageSongInfo.Controls.Add(this.richTextBox1);
            this.tabPageSongInfo.Location = new System.Drawing.Point(4, 24);
            this.tabPageSongInfo.Name = "tabPageSongInfo";
            this.tabPageSongInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSongInfo.Size = new System.Drawing.Size(847, 684);
            this.tabPageSongInfo.TabIndex = 0;
            this.tabPageSongInfo.Text = "All Songs";
            this.tabPageSongInfo.UseVisualStyleBackColor = true;
            // 
            // tabPageWorshipPlanner
            // 
            this.tabPageWorshipPlanner.Controls.Add(this.richTextBox2);
            this.tabPageWorshipPlanner.Location = new System.Drawing.Point(4, 24);
            this.tabPageWorshipPlanner.Name = "tabPageWorshipPlanner";
            this.tabPageWorshipPlanner.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWorshipPlanner.Size = new System.Drawing.Size(847, 684);
            this.tabPageWorshipPlanner.TabIndex = 1;
            this.tabPageWorshipPlanner.Text = "Worship Planner";
            this.tabPageWorshipPlanner.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(121, 88);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(431, 96);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "This page will show all song info. One column will display the last date that son" +
    "g was played. Clicking/hovering will display a dropdown containing all the dates" +
    " that song was played";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(129, 87);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(362, 96);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "This page allows the user to arrange and select elements of worship, creating an " +
    "order of service. (The ability for users to create a template would be good)";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 736);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMain";
            this.Text = "Church Music Directory";
            this.tabControl1.ResumeLayout(false);
            this.tabPageSongInfo.ResumeLayout(false);
            this.tabPageWorshipPlanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPageSongInfo;
        private TabPage tabPageWorshipPlanner;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
    }
}