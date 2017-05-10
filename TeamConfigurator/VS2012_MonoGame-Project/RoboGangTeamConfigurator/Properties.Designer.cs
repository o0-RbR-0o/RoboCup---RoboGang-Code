namespace RoboGangTeamConfigurator
{
    partial class Properties
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryFormatToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.textFormatToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maskedTextBoxPositionY = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxPositionX = new System.Windows.Forms.MaskedTextBox();
            this.comboBoxPlayers = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(104, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Move";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(6, 106);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Rotate";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "You can also manually edit this",
            "Default",
            "Renegade",
            "RenegadeOld",
            "RenegadeNew",
            "LazyKeeper",
            "AdvancedKeeper"});
            this.comboBox1.Location = new System.Drawing.Point(6, 79);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(190, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Personality:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(227, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToFileToolStripMenuItem,
            this.loadFromFileToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToFileToolStripMenuItem
            // 
            this.saveToFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.binaryFormatToolStripMenuItem,
            this.textFormatToolStripMenuItem});
            this.saveToFileToolStripMenuItem.Name = "saveToFileToolStripMenuItem";
            this.saveToFileToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.saveToFileToolStripMenuItem.Text = "Save to file...";
            // 
            // binaryFormatToolStripMenuItem
            // 
            this.binaryFormatToolStripMenuItem.Name = "binaryFormatToolStripMenuItem";
            this.binaryFormatToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.binaryFormatToolStripMenuItem.Text = "Binary Format";
            this.binaryFormatToolStripMenuItem.Click += new System.EventHandler(this.binaryFormatToolStripMenuItem_Click);
            // 
            // textFormatToolStripMenuItem
            // 
            this.textFormatToolStripMenuItem.Name = "textFormatToolStripMenuItem";
            this.textFormatToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.textFormatToolStripMenuItem.Text = "Text Format";
            this.textFormatToolStripMenuItem.Click += new System.EventHandler(this.textFormatToolStripMenuItem_Click);
            // 
            // loadFromFileToolStripMenuItem
            // 
            this.loadFromFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.binaryFormatToolStripMenuItem1,
            this.textFormatToolStripMenuItem1});
            this.loadFromFileToolStripMenuItem.Name = "loadFromFileToolStripMenuItem";
            this.loadFromFileToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.loadFromFileToolStripMenuItem.Text = "Load from file...";
            // 
            // binaryFormatToolStripMenuItem1
            // 
            this.binaryFormatToolStripMenuItem1.Name = "binaryFormatToolStripMenuItem1";
            this.binaryFormatToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.binaryFormatToolStripMenuItem1.Text = "Binary Format";
            this.binaryFormatToolStripMenuItem1.Click += new System.EventHandler(this.binaryFormatToolStripMenuItem1_Click);
            // 
            // textFormatToolStripMenuItem1
            // 
            this.textFormatToolStripMenuItem1.Name = "textFormatToolStripMenuItem1";
            this.textFormatToolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.textFormatToolStripMenuItem1.Text = "Text Format";
            this.textFormatToolStripMenuItem1.Click += new System.EventHandler(this.textFormatToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.maskedTextBoxPositionY);
            this.groupBox1.Controls.Add(this.maskedTextBoxPositionX);
            this.groupBox1.Controls.Add(this.comboBoxPlayers);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 208);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // maskedTextBoxPositionY
            // 
            this.maskedTextBoxPositionY.Enabled = false;
            this.maskedTextBoxPositionY.Location = new System.Drawing.Point(104, 177);
            this.maskedTextBoxPositionY.Name = "maskedTextBoxPositionY";
            this.maskedTextBoxPositionY.Size = new System.Drawing.Size(92, 20);
            this.maskedTextBoxPositionY.TabIndex = 8;
            // 
            // maskedTextBoxPositionX
            // 
            this.maskedTextBoxPositionX.Enabled = false;
            this.maskedTextBoxPositionX.Location = new System.Drawing.Point(9, 177);
            this.maskedTextBoxPositionX.Name = "maskedTextBoxPositionX";
            this.maskedTextBoxPositionX.Size = new System.Drawing.Size(89, 20);
            this.maskedTextBoxPositionX.TabIndex = 8;
            // 
            // comboBoxPlayers
            // 
            this.comboBoxPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlayers.FormattingEnabled = true;
            this.comboBoxPlayers.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11"});
            this.comboBoxPlayers.Location = new System.Drawing.Point(9, 36);
            this.comboBoxPlayers.MaxDropDownItems = 11;
            this.comboBoxPlayers.Name = "comboBoxPlayers";
            this.comboBoxPlayers.Size = new System.Drawing.Size(187, 21);
            this.comboBoxPlayers.TabIndex = 3;
            this.comboBoxPlayers.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlayers_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(101, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Position Y:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Position X:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(104, 135);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(92, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Make Goalie";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 135);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(92, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Make Player";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Player Select:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 30;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Properties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 249);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Properties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Properties_FormClosed);
            this.Load += new System.EventHandler(this.Properties_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPositionY;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPositionX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxPlayers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem binaryFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binaryFormatToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem textFormatToolStripMenuItem1;

    }
}