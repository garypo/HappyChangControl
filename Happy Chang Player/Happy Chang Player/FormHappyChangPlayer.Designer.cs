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
            this.SuspendLayout();
            // 
            // buttonBasicWin
            // 
            this.buttonBasicWin.Location = new System.Drawing.Point(637, 37);
            this.buttonBasicWin.Name = "buttonBasicWin";
            this.buttonBasicWin.Size = new System.Drawing.Size(75, 23);
            this.buttonBasicWin.TabIndex = 0;
            this.buttonBasicWin.Text = "Open Basic Window";
            this.buttonBasicWin.UseVisualStyleBackColor = true;
            this.buttonBasicWin.Click += new System.EventHandler(this.buttonBasicWin_Click);
            // 
            // FormHappyChangPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 568);
            this.Controls.Add(this.buttonBasicWin);
            this.Name = "FormHappyChangPlayer";
            this.Text = "Happy Chang Player";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonBasicWin;
    }
}

