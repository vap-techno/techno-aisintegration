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
            this.panelSettings = new System.Windows.Forms.Panel();
            this.buttonSendCmd = new System.Windows.Forms.Button();
            this.buttonConnectToOpc = new System.Windows.Forms.Button();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.labelPathToFile = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.panelSettings.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSettings
            // 
            this.panelSettings.Controls.Add(this.buttonSendCmd);
            this.panelSettings.Controls.Add(this.buttonConnectToOpc);
            this.panelSettings.Controls.Add(this.buttonLoadFile);
            this.panelSettings.Controls.Add(this.labelPathToFile);
            this.panelSettings.Controls.Add(this.label3);
            this.panelSettings.Controls.Add(this.label2);
            this.panelSettings.Controls.Add(this.label1);
            this.panelSettings.Location = new System.Drawing.Point(4, 3);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(1229, 87);
            this.panelSettings.TabIndex = 0;
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
            this.labelPathToFile.Location = new System.Drawing.Point(296, 62);
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
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "OPC UA URL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "DESTINATION";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxResult);
            this.panel2.Location = new System.Drawing.Point(4, 96);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1229, 669);
            this.panel2.TabIndex = 1;
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(4, 4);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(1222, 662);
            this.textBoxResult.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 777);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelSettings);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
    }
}

