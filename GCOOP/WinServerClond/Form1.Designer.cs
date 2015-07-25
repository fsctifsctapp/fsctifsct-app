namespace WinServerClond
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.BtStart = new System.Windows.Forms.Button();
            this.LbTopIP = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 34);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(644, 295);
            this.dataGridView1.TabIndex = 0;
            // 
            // BtStart
            // 
            this.BtStart.Location = new System.Drawing.Point(13, 5);
            this.BtStart.Name = "BtStart";
            this.BtStart.Size = new System.Drawing.Size(75, 23);
            this.BtStart.TabIndex = 1;
            this.BtStart.Text = "Start";
            this.BtStart.UseVisualStyleBackColor = true;
            this.BtStart.Click += new System.EventHandler(this.BtStart_Click);
            // 
            // LbTopIP
            // 
            this.LbTopIP.AutoSize = true;
            this.LbTopIP.Location = new System.Drawing.Point(94, 10);
            this.LbTopIP.Name = "LbTopIP";
            this.LbTopIP.Size = new System.Drawing.Size(43, 13);
            this.LbTopIP.TabIndex = 2;
            this.LbTopIP.Text = "Top ip: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 341);
            this.Controls.Add(this.LbTopIP);
            this.Controls.Add(this.BtStart);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Win Server Clond : GCOOP -  Isocare Systems.";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BtStart;
        private System.Windows.Forms.Label LbTopIP;
    }
}

