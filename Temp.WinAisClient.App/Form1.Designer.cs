namespace Temp.WinAisClient.App
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
            this.components = new System.ComponentModel.Container();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.checkBoxRnd = new System.Windows.Forms.CheckBox();
            this.checkBoxCyclic = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelConnectionState = new System.Windows.Forms.Label();
            this.buttonSendCmd = new System.Windows.Forms.Button();
            this.buttonConnectToOpc = new System.Windows.Forms.Button();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.labelPathToFile = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxCmd = new System.Windows.Forms.TextBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panelSettings.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSettings
            // 
            this.panelSettings.Controls.Add(this.checkBoxRnd);
            this.panelSettings.Controls.Add(this.checkBoxCyclic);
            this.panelSettings.Controls.Add(this.label6);
            this.panelSettings.Controls.Add(this.label4);
            this.panelSettings.Controls.Add(this.label5);
            this.panelSettings.Controls.Add(this.labelConnectionState);
            this.panelSettings.Controls.Add(this.buttonSendCmd);
            this.panelSettings.Controls.Add(this.buttonConnectToOpc);
            this.panelSettings.Controls.Add(this.buttonLoadFile);
            this.panelSettings.Controls.Add(this.labelPathToFile);
            this.panelSettings.Controls.Add(this.label3);
            this.panelSettings.Controls.Add(this.label2);
            this.panelSettings.Controls.Add(this.label1);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSettings.Location = new System.Drawing.Point(0, 0);
            this.panelSettings.MinimumSize = new System.Drawing.Size(1297, 103);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(1297, 103);
            this.panelSettings.TabIndex = 0;
            // 
            // checkBoxRnd
            // 
            this.checkBoxRnd.AutoSize = true;
            this.checkBoxRnd.Location = new System.Drawing.Point(296, 60);
            this.checkBoxRnd.Name = "checkBoxRnd";
            this.checkBoxRnd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxRnd.Size = new System.Drawing.Size(50, 17);
            this.checkBoxRnd.TabIndex = 16;
            this.checkBoxRnd.Text = "RND";
            this.checkBoxRnd.UseVisualStyleBackColor = true;
            this.checkBoxRnd.CheckedChanged += new System.EventHandler(this.checkBoxRnd_CheckedChanged);
            // 
            // checkBoxCyclic
            // 
            this.checkBoxCyclic.AutoSize = true;
            this.checkBoxCyclic.Location = new System.Drawing.Point(356, 60);
            this.checkBoxCyclic.Name = "checkBoxCyclic";
            this.checkBoxCyclic.Size = new System.Drawing.Size(54, 17);
            this.checkBoxCyclic.TabIndex = 15;
            this.checkBoxCyclic.Text = "Cyclic";
            this.checkBoxCyclic.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(946, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "RESPONSE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(282, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "CMD";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(219, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "CONNECTION";
            // 
            // labelConnectionState
            // 
            this.labelConnectionState.AutoSize = true;
            this.labelConnectionState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelConnectionState.Location = new System.Drawing.Point(303, 33);
            this.labelConnectionState.Name = "labelConnectionState";
            this.labelConnectionState.Size = new System.Drawing.Size(23, 13);
            this.labelConnectionState.TabIndex = 11;
            this.labelConnectionState.Text = "No";
            // 
            // buttonSendCmd
            // 
            this.buttonSendCmd.Location = new System.Drawing.Point(215, 56);
            this.buttonSendCmd.Name = "buttonSendCmd";
            this.buttonSendCmd.Size = new System.Drawing.Size(75, 23);
            this.buttonSendCmd.TabIndex = 10;
            this.buttonSendCmd.Text = "SEND";
            this.buttonSendCmd.UseVisualStyleBackColor = true;
            this.buttonSendCmd.Click += new System.EventHandler(this.buttonSendCmd_Click);
            // 
            // buttonConnectToOpc
            // 
            this.buttonConnectToOpc.Location = new System.Drawing.Point(134, 28);
            this.buttonConnectToOpc.Name = "buttonConnectToOpc";
            this.buttonConnectToOpc.Size = new System.Drawing.Size(75, 23);
            this.buttonConnectToOpc.TabIndex = 9;
            this.buttonConnectToOpc.Text = "CONNECT";
            this.buttonConnectToOpc.UseVisualStyleBackColor = true;
            this.buttonConnectToOpc.Click += new System.EventHandler(this.buttonConnectToOpc_Click);
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(134, 57);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadFile.TabIndex = 8;
            this.buttonLoadFile.Text = "LOAD";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // labelPathToFile
            // 
            this.labelPathToFile.AutoSize = true;
            this.labelPathToFile.Location = new System.Drawing.Point(425, 61);
            this.labelPathToFile.Name = "labelPathToFile";
            this.labelPathToFile.Size = new System.Drawing.Size(54, 13);
            this.labelPathToFile.TabIndex = 7;
            this.labelPathToFile.Text = "<filePath>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "JSON COMMAND FILE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "OPC UA";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "SIMPLE OPC UA CLIENT";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxResult);
            this.panel2.Location = new System.Drawing.Point(646, 112);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(649, 311);
            this.panel2.TabIndex = 1;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResult.Location = new System.Drawing.Point(0, 0);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxResult.Size = new System.Drawing.Size(649, 311);
            this.textBoxResult.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxCmd);
            this.panel1.Location = new System.Drawing.Point(4, 112);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(636, 311);
            this.panel1.TabIndex = 2;
            // 
            // textBoxCmd
            // 
            this.textBoxCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCmd.Location = new System.Drawing.Point(0, 0);
            this.textBoxCmd.Multiline = true;
            this.textBoxCmd.Name = "textBoxCmd";
            this.textBoxCmd.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCmd.Size = new System.Drawing.Size(636, 311);
            this.textBoxCmd.TabIndex = 0;
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 300;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 422);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelSettings);
            this.MinimumSize = new System.Drawing.Size(1313, 461);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonConnectToOpc;
        private System.Windows.Forms.Button buttonLoadFile;
        private System.Windows.Forms.Label labelPathToFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Button buttonSendCmd;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelConnectionState;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxCmd;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.CheckBox checkBoxCyclic;
        private System.Windows.Forms.CheckBox checkBoxRnd;
    }
}

