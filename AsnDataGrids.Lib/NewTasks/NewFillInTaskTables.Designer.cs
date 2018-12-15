using AsnDataGrids.Lib.AllTasks;

namespace AsnDataGrids.Lib.NewTasks
{
    partial class NewFillInTaskTables
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
            this.tabPageFillInMcTask = new System.Windows.Forms.TabPage();
            this.fillInMcTaskNewDataGridControl1 = new AsnDataGrids.Lib.NewTasks.FillInMcTaskNewDataGridControl();
            this.tabPageFillInTaskDetail = new System.Windows.Forms.TabPage();
            this.fillInDetailNewDataGridControl1 = new AsnDataGrids.Lib.NewTasks.FillInDetailNewDataGridControl();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageFillInMcTask.SuspendLayout();
            this.tabPageFillInTaskDetail.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageFillInMcTask
            // 
            this.tabPageFillInMcTask.Controls.Add(this.fillInMcTaskNewDataGridControl1);
            this.tabPageFillInMcTask.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillInMcTask.Name = "tabPageFillInMcTask";
            this.tabPageFillInMcTask.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillInMcTask.Size = new System.Drawing.Size(1602, 397);
            this.tabPageFillInMcTask.TabIndex = 2;
            this.tabPageFillInMcTask.Text = "НАЛИВ КМХ (FillInMcTask)";
            this.tabPageFillInMcTask.UseVisualStyleBackColor = true;
            // 
            // fillInMcTaskNewDataGridControl1
            // 
            this.fillInMcTaskNewDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillInMcTaskNewDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillInMcTaskNewDataGridControl1.Name = "fillInMcTaskNewDataGridControl1";
            this.fillInMcTaskNewDataGridControl1.Size = new System.Drawing.Size(1596, 391);
            this.fillInMcTaskNewDataGridControl1.TabIndex = 0;
            // 
            // tabPageFillInTaskDetail
            // 
            this.tabPageFillInTaskDetail.Controls.Add(this.fillInDetailNewDataGridControl1);
            this.tabPageFillInTaskDetail.Location = new System.Drawing.Point(4, 24);
            this.tabPageFillInTaskDetail.Name = "tabPageFillInTaskDetail";
            this.tabPageFillInTaskDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFillInTaskDetail.Size = new System.Drawing.Size(1602, 397);
            this.tabPageFillInTaskDetail.TabIndex = 1;
            this.tabPageFillInTaskDetail.Text = "НАЛИВ В АЦ. СЕКЦИИ (FillInDetail)";
            this.tabPageFillInTaskDetail.UseVisualStyleBackColor = true;
            // 
            // fillInDetailNewDataGridControl1
            // 
            this.fillInDetailNewDataGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillInDetailNewDataGridControl1.Location = new System.Drawing.Point(3, 3);
            this.fillInDetailNewDataGridControl1.Name = "fillInDetailNewDataGridControl1";
            this.fillInDetailNewDataGridControl1.Size = new System.Drawing.Size(1596, 391);
            this.fillInDetailNewDataGridControl1.TabIndex = 0;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageFillInTaskDetail);
            this.tabControlMain.Controls.Add(this.tabPageFillInMcTask);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1610, 425);
            this.tabControlMain.TabIndex = 1;
            // 
            // NewFillInTaskTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Name = "NewFillInTaskTables";
            this.Size = new System.Drawing.Size(1610, 425);
            this.tabPageFillInMcTask.ResumeLayout(false);
            this.tabPageFillInTaskDetail.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPageFillInMcTask;
        private FillInMcTaskNewDataGridControl fillInMcTaskNewDataGridControl1;
        private System.Windows.Forms.TabPage tabPageFillInTaskDetail;
        private FillInDetailNewDataGridControl fillInDetailNewDataGridControl1;
        private System.Windows.Forms.TabControl tabControlMain;
    }
}
