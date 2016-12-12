namespace Happy_Chang_Player
{
    partial class FormHappyChangPlayer
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
            this.buttonBasicWin = new System.Windows.Forms.Button();
            this.buttonDevTools = new System.Windows.Forms.Button();
            this.groupBoxRegion = new System.Windows.Forms.GroupBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.textBoxOffsetY = new System.Windows.Forms.TextBox();
            this.textBoxOffsetX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxScreenCap = new System.Windows.Forms.PictureBox();
            this.buttonRegionCapture = new System.Windows.Forms.Button();
            this.buttonMonitoring = new System.Windows.Forms.Button();
            this.trackBarWidth = new System.Windows.Forms.TrackBar();
            this.trackBarHeight = new System.Windows.Forms.TrackBar();
            this.trackBarAppScale = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.labelAppScale = new System.Windows.Forms.Label();
            this.labelClickDelay = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarClickDelay = new System.Windows.Forms.TrackBar();
            this.labelClicksPerBatch = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBarClicksPerBatch = new System.Windows.Forms.TrackBar();
            this.groupBoxRegion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreenCap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAppScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarClickDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarClicksPerBatch)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonBasicWin
            // 
            this.buttonBasicWin.Location = new System.Drawing.Point(956, 57);
            this.buttonBasicWin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonBasicWin.Name = "buttonBasicWin";
            this.buttonBasicWin.Size = new System.Drawing.Size(195, 56);
            this.buttonBasicWin.TabIndex = 0;
            this.buttonBasicWin.Text = "Open Basic Window";
            this.buttonBasicWin.UseVisualStyleBackColor = true;
            this.buttonBasicWin.Click += new System.EventHandler(this.buttonBasicWin_Click);
            // 
            // buttonDevTools
            // 
            this.buttonDevTools.Location = new System.Drawing.Point(956, 745);
            this.buttonDevTools.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDevTools.Name = "buttonDevTools";
            this.buttonDevTools.Size = new System.Drawing.Size(195, 56);
            this.buttonDevTools.TabIndex = 1;
            this.buttonDevTools.Text = "Development Tools";
            this.buttonDevTools.UseVisualStyleBackColor = true;
            // 
            // groupBoxRegion
            // 
            this.groupBoxRegion.Controls.Add(this.textBoxHeight);
            this.groupBoxRegion.Controls.Add(this.textBoxWidth);
            this.groupBoxRegion.Controls.Add(this.textBoxOffsetY);
            this.groupBoxRegion.Controls.Add(this.textBoxOffsetX);
            this.groupBoxRegion.Controls.Add(this.label1);
            this.groupBoxRegion.Location = new System.Drawing.Point(25, 27);
            this.groupBoxRegion.Name = "groupBoxRegion";
            this.groupBoxRegion.Size = new System.Drawing.Size(700, 86);
            this.groupBoxRegion.TabIndex = 2;
            this.groupBoxRegion.TabStop = false;
            this.groupBoxRegion.Text = "Monitoring Region";
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(561, 29);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(100, 26);
            this.textBoxHeight.TabIndex = 4;
            this.textBoxHeight.TextChanged += new System.EventHandler(this.textBoxHeight_TextChanged);
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(442, 29);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(100, 26);
            this.textBoxWidth.TabIndex = 3;
            this.textBoxWidth.TextChanged += new System.EventHandler(this.textBoxWidth_TextChanged);
            // 
            // textBoxOffsetY
            // 
            this.textBoxOffsetY.Location = new System.Drawing.Point(326, 29);
            this.textBoxOffsetY.Name = "textBoxOffsetY";
            this.textBoxOffsetY.Size = new System.Drawing.Size(100, 26);
            this.textBoxOffsetY.TabIndex = 2;
            this.textBoxOffsetY.TextChanged += new System.EventHandler(this.textBoxOffsetY_TextChanged);
            // 
            // textBoxOffsetX
            // 
            this.textBoxOffsetX.Location = new System.Drawing.Point(209, 30);
            this.textBoxOffsetX.Name = "textBoxOffsetX";
            this.textBoxOffsetX.Size = new System.Drawing.Size(100, 26);
            this.textBoxOffsetX.TabIndex = 1;
            this.textBoxOffsetX.TextChanged += new System.EventHandler(this.textBoxOffsetX_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "(X, Y, Width, Height): ";
            // 
            // pictureBoxScreenCap
            // 
            this.pictureBoxScreenCap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxScreenCap.Location = new System.Drawing.Point(46, 303);
            this.pictureBoxScreenCap.Name = "pictureBoxScreenCap";
            this.pictureBoxScreenCap.Size = new System.Drawing.Size(576, 350);
            this.pictureBoxScreenCap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxScreenCap.TabIndex = 3;
            this.pictureBoxScreenCap.TabStop = false;
            // 
            // buttonRegionCapture
            // 
            this.buttonRegionCapture.Location = new System.Drawing.Point(770, 237);
            this.buttonRegionCapture.Name = "buttonRegionCapture";
            this.buttonRegionCapture.Size = new System.Drawing.Size(192, 53);
            this.buttonRegionCapture.TabIndex = 4;
            this.buttonRegionCapture.Text = "Region Capture";
            this.buttonRegionCapture.UseVisualStyleBackColor = true;
            this.buttonRegionCapture.Click += new System.EventHandler(this.buttonRegionCapture_Click);
            // 
            // buttonMonitoring
            // 
            this.buttonMonitoring.Location = new System.Drawing.Point(770, 312);
            this.buttonMonitoring.Name = "buttonMonitoring";
            this.buttonMonitoring.Size = new System.Drawing.Size(192, 67);
            this.buttonMonitoring.TabIndex = 5;
            this.buttonMonitoring.Text = "Start Monitoring";
            this.buttonMonitoring.UseVisualStyleBackColor = true;
            this.buttonMonitoring.Click += new System.EventHandler(this.buttonMonitoring_Click);
            // 
            // trackBarWidth
            // 
            this.trackBarWidth.LargeChange = 200;
            this.trackBarWidth.Location = new System.Drawing.Point(46, 675);
            this.trackBarWidth.Name = "trackBarWidth";
            this.trackBarWidth.Size = new System.Drawing.Size(576, 69);
            this.trackBarWidth.SmallChange = 50;
            this.trackBarWidth.TabIndex = 6;
            this.trackBarWidth.TickFrequency = 100;
            this.trackBarWidth.Scroll += new System.EventHandler(this.trackBarWidth_Scroll);
            // 
            // trackBarHeight
            // 
            this.trackBarHeight.LargeChange = 200;
            this.trackBarHeight.Location = new System.Drawing.Point(628, 303);
            this.trackBarHeight.Maximum = 100;
            this.trackBarHeight.Minimum = 100;
            this.trackBarHeight.Name = "trackBarHeight";
            this.trackBarHeight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarHeight.Size = new System.Drawing.Size(69, 350);
            this.trackBarHeight.SmallChange = 50;
            this.trackBarHeight.TabIndex = 9;
            this.trackBarHeight.TickFrequency = 100;
            this.trackBarHeight.Value = 100;
            this.trackBarHeight.Scroll += new System.EventHandler(this.trackBarHeight_Scroll);
            // 
            // trackBarAppScale
            // 
            this.trackBarAppScale.LargeChange = 10;
            this.trackBarAppScale.Location = new System.Drawing.Point(190, 133);
            this.trackBarAppScale.Maximum = 200;
            this.trackBarAppScale.Minimum = 10;
            this.trackBarAppScale.Name = "trackBarAppScale";
            this.trackBarAppScale.Size = new System.Drawing.Size(193, 69);
            this.trackBarAppScale.SmallChange = 5;
            this.trackBarAppScale.TabIndex = 6;
            this.trackBarAppScale.TickFrequency = 25;
            this.trackBarAppScale.Value = 10;
            this.trackBarAppScale.Scroll += new System.EventHandler(this.trackBarAppScale_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Application Scale";
            // 
            // labelAppScale
            // 
            this.labelAppScale.AutoSize = true;
            this.labelAppScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAppScale.Location = new System.Drawing.Point(395, 133);
            this.labelAppScale.Name = "labelAppScale";
            this.labelAppScale.Size = new System.Drawing.Size(63, 25);
            this.labelAppScale.TabIndex = 8;
            this.labelAppScale.Text = "100%";
            // 
            // labelClickDelay
            // 
            this.labelClickDelay.AutoSize = true;
            this.labelClickDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClickDelay.Location = new System.Drawing.Point(395, 182);
            this.labelClickDelay.Name = "labelClickDelay";
            this.labelClickDelay.Size = new System.Drawing.Size(60, 25);
            this.labelClickDelay.TabIndex = 12;
            this.labelClickDelay.Text = "30ms";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Delay Per Click";
            // 
            // trackBarClickDelay
            // 
            this.trackBarClickDelay.LargeChange = 10;
            this.trackBarClickDelay.Location = new System.Drawing.Point(190, 182);
            this.trackBarClickDelay.Maximum = 1000;
            this.trackBarClickDelay.Name = "trackBarClickDelay";
            this.trackBarClickDelay.Size = new System.Drawing.Size(193, 69);
            this.trackBarClickDelay.SmallChange = 5;
            this.trackBarClickDelay.TabIndex = 10;
            this.trackBarClickDelay.TickFrequency = 20;
            this.trackBarClickDelay.Value = 10;
            this.trackBarClickDelay.Scroll += new System.EventHandler(this.trackBarClickDelay_Scroll);
            // 
            // labelClicksPerBatch
            // 
            this.labelClicksPerBatch.AutoSize = true;
            this.labelClicksPerBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClicksPerBatch.Location = new System.Drawing.Point(395, 237);
            this.labelClicksPerBatch.Name = "labelClicksPerBatch";
            this.labelClicksPerBatch.Size = new System.Drawing.Size(34, 25);
            this.labelClicksPerBatch.TabIndex = 15;
            this.labelClicksPerBatch.Text = "12";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Clicks per Batch";
            // 
            // trackBarClicksPerBatch
            // 
            this.trackBarClicksPerBatch.Location = new System.Drawing.Point(190, 237);
            this.trackBarClicksPerBatch.Maximum = 20;
            this.trackBarClicksPerBatch.Minimum = 1;
            this.trackBarClicksPerBatch.Name = "trackBarClicksPerBatch";
            this.trackBarClicksPerBatch.Size = new System.Drawing.Size(193, 69);
            this.trackBarClicksPerBatch.SmallChange = 5;
            this.trackBarClicksPerBatch.TabIndex = 13;
            this.trackBarClicksPerBatch.TickFrequency = 20;
            this.trackBarClicksPerBatch.Value = 10;
            this.trackBarClicksPerBatch.Scroll += new System.EventHandler(this.trackBarClicksPerBatch_Scroll);
            // 
            // FormHappyChangPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1306, 874);
            this.Controls.Add(this.pictureBoxScreenCap);
            this.Controls.Add(this.labelClicksPerBatch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.trackBarClicksPerBatch);
            this.Controls.Add(this.labelClickDelay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackBarClickDelay);
            this.Controls.Add(this.trackBarHeight);
            this.Controls.Add(this.labelAppScale);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBarAppScale);
            this.Controls.Add(this.trackBarWidth);
            this.Controls.Add(this.buttonMonitoring);
            this.Controls.Add(this.buttonRegionCapture);
            this.Controls.Add(this.groupBoxRegion);
            this.Controls.Add(this.buttonDevTools);
            this.Controls.Add(this.buttonBasicWin);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormHappyChangPlayer";
            this.Text = "Happy Chang Player";
            this.groupBoxRegion.ResumeLayout(false);
            this.groupBoxRegion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreenCap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAppScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarClickDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarClicksPerBatch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBasicWin;
        private System.Windows.Forms.Button buttonDevTools;
        private System.Windows.Forms.GroupBox groupBoxRegion;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.TextBox textBoxOffsetY;
        private System.Windows.Forms.TextBox textBoxOffsetX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxScreenCap;
        private System.Windows.Forms.Button buttonRegionCapture;
        private System.Windows.Forms.Button buttonMonitoring;
        private System.Windows.Forms.TrackBar trackBarHeight;
        private System.Windows.Forms.TrackBar trackBarWidth;
        private System.Windows.Forms.TrackBar trackBarAppScale;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelAppScale;
        private System.Windows.Forms.Label labelClickDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarClickDelay;
        private System.Windows.Forms.Label labelClicksPerBatch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBarClicksPerBatch;
    }
}

