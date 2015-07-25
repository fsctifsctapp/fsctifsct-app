namespace WinReport
{
    partial class WinReportForm
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
            this.tbText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.BackColor = System.Drawing.Color.DarkOrange;
            this.tbText.ForeColor = System.Drawing.Color.Yellow;
            this.tbText.Location = new System.Drawing.Point(2, 2);
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.ReadOnly = true;
            this.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbText.Size = new System.Drawing.Size(739, 370);
            this.tbText.TabIndex = 0;
            this.tbText.Text = "WinReport Server";
            // 
            // WinReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 373);
            this.Controls.Add(this.tbText);
            this.Name = "WinReportForm";
            this.Text = "WinReport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MultiSocket_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbText;
    }
}

