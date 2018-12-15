namespace AsnDataGrids.Lib.AllTasks
{
    partial class AllFillInTaskTables
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

        #region Component Designer generated code

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
            this.tabControlMain.SuspendLayout();
            this.tabPageFillInTask.SuspendLayout();
            this.tabPageFillInTaskDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageFillInTask);
            this.tabControlMain.Controls.Add(this.tabPageFillInTaskDetail);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1610, 425);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageFillInTask
            // 
            this.tabPageFillInTask.Controls.Add(this.fillInTaskDataGridControl1);
            this.tabPageFillInTask.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillInTask.Name = "tabPageFillInTask";
            this.tabPageFillInTask.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillInTask.Size = new System.Drawing.Size(1602, 397);
            this.tabPageFillInTask.TabIndex = 0;
            this.tabPageFillInTask.Text = "НАЛИВ В АЦ (FillInTask)";
            this.tabPageFillInTask.UseVisualStyleBackColor = true;
            // 
            // fillInTaskDataGridControl1
            // 
            this.fillInTaskDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillInTaskDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillInTaskDataGridControl1.Name = "fillInTaskDataGridControl1";
            this.fillInTaskDataGridControl1.Size = new System.Drawing.Size(1596, 391);
            this.fillInTaskDataGridControl1.TabIndex = 0;
            // 
            // tabPageFillInTaskDetail
            // 
            this.tabPageFillInTaskDetail.Controls.Add(this.fillInDetailDataGridControl1);
            this.tabPageFillInTaskDetail.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillInTaskDetail.Name = "tabPageFillInTaskDetail";
            this.tabPageFillInTaskDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillInTaskDetail.Size = new System.Drawing.Size(1602, 397);
            this.tabPageFillInTaskDetail.TabIndex = 1;
            this.tabPageFillInTaskDetail.Text = "НАЛИВ В АЦ. СЕКЦИИ (FillInDetail)";
            this.tabPageFillInTaskDetail.UseVisualStyleBackColor = true;
            // 
            // fillInDetailDataGridControl1
            // 
            this.fillInDetailDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillInDetailDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillInDetailDataGridControl1.Name = "fillInDetailDataGridControl1";
            this.fillInDetailDataGridControl1.Size = new System.Drawing.Size(1596, 391);
            this.fillInDetailDataGridControl1.TabIndex = 0;
            // 
            // AllFillInTaskTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Name = "AllFillInTaskTables";
            this.Size = new System.Drawing.Size(1610, 425);
            this.Load += new System.EventHandler(this.AllFillInTaskTables_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageFillInTask.ResumeLayout(false);
            this.tabPageFillInTaskDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageFillInTask;
        private FillInTaskDataGridControl fillInTaskDataGridControl1;
        private System.Windows.Forms.TabPage tabPageFillInTaskDetail;
        private FillInDetailDataGridControl fillInDetailDataGridControl1;
    }
}
