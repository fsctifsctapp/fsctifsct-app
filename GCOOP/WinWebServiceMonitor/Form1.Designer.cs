namespace WinWebServiceMonitor
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
            this.label1 = new System.Windows.Forms.Label();
            this.TbIp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TbSite = new System.Windows.Forms.TextBox();
            this.TbAppPool = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TbPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TbPrivate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TbVirtual = new System.Windows.Forms.TextBox();
            this.TbText = new System.Windows.Forms.TextBox();
            this.BtStart = new System.Windows.Forms.Button();
            this.BtReApp = new System.Windows.Forms.Button();
            this.BtReIIS = new System.Windows.Forms.Button();
            this.LbStatus = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TbMode = new System.Windows.Forms.TextBox();
            this.TbStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ws IP";
            // 
            // TbIp
            // 
            this.TbIp.BackColor = System.Drawing.Color.FloralWhite;
            this.TbIp.Location = new System.Drawing.Point(56, 14);
            this.TbIp.Name = "TbIp";
            this.TbIp.ReadOnly = true;
            this.TbIp.Size = new System.Drawing.Size(81, 20);
            this.TbIp.TabIndex = 1;
            this.TbIp.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Site";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(291, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "App pool";
            // 
            // TbSite
            // 
            this.TbSite.BackColor = System.Drawing.Color.FloralWhite;
            this.TbSite.Location = new System.Drawing.Point(204, 14);
            this.TbSite.Name = "TbSite";
            this.TbSite.ReadOnly = true;
            this.TbSite.Size = new System.Drawing.Size(77, 20);
            this.TbSite.TabIndex = 4;
            this.TbSite.Text = "CAT";
            // 
            // TbAppPool
            // 
            this.TbAppPool.BackColor = System.Drawing.Color.FloralWhite;
            this.TbAppPool.Location = new System.Drawing.Point(347, 14);
            this.TbAppPool.Name = "TbAppPool";
            this.TbAppPool.ReadOnly = true;
            this.TbAppPool.Size = new System.Drawing.Size(90, 20);
            this.TbAppPool.TabIndex = 5;
            this.TbAppPool.Text = "WebService";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Port";
            // 
            // TbPort
            // 
            this.TbPort.BackColor = System.Drawing.Color.FloralWhite;
            this.TbPort.Location = new System.Drawing.Point(56, 58);
            this.TbPort.Name = "TbPort";
            this.TbPort.ReadOnly = true;
            this.TbPort.Size = new System.Drawing.Size(81, 20);
            this.TbPort.TabIndex = 7;
            this.TbPort.Text = "4120";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(141, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Private KB";
            // 
            // TbPrivate
            // 
            this.TbPrivate.BackColor = System.Drawing.Color.FloralWhite;
            this.TbPrivate.Location = new System.Drawing.Point(204, 58);
            this.TbPrivate.Name = "TbPrivate";
            this.TbPrivate.ReadOnly = true;
            this.TbPrivate.Size = new System.Drawing.Size(77, 20);
            this.TbPrivate.TabIndex = 9;
            this.TbPrivate.Text = "800000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(287, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Virtual KB";
            // 
            // TbVirtual
            // 
            this.TbVirtual.BackColor = System.Drawing.Color.FloralWhite;
            this.TbVirtual.Location = new System.Drawing.Point(347, 58);
            this.TbVirtual.Name = "TbVirtual";
            this.TbVirtual.ReadOnly = true;
            this.TbVirtual.Size = new System.Drawing.Size(90, 20);
            this.TbVirtual.TabIndex = 11;
            this.TbVirtual.Text = "1500000";
            // 
            // TbText
            // 
            this.TbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TbText.BackColor = System.Drawing.Color.Orange;
            this.TbText.ForeColor = System.Drawing.Color.Yellow;
            this.TbText.Location = new System.Drawing.Point(10, 112);
            this.TbText.Multiline = true;
            this.TbText.Name = "TbText";
            this.TbText.ReadOnly = true;
            this.TbText.Size = new System.Drawing.Size(602, 215);
            this.TbText.TabIndex = 12;
            this.TbText.Text = "Win WebService Monitor and management";
            // 
            // BtStart
            // 
            this.BtStart.Location = new System.Drawing.Point(550, 5);
            this.BtStart.Name = "BtStart";
            this.BtStart.Size = new System.Drawing.Size(59, 23);
            this.BtStart.TabIndex = 13;
            this.BtStart.Text = "Start";
            this.BtStart.UseVisualStyleBackColor = true;
            this.BtStart.Click += new System.EventHandler(this.BtStart_Click);
            // 
            // BtReApp
            // 
            this.BtReApp.Location = new System.Drawing.Point(550, 34);
            this.BtReApp.Name = "BtReApp";
            this.BtReApp.Size = new System.Drawing.Size(59, 23);
            this.BtReApp.TabIndex = 14;
            this.BtReApp.Text = "Re: App";
            this.BtReApp.UseVisualStyleBackColor = true;
            this.BtReApp.Click += new System.EventHandler(this.BtReApp_Click);
            // 
            // BtReIIS
            // 
            this.BtReIIS.Location = new System.Drawing.Point(550, 63);
            this.BtReIIS.Name = "BtReIIS";
            this.BtReIIS.Size = new System.Drawing.Size(59, 23);
            this.BtReIIS.TabIndex = 15;
            this.BtReIIS.Text = "Re: IIS";
            this.BtReIIS.UseVisualStyleBackColor = true;
            // 
            // LbStatus
            // 
            this.LbStatus.AutoSize = true;
            this.LbStatus.ForeColor = System.Drawing.Color.Firebrick;
            this.LbStatus.Location = new System.Drawing.Point(14, 93);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(47, 13);
            this.LbStatus.TabIndex = 16;
            this.LbStatus.Text = "status: 0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(445, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Mode";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(448, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Status";
            // 
            // TbMode
            // 
            this.TbMode.BackColor = System.Drawing.Color.FloralWhite;
            this.TbMode.Location = new System.Drawing.Point(487, 14);
            this.TbMode.Name = "TbMode";
            this.TbMode.ReadOnly = true;
            this.TbMode.Size = new System.Drawing.Size(52, 20);
            this.TbMode.TabIndex = 19;
            this.TbMode.Text = "Auto";
            // 
            // TbStatus
            // 
            this.TbStatus.BackColor = System.Drawing.Color.FloralWhite;
            this.TbStatus.Location = new System.Drawing.Point(487, 58);
            this.TbStatus.Name = "TbStatus";
            this.TbStatus.ReadOnly = true;
            this.TbStatus.Size = new System.Drawing.Size(52, 20);
            this.TbStatus.TabIndex = 20;
            this.TbStatus.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 339);
            this.Controls.Add(this.TbStatus);
            this.Controls.Add(this.TbMode);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.LbStatus);
            this.Controls.Add(this.BtReIIS);
            this.Controls.Add(this.BtReApp);
            this.Controls.Add(this.BtStart);
            this.Controls.Add(this.TbText);
            this.Controls.Add(this.TbVirtual);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TbPrivate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TbPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TbAppPool);
            this.Controls.Add(this.TbSite);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TbIp);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Win WebService Monitor : GCOOP - Isocare Systems.";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbIp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TbSite;
        private System.Windows.Forms.TextBox TbAppPool;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TbPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TbPrivate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TbVirtual;
        private System.Windows.Forms.TextBox TbText;
        private System.Windows.Forms.Button BtStart;
        private System.Windows.Forms.Button BtReApp;
        private System.Windows.Forms.Button BtReIIS;
        private System.Windows.Forms.Label LbStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TbMode;
        private System.Windows.Forms.TextBox TbStatus;
    }
}

