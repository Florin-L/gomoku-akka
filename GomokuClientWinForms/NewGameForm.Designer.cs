namespace GomokuClient
{
    partial class NewGameForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.whitePlayerName = new System.Windows.Forms.ComboBox();
            this.blackPlayerName = new System.Windows.Forms.ComboBox();
            this.whiteSearchDepth = new System.Windows.Forms.NumericUpDown();
            this.blackSearchDepth = new System.Windows.Forms.NumericUpDown();
            this.whiteTimeLimit = new System.Windows.Forms.NumericUpDown();
            this.blackTimeLimit = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSwitchSides = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.whiteSearchDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackSearchDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.whiteTimeLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackTimeLimit)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.whitePlayerName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.blackPlayerName, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.whiteSearchDepth, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.blackSearchDepth, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.whiteTimeLimit, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.blackTimeLimit, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(475, 169);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Player Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(240, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 42);
            this.label2.TabIndex = 1;
            this.label2.Text = "Search Depth";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(358, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 42);
            this.label3.TabIndex = 2;
            this.label3.Text = "Time Limit";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // whitePlayerName
            // 
            this.whitePlayerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whitePlayerName.FormattingEnabled = true;
            this.whitePlayerName.Items.AddRange(new object[] {
            "Human",
            "Computer"});
            this.whitePlayerName.Location = new System.Drawing.Point(5, 47);
            this.whitePlayerName.Margin = new System.Windows.Forms.Padding(5);
            this.whitePlayerName.Name = "whitePlayerName";
            this.whitePlayerName.Size = new System.Drawing.Size(227, 21);
            this.whitePlayerName.TabIndex = 0;
            // 
            // blackPlayerName
            // 
            this.blackPlayerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blackPlayerName.FormattingEnabled = true;
            this.blackPlayerName.Items.AddRange(new object[] {
            "Human",
            "Computer"});
            this.blackPlayerName.Location = new System.Drawing.Point(5, 89);
            this.blackPlayerName.Margin = new System.Windows.Forms.Padding(5);
            this.blackPlayerName.Name = "blackPlayerName";
            this.blackPlayerName.Size = new System.Drawing.Size(227, 21);
            this.blackPlayerName.TabIndex = 3;
            // 
            // whiteSearchDepth
            // 
            this.whiteSearchDepth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whiteSearchDepth.Enabled = false;
            this.whiteSearchDepth.Location = new System.Drawing.Point(242, 47);
            this.whiteSearchDepth.Margin = new System.Windows.Forms.Padding(5);
            this.whiteSearchDepth.Name = "whiteSearchDepth";
            this.whiteSearchDepth.Size = new System.Drawing.Size(108, 20);
            this.whiteSearchDepth.TabIndex = 1;
            this.whiteSearchDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // blackSearchDepth
            // 
            this.blackSearchDepth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blackSearchDepth.Location = new System.Drawing.Point(242, 89);
            this.blackSearchDepth.Margin = new System.Windows.Forms.Padding(5);
            this.blackSearchDepth.Name = "blackSearchDepth";
            this.blackSearchDepth.Size = new System.Drawing.Size(108, 20);
            this.blackSearchDepth.TabIndex = 4;
            this.blackSearchDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // whiteTimeLimit
            // 
            this.whiteTimeLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whiteTimeLimit.Location = new System.Drawing.Point(360, 47);
            this.whiteTimeLimit.Margin = new System.Windows.Forms.Padding(5);
            this.whiteTimeLimit.Name = "whiteTimeLimit";
            this.whiteTimeLimit.Size = new System.Drawing.Size(110, 20);
            this.whiteTimeLimit.TabIndex = 2;
            this.whiteTimeLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // blackTimeLimit
            // 
            this.blackTimeLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blackTimeLimit.Location = new System.Drawing.Point(360, 89);
            this.blackTimeLimit.Margin = new System.Windows.Forms.Padding(5);
            this.blackTimeLimit.Name = "blackTimeLimit";
            this.blackTimeLimit.Size = new System.Drawing.Size(110, 20);
            this.blackTimeLimit.TabIndex = 5;
            this.blackTimeLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 3);
            this.panel1.Controls.Add(this.btnSwitchSides);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 129);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(469, 37);
            this.panel1.TabIndex = 9;
            // 
            // btnSwitchSides
            // 
            this.btnSwitchSides.Location = new System.Drawing.Point(290, 8);
            this.btnSwitchSides.Name = "btnSwitchSides";
            this.btnSwitchSides.Size = new System.Drawing.Size(75, 23);
            this.btnSwitchSides.TabIndex = 8;
            this.btnSwitchSides.Text = "S&witch sides";
            this.btnSwitchSides.UseVisualStyleBackColor = true;
            this.btnSwitchSides.Click += new System.EventHandler(this.btnSwitchSides_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStart
            // 
            this.btnStart.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnStart.Location = new System.Drawing.Point(104, 8);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // NewGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 169);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "NewGameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New game";
            this.Load += new System.EventHandler(this.NewGameForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.whiteSearchDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackSearchDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.whiteTimeLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackTimeLimit)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox whitePlayerName;
        private System.Windows.Forms.ComboBox blackPlayerName;
        private System.Windows.Forms.NumericUpDown whiteSearchDepth;
        private System.Windows.Forms.NumericUpDown blackSearchDepth;
        private System.Windows.Forms.NumericUpDown whiteTimeLimit;
        private System.Windows.Forms.NumericUpDown blackTimeLimit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSwitchSides;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStart;
    }
}