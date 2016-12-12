namespace Happy_Chang_Player
{
    partial class FormBasic
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLocationX = new System.Windows.Forms.TextBox();
            this.textBoxLocationY = new System.Windows.Forms.TextBox();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.buttonGotoAppLocation = new System.Windows.Forms.Button();
            this.buttonMonitoring = new System.Windows.Forms.Button();
            this.labelScreenPhysicalSize = new System.Windows.Forms.Label();
            this.textBoxScreenWidth = new System.Windows.Forms.TextBox();
            this.textBoxScreenHeight = new System.Windows.Forms.TextBox();
            this.labelScreenRatio = new System.Windows.Forms.Label();
            this.textBoxScreenRatio = new System.Windows.Forms.TextBox();
            this.textBoxMonitoringAreaHeight = new System.Windows.Forms.TextBox();
            this.textBoxMonitoringAreaWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownRows = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownColumns = new System.Windows.Forms.NumericUpDown();
            this.buttonTestFunction = new System.Windows.Forms.Button();
            this.textBoxTestInput = new System.Windows.Forms.TextBox();
            this.buttonArrangeWindows = new System.Windows.Forms.Button();
            this.buttonShutdown = new System.Windows.Forms.Button();
            this.buttonTest1 = new System.Windows.Forms.Button();
            this.buttonTest2 = new System.Windows.Forms.Button();
            this.buttonQueueSongs = new System.Windows.Forms.Button();
            this.textBoxSY = new System.Windows.Forms.TextBox();
            this.textBoxSX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxScreenRatioApply = new System.Windows.Forms.CheckBox();
            this.buttonGotoScreenLocation = new System.Windows.Forms.Button();
            this.buttonHappyChangHelper = new System.Windows.Forms.Button();
            this.groupBoxScreenInfo = new System.Windows.Forms.GroupBox();
            this.buttonGetAllProcessName = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumns)).BeginInit();
            this.groupBoxScreenInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(41, 354);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(538, 327);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 700);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Location (Relative to App):";
            // 
            // textBoxLocationX
            // 
            this.textBoxLocationX.Location = new System.Drawing.Point(286, 697);
            this.textBoxLocationX.Name = "textBoxLocationX";
            this.textBoxLocationX.Size = new System.Drawing.Size(100, 26);
            this.textBoxLocationX.TabIndex = 2;
            this.textBoxLocationX.Enter += new System.EventHandler(this.textBoxLocationX_Enter);
            // 
            // textBoxLocationY
            // 
            this.textBoxLocationY.Location = new System.Drawing.Point(392, 697);
            this.textBoxLocationY.Name = "textBoxLocationY";
            this.textBoxLocationY.Size = new System.Drawing.Size(100, 26);
            this.textBoxLocationY.TabIndex = 3;
            this.textBoxLocationY.Enter += new System.EventHandler(this.textBoxLocationY_Enter);
            // 
            // buttonCapture
            // 
            this.buttonCapture.Location = new System.Drawing.Point(864, 425);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(159, 61);
            this.buttonCapture.TabIndex = 4;
            this.buttonCapture.Text = "Screen Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonGotoAppLocation
            // 
            this.buttonGotoAppLocation.Location = new System.Drawing.Point(510, 691);
            this.buttonGotoAppLocation.Name = "buttonGotoAppLocation";
            this.buttonGotoAppLocation.Size = new System.Drawing.Size(216, 38);
            this.buttonGotoAppLocation.TabIndex = 6;
            this.buttonGotoAppLocation.Text = "Go to App Location";
            this.buttonGotoAppLocation.UseVisualStyleBackColor = true;
            this.buttonGotoAppLocation.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonMonitoring
            // 
            this.buttonMonitoring.Location = new System.Drawing.Point(864, 30);
            this.buttonMonitoring.Name = "buttonMonitoring";
            this.buttonMonitoring.Size = new System.Drawing.Size(159, 61);
            this.buttonMonitoring.TabIndex = 8;
            this.buttonMonitoring.Text = "Start Monitoring";
            this.buttonMonitoring.UseVisualStyleBackColor = true;
            this.buttonMonitoring.Click += new System.EventHandler(this.buttonMonitoring_Click);
            // 
            // labelScreenPhysicalSize
            // 
            this.labelScreenPhysicalSize.AutoSize = true;
            this.labelScreenPhysicalSize.Location = new System.Drawing.Point(11, 37);
            this.labelScreenPhysicalSize.Name = "labelScreenPhysicalSize";
            this.labelScreenPhysicalSize.Size = new System.Drawing.Size(216, 20);
            this.labelScreenPhysicalSize.TabIndex = 9;
            this.labelScreenPhysicalSize.Text = "Screen Physical Size (W x H):";
            // 
            // textBoxScreenWidth
            // 
            this.textBoxScreenWidth.Location = new System.Drawing.Point(225, 34);
            this.textBoxScreenWidth.Name = "textBoxScreenWidth";
            this.textBoxScreenWidth.Size = new System.Drawing.Size(86, 26);
            this.textBoxScreenWidth.TabIndex = 10;
            this.textBoxScreenWidth.Text = "900";
            // 
            // textBoxScreenHeight
            // 
            this.textBoxScreenHeight.Location = new System.Drawing.Point(317, 34);
            this.textBoxScreenHeight.Name = "textBoxScreenHeight";
            this.textBoxScreenHeight.Size = new System.Drawing.Size(86, 26);
            this.textBoxScreenHeight.TabIndex = 11;
            this.textBoxScreenHeight.Text = "1440";
            // 
            // labelScreenRatio
            // 
            this.labelScreenRatio.AutoSize = true;
            this.labelScreenRatio.Location = new System.Drawing.Point(447, 36);
            this.labelScreenRatio.Name = "labelScreenRatio";
            this.labelScreenRatio.Size = new System.Drawing.Size(106, 20);
            this.labelScreenRatio.TabIndex = 12;
            this.labelScreenRatio.Text = "Screen Ratio:";
            // 
            // textBoxScreenRatio
            // 
            this.textBoxScreenRatio.Enabled = false;
            this.textBoxScreenRatio.Location = new System.Drawing.Point(559, 34);
            this.textBoxScreenRatio.Name = "textBoxScreenRatio";
            this.textBoxScreenRatio.Size = new System.Drawing.Size(86, 26);
            this.textBoxScreenRatio.TabIndex = 13;
            // 
            // textBoxMonitoringAreaHeight
            // 
            this.textBoxMonitoringAreaHeight.Location = new System.Drawing.Point(317, 75);
            this.textBoxMonitoringAreaHeight.Name = "textBoxMonitoringAreaHeight";
            this.textBoxMonitoringAreaHeight.Size = new System.Drawing.Size(86, 26);
            this.textBoxMonitoringAreaHeight.TabIndex = 16;
            // 
            // textBoxMonitoringAreaWidth
            // 
            this.textBoxMonitoringAreaWidth.Location = new System.Drawing.Point(225, 75);
            this.textBoxMonitoringAreaWidth.Name = "textBoxMonitoringAreaWidth";
            this.textBoxMonitoringAreaWidth.Size = new System.Drawing.Size(86, 26);
            this.textBoxMonitoringAreaWidth.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Area of Monitoring (W x H):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(447, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "No. of Instances (H x V):";
            // 
            // numericUpDownRows
            // 
            this.numericUpDownRows.Location = new System.Drawing.Point(709, 75);
            this.numericUpDownRows.Name = "numericUpDownRows";
            this.numericUpDownRows.Size = new System.Drawing.Size(64, 26);
            this.numericUpDownRows.TabIndex = 20;
            this.numericUpDownRows.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // numericUpDownColumns
            // 
            this.numericUpDownColumns.Location = new System.Drawing.Point(633, 76);
            this.numericUpDownColumns.Name = "numericUpDownColumns";
            this.numericUpDownColumns.Size = new System.Drawing.Size(67, 26);
            this.numericUpDownColumns.TabIndex = 21;
            this.numericUpDownColumns.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // buttonTestFunction
            // 
            this.buttonTestFunction.Location = new System.Drawing.Point(864, 354);
            this.buttonTestFunction.Name = "buttonTestFunction";
            this.buttonTestFunction.Size = new System.Drawing.Size(159, 65);
            this.buttonTestFunction.TabIndex = 24;
            this.buttonTestFunction.Text = "Test Function";
            this.buttonTestFunction.UseVisualStyleBackColor = true;
            this.buttonTestFunction.Click += new System.EventHandler(this.buttonTestFunction_Click);
            // 
            // textBoxTestInput
            // 
            this.textBoxTestInput.Location = new System.Drawing.Point(864, 322);
            this.textBoxTestInput.Name = "textBoxTestInput";
            this.textBoxTestInput.Size = new System.Drawing.Size(159, 26);
            this.textBoxTestInput.TabIndex = 25;
            // 
            // buttonArrangeWindows
            // 
            this.buttonArrangeWindows.Location = new System.Drawing.Point(864, 111);
            this.buttonArrangeWindows.Name = "buttonArrangeWindows";
            this.buttonArrangeWindows.Size = new System.Drawing.Size(159, 58);
            this.buttonArrangeWindows.TabIndex = 26;
            this.buttonArrangeWindows.Text = "Arrange Windows";
            this.buttonArrangeWindows.UseVisualStyleBackColor = true;
            this.buttonArrangeWindows.Click += new System.EventHandler(this.buttonArrangeWindow_Click);
            // 
            // buttonShutdown
            // 
            this.buttonShutdown.Location = new System.Drawing.Point(864, 193);
            this.buttonShutdown.Name = "buttonShutdown";
            this.buttonShutdown.Size = new System.Drawing.Size(159, 65);
            this.buttonShutdown.TabIndex = 27;
            this.buttonShutdown.Text = "Schedule Shutdown";
            this.buttonShutdown.UseVisualStyleBackColor = true;
            // 
            // buttonTest1
            // 
            this.buttonTest1.Location = new System.Drawing.Point(629, 354);
            this.buttonTest1.Name = "buttonTest1";
            this.buttonTest1.Size = new System.Drawing.Size(138, 65);
            this.buttonTest1.TabIndex = 28;
            this.buttonTest1.Text = "Test1";
            this.buttonTest1.UseVisualStyleBackColor = true;
            this.buttonTest1.Click += new System.EventHandler(this.buttonTest1_Click);
            // 
            // buttonTest2
            // 
            this.buttonTest2.Location = new System.Drawing.Point(629, 445);
            this.buttonTest2.Name = "buttonTest2";
            this.buttonTest2.Size = new System.Drawing.Size(138, 65);
            this.buttonTest2.TabIndex = 29;
            this.buttonTest2.Text = "Test2";
            this.buttonTest2.UseVisualStyleBackColor = true;
            this.buttonTest2.Click += new System.EventHandler(this.buttonTest2_Click);
            // 
            // buttonQueueSongs
            // 
            this.buttonQueueSongs.Location = new System.Drawing.Point(629, 534);
            this.buttonQueueSongs.Name = "buttonQueueSongs";
            this.buttonQueueSongs.Size = new System.Drawing.Size(138, 62);
            this.buttonQueueSongs.TabIndex = 30;
            this.buttonQueueSongs.Text = "Queue Song";
            this.buttonQueueSongs.UseVisualStyleBackColor = true;
            this.buttonQueueSongs.Click += new System.EventHandler(this.buttonQueueSongs_Click);
            // 
            // textBoxSY
            // 
            this.textBoxSY.Location = new System.Drawing.Point(392, 744);
            this.textBoxSY.Name = "textBoxSY";
            this.textBoxSY.Size = new System.Drawing.Size(100, 26);
            this.textBoxSY.TabIndex = 33;
            this.textBoxSY.Enter += new System.EventHandler(this.textBoxSY_Enter);
            // 
            // textBoxSX
            // 
            this.textBoxSX.Location = new System.Drawing.Point(286, 744);
            this.textBoxSX.Name = "textBoxSX";
            this.textBoxSX.Size = new System.Drawing.Size(100, 26);
            this.textBoxSX.TabIndex = 32;
            this.textBoxSX.Enter += new System.EventHandler(this.textBoxSX_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(154, 747);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 20);
            this.label5.TabIndex = 31;
            this.label5.Text = "Screen Location:";
            // 
            // checkBoxScreenRatioApply
            // 
            this.checkBoxScreenRatioApply.AutoSize = true;
            this.checkBoxScreenRatioApply.Location = new System.Drawing.Point(286, 780);
            this.checkBoxScreenRatioApply.Name = "checkBoxScreenRatioApply";
            this.checkBoxScreenRatioApply.Size = new System.Drawing.Size(185, 24);
            this.checkBoxScreenRatioApply.TabIndex = 34;
            this.checkBoxScreenRatioApply.Text = "Screen Ratio Applied";
            this.checkBoxScreenRatioApply.UseVisualStyleBackColor = true;
            // 
            // buttonGotoScreenLocation
            // 
            this.buttonGotoScreenLocation.Location = new System.Drawing.Point(510, 738);
            this.buttonGotoScreenLocation.Name = "buttonGotoScreenLocation";
            this.buttonGotoScreenLocation.Size = new System.Drawing.Size(216, 38);
            this.buttonGotoScreenLocation.TabIndex = 35;
            this.buttonGotoScreenLocation.Text = "Go to Screen Location";
            this.buttonGotoScreenLocation.UseVisualStyleBackColor = true;
            this.buttonGotoScreenLocation.Click += new System.EventHandler(this.buttonGotoScreenLocation_Click);
            // 
            // buttonHappyChangHelper
            // 
            this.buttonHappyChangHelper.Location = new System.Drawing.Point(864, 691);
            this.buttonHappyChangHelper.Name = "buttonHappyChangHelper";
            this.buttonHappyChangHelper.Size = new System.Drawing.Size(159, 67);
            this.buttonHappyChangHelper.TabIndex = 36;
            this.buttonHappyChangHelper.Text = "Happy Chang Helper";
            this.buttonHappyChangHelper.UseVisualStyleBackColor = true;
            this.buttonHappyChangHelper.Click += new System.EventHandler(this.buttonHappyChangHelper_Click);
            // 
            // groupBoxScreenInfo
            // 
            this.groupBoxScreenInfo.Controls.Add(this.labelScreenPhysicalSize);
            this.groupBoxScreenInfo.Controls.Add(this.textBoxScreenWidth);
            this.groupBoxScreenInfo.Controls.Add(this.textBoxScreenHeight);
            this.groupBoxScreenInfo.Controls.Add(this.labelScreenRatio);
            this.groupBoxScreenInfo.Controls.Add(this.textBoxScreenRatio);
            this.groupBoxScreenInfo.Controls.Add(this.label2);
            this.groupBoxScreenInfo.Controls.Add(this.textBoxMonitoringAreaWidth);
            this.groupBoxScreenInfo.Controls.Add(this.textBoxMonitoringAreaHeight);
            this.groupBoxScreenInfo.Controls.Add(this.label3);
            this.groupBoxScreenInfo.Controls.Add(this.numericUpDownRows);
            this.groupBoxScreenInfo.Controls.Add(this.numericUpDownColumns);
            this.groupBoxScreenInfo.Location = new System.Drawing.Point(26, 13);
            this.groupBoxScreenInfo.Name = "groupBoxScreenInfo";
            this.groupBoxScreenInfo.Size = new System.Drawing.Size(793, 145);
            this.groupBoxScreenInfo.TabIndex = 37;
            this.groupBoxScreenInfo.TabStop = false;
            this.groupBoxScreenInfo.Text = "Screen Information";
            // 
            // buttonGetAllProcessName
            // 
            this.buttonGetAllProcessName.Location = new System.Drawing.Point(130, 193);
            this.buttonGetAllProcessName.Name = "buttonGetAllProcessName";
            this.buttonGetAllProcessName.Size = new System.Drawing.Size(207, 53);
            this.buttonGetAllProcessName.TabIndex = 38;
            this.buttonGetAllProcessName.Text = "Get All Process Names";
            this.buttonGetAllProcessName.UseVisualStyleBackColor = true;
            this.buttonGetAllProcessName.Click += new System.EventHandler(this.buttonGetAllProcessName_Click);
            // 
            // FormBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 838);
            this.Controls.Add(this.buttonGetAllProcessName);
            this.Controls.Add(this.groupBoxScreenInfo);
            this.Controls.Add(this.buttonHappyChangHelper);
            this.Controls.Add(this.buttonGotoScreenLocation);
            this.Controls.Add(this.checkBoxScreenRatioApply);
            this.Controls.Add(this.textBoxSY);
            this.Controls.Add(this.textBoxSX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonQueueSongs);
            this.Controls.Add(this.buttonTest2);
            this.Controls.Add(this.buttonTest1);
            this.Controls.Add(this.buttonShutdown);
            this.Controls.Add(this.buttonArrangeWindows);
            this.Controls.Add(this.textBoxTestInput);
            this.Controls.Add(this.buttonTestFunction);
            this.Controls.Add(this.buttonMonitoring);
            this.Controls.Add(this.buttonGotoAppLocation);
            this.Controls.Add(this.buttonCapture);
            this.Controls.Add(this.textBoxLocationY);
            this.Controls.Add(this.textBoxLocationX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox);
            this.Name = "FormBasic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Development Tools";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumns)).EndInit();
            this.groupBoxScreenInfo.ResumeLayout(false);
            this.groupBoxScreenInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLocationX;
        private System.Windows.Forms.TextBox textBoxLocationY;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.Button buttonGotoAppLocation;
        private System.Windows.Forms.Button buttonMonitoring;
        private System.Windows.Forms.Label labelScreenPhysicalSize;
        private System.Windows.Forms.TextBox textBoxScreenWidth;
        private System.Windows.Forms.TextBox textBoxScreenHeight;
        private System.Windows.Forms.Label labelScreenRatio;
        private System.Windows.Forms.TextBox textBoxScreenRatio;
        private System.Windows.Forms.TextBox textBoxMonitoringAreaHeight;
        private System.Windows.Forms.TextBox textBoxMonitoringAreaWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownRows;
        private System.Windows.Forms.NumericUpDown numericUpDownColumns;
        private System.Windows.Forms.Button buttonTestFunction;
        private System.Windows.Forms.TextBox textBoxTestInput;
        private System.Windows.Forms.Button buttonArrangeWindows;
        private System.Windows.Forms.Button buttonShutdown;
        private System.Windows.Forms.Button buttonTest1;
        private System.Windows.Forms.Button buttonTest2;
        private System.Windows.Forms.Button buttonQueueSongs;
        private System.Windows.Forms.TextBox textBoxSY;
        private System.Windows.Forms.TextBox textBoxSX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxScreenRatioApply;
        private System.Windows.Forms.Button buttonGotoScreenLocation;
        private System.Windows.Forms.Button buttonHappyChangHelper;
        private System.Windows.Forms.GroupBox groupBoxScreenInfo;
        private System.Windows.Forms.Button buttonGetAllProcessName;
    }
}

