namespace GomokuClient
{
    partial class Form1
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.connectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.playerThink = new System.Windows.Forms.ToolStripStatusLabel();
            this.computerColorPanel = new System.Windows.Forms.Panel();
            this.playerBlackLabel = new System.Windows.Forms.Label();
            this.playerOneColorPanel = new System.Windows.Forms.Panel();
            this.playerWhiteLabel = new System.Windows.Forms.Label();
            this.turnPicture = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.showNumbersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.boardCanvas = new GomokuClient.BoardCanvas();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.turnPicture)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.statusStrip1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.computerColorPanel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.playerBlackLabel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.playerOneColorPanel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.playerWhiteLabel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.turnPicture);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.boardCanvas);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(489, 555);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(489, 579);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionStatusLabel,
            this.playerThink,
            this.gameStatusLabel,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 533);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(489, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(139, 17);
            this.connectionStatusLabel.Text = "Server connection status:";
            // 
            // playerThink
            // 
            this.playerThink.Name = "playerThink";
            this.playerThink.Size = new System.Drawing.Size(0, 17);
            // 
            // computerColorPanel
            // 
            this.computerColorPanel.BackColor = System.Drawing.Color.Black;
            this.computerColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.computerColorPanel.Location = new System.Drawing.Point(447, 497);
            this.computerColorPanel.Name = "computerColorPanel";
            this.computerColorPanel.Size = new System.Drawing.Size(25, 25);
            this.computerColorPanel.TabIndex = 10;
            // 
            // playerBlackLabel
            // 
            this.playerBlackLabel.AutoSize = true;
            this.playerBlackLabel.Location = new System.Drawing.Point(389, 503);
            this.playerBlackLabel.Name = "playerBlackLabel";
            this.playerBlackLabel.Size = new System.Drawing.Size(52, 13);
            this.playerBlackLabel.TabIndex = 9;
            this.playerBlackLabel.Text = "Computer";
            // 
            // playerOneColorPanel
            // 
            this.playerOneColorPanel.BackColor = System.Drawing.Color.White;
            this.playerOneColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playerOneColorPanel.Location = new System.Drawing.Point(14, 497);
            this.playerOneColorPanel.Name = "playerOneColorPanel";
            this.playerOneColorPanel.Size = new System.Drawing.Size(25, 25);
            this.playerOneColorPanel.TabIndex = 8;
            // 
            // playerWhiteLabel
            // 
            this.playerWhiteLabel.AutoSize = true;
            this.playerWhiteLabel.Location = new System.Drawing.Point(45, 503);
            this.playerWhiteLabel.Name = "playerWhiteLabel";
            this.playerWhiteLabel.Size = new System.Drawing.Size(41, 13);
            this.playerWhiteLabel.TabIndex = 7;
            this.playerWhiteLabel.Text = "Human";
            // 
            // turnPicture
            // 
            this.turnPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.turnPicture.Location = new System.Drawing.Point(254, 496);
            this.turnPicture.Name = "turnPicture";
            this.turnPicture.Size = new System.Drawing.Size(25, 25);
            this.turnPicture.TabIndex = 6;
            this.turnPicture.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(219, 501);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Turn";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem1,
            this.editToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(489, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem1
            // 
            this.gameToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.cancelToolStripMenuItem1,
            this.exitToolStripMenuItem1});
            this.gameToolStripMenuItem1.Name = "gameToolStripMenuItem1";
            this.gameToolStripMenuItem1.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem1.Text = "&Game";
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.newToolStripMenuItem1.Text = "&New";
            this.newToolStripMenuItem1.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // cancelToolStripMenuItem1
            // 
            this.cancelToolStripMenuItem1.Name = "cancelToolStripMenuItem1";
            this.cancelToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.cancelToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.cancelToolStripMenuItem1.Text = "Canc&el";
            this.cancelToolStripMenuItem1.Click += new System.EventHandler(this.cancelToolStripMenuItem1_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.exitToolStripMenuItem1.Text = "E&xit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showNumbersToolStripMenuItem});
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.editToolStripMenuItem1.Text = "&View";
            // 
            // showNumbersToolStripMenuItem
            // 
            this.showNumbersToolStripMenuItem.Name = "showNumbersToolStripMenuItem";
            this.showNumbersToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.showNumbersToolStripMenuItem.Text = "Show numbers";
            this.showNumbersToolStripMenuItem.Click += new System.EventHandler(this.showNumbersToolStripMenuItem_Click);
            // 
            // gameStatusLabel
            // 
            this.gameStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.gameStatusLabel.Name = "gameStatusLabel";
            this.gameStatusLabel.Size = new System.Drawing.Size(0, 17);
            this.gameStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // boardCanvas
            // 
            this.boardCanvas.DataContext = null;
            this.boardCanvas.Location = new System.Drawing.Point(14, 30);
            this.boardCanvas.Margin = new System.Windows.Forms.Padding(10);
            this.boardCanvas.Name = "boardCanvas";
            this.boardCanvas.Size = new System.Drawing.Size(460, 461);
            this.boardCanvas.TabIndex = 4;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 579);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gomoku";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.turnPicture)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private BoardCanvas boardCanvas;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox turnPicture;
        private System.Windows.Forms.Panel computerColorPanel;
        private System.Windows.Forms.Label playerBlackLabel;
        private System.Windows.Forms.Panel playerOneColorPanel;
        private System.Windows.Forms.Label playerWhiteLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel playerThink;
        private System.Windows.Forms.ToolStripMenuItem showNumbersToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel gameStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

