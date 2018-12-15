namespace AsnDataGridsApp
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
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageFillInTask = new System.Windows.Forms.TabPage();
            this.fillInTaskDataGridControl1 = new AsnDataGrids.Lib.AllTasks.FillInTaskDataGridControl();
            this.tabPageFillInTaskDetail = new System.Windows.Forms.TabPage();
            this.fillInDetailDataGridControl1 = new AsnDataGrids.Lib.AllTasks.FillInDetailDataGridControl();
            this.tabPageFillInMcTask = new System.Windows.Forms.TabPage();
            this.fillInMcTaskDataGridControl1 = new AsnDataGrids.Lib.AllTasks.FillInMcTaskDataGridControl();
            this.tabPageFillOutTask = new System.Windows.Forms.TabPage();
            this.fillOutTaskDataGridControl1 = new AsnDataGrids.Lib.AllTasks.FillOutTaskDataGridControl();
            this.tabPageFillOutDetail = new System.Windows.Forms.TabPage();
            this.fillOutDetailDataGridControl1 = new AsnDataGrids.Lib.AllTasks.FillOutDetailDataGridControl();
            this.tabControlMain.SuspendLayout();
            this.tabPageFillInTask.SuspendLayout();
            this.tabPageFillInTaskDetail.SuspendLayout();
            this.tabPageFillInMcTask.SuspendLayout();
            this.tabPageFillOutTask.SuspendLayout();
            this.tabPageFillOutDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageFillInTask);
            this.tabControlMain.Controls.Add(this.tabPageFillInTaskDetail);
            this.tabControlMain.Controls.Add(this.tabPageFillInMcTask);
            this.tabControlMain.Controls.Add(this.tabPageFillOutTask);
            this.tabControlMain.Controls.Add(this.tabPageFillOutDetail);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1634, 461);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageFillInTask
            // 
            this.tabPageFillInTask.Controls.Add(this.fillInTaskDataGridControl1);
            this.tabPageFillInTask.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillInTask.Name = "tabPageFillInTask";
            this.tabPageFillInTask.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillInTask.Size = new System.Drawing.Size(1626, 433);
            this.tabPageFillInTask.TabIndex = 0;
            this.tabPageFillInTask.Text = "НАЛИВ В АЦ (FillInTask)";
            this.tabPageFillInTask.UseVisualStyleBackColor = true;
            this.tabPageFillInTask.Click += new System.EventHandler(this.tabPageFillInTask_Click);
            // 
            // fillInTaskDataGridControl1
            // 
            this.fillInTaskDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillInTaskDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillInTaskDataGridControl1.Name = "fillInTaskDataGridControl1";
            this.fillInTaskDataGridControl1.Size = new System.Drawing.Size(1620, 427);
            this.fillInTaskDataGridControl1.TabIndex = 0;
            // 
            // tabPageFillInTaskDetail
            // 
            this.tabPageFillInTaskDetail.Controls.Add(this.fillInDetailDataGridControl1);
            this.tabPageFillInTaskDetail.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillInTaskDetail.Name = "tabPageFillInTaskDetail";
            this.tabPageFillInTaskDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillInTaskDetail.Size = new System.Drawing.Size(1626, 533);
            this.tabPageFillInTaskDetail.TabIndex = 1;
            this.tabPageFillInTaskDetail.Text = "НАЛИВ В АЦ. СЕКЦИИ (FillInDetail)";
            this.tabPageFillInTaskDetail.UseVisualStyleBackColor = true;
            // 
            // fillInDetailDataGridControl1
            // 
            this.fillInDetailDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillInDetailDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillInDetailDataGridControl1.Name = "fillInDetailDataGridControl1";
            this.fillInDetailDataGridControl1.Size = new System.Drawing.Size(1620, 527);
            this.fillInDetailDataGridControl1.TabIndex = 0;
            // 
            // tabPageFillInMcTask
            // 
            this.tabPageFillInMcTask.Controls.Add(this.fillInMcTaskDataGridControl1);
            this.tabPageFillInMcTask.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillInMcTask.Name = "tabPageFillInMcTask";
            this.tabPageFillInMcTask.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillInMcTask.Size = new System.Drawing.Size(1626, 533);
            this.tabPageFillInMcTask.TabIndex = 2;
            this.tabPageFillInMcTask.Text = "НАЛИВ КМХ (FillInMcTask)";
            this.tabPageFillInMcTask.UseVisualStyleBackColor = true;
            // 
            // fillInMcTaskDataGridControl1
            // 
            this.fillInMcTaskDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillInMcTaskDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillInMcTaskDataGridControl1.Name = "fillInMcTaskDataGridControl1";
            this.fillInMcTaskDataGridControl1.Size = new System.Drawing.Size(1620, 527);
            this.fillInMcTaskDataGridControl1.TabIndex = 0;
            // 
            // tabPageFillOutTask
            // 
            this.tabPageFillOutTask.Controls.Add(this.fillOutTaskDataGridControl1);
            this.tabPageFillOutTask.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillOutTask.Name = "tabPageFillOutTask";
            this.tabPageFillOutTask.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillOutTask.Size = new System.Drawing.Size(1626, 533);
            this.tabPageFillOutTask.TabIndex = 3;
            this.tabPageFillOutTask.Text = "СЛИВ ИЗ АЦ (FillOutTask)";
            this.tabPageFillOutTask.UseVisualStyleBackColor = true;
            // 
            // fillOutTaskDataGridControl1
            // 
            this.fillOutTaskDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillOutTaskDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillOutTaskDataGridControl1.Name = "fillOutTaskDataGridControl1";
            this.fillOutTaskDataGridControl1.Size = new System.Drawing.Size(1620, 527);
            this.fillOutTaskDataGridControl1.TabIndex = 0;
            // 
            // tabPageFillOutDetail
            // 
            this.tabPageFillOutDetail.Controls.Add(this.fillOutDetailDataGridControl1);
            this.tabPageFillOutDetail.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillOutDetail.Name = "tabPageFillOutDetail";
            this.tabPageFillOutDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillOutDetail.Size = new System.Drawing.Size(1626, 533);
            this.tabPageFillOutDetail.TabIndex = 4;
            this.tabPageFillOutDetail.Text = "СЛИВ ИЗ АЦ (FillOutDetail)";
            this.tabPageFillOutDetail.UseVisualStyleBackColor = true;
            // 
            // fillOutDetailDataGridControl1
            // 
            this.fillOutDetailDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillOutDetailDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillOutDetailDataGridControl1.Name = "fillOutDetailDataGridControl1";
            this.fillOutDetailDataGridControl1.Size = new System.Drawing.Size(1620, 527);
            this.fillOutDetailDataGridControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1634, 461);
            this.Controls.Add(this.tabControlMain);
            this.Name = "Form1";
            this.Text = "ASN DATA GRIDS";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageFillInTask.ResumeLayout(false);
            this.tabPageFillInTaskDetail.ResumeLayout(false);
            this.tabPageFillInMcTask.ResumeLayout(false);
            this.tabPageFillOutTask.ResumeLayout(false);
            this.tabPageFillOutDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageFillInTask;
        private System.Windows.Forms.TabPage tabPageFillInTaskDetail;
        private AsnDataGrids.Lib.AllTasks.FillInTaskDataGridControl fillInTaskDataGridControl1;
        private AsnDataGrids.Lib.AllTasks.FillInDetailDataGridControl fillInDetailDataGridControl1;
        private System.Windows.Forms.TabPage tabPageFillInMcTask;
        private System.Windows.Forms.TabPage tabPageFillOutTask;
        private System.Windows.Forms.TabPage tabPageFillOutDetail;
        private AsnDataGrids.Lib.AllTasks.FillInMcTaskDataGridControl fillInMcTaskDataGridControl1;
        private AsnDataGrids.Lib.AllTasks.FillOutTaskDataGridControl fillOutTaskDataGridControl1;
        private AsnDataGrids.Lib.AllTasks.FillOutDetailDataGridControl fillOutDetailDataGridControl1;
    }
}

