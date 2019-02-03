namespace AsnDataGrids.Lib.NewTasks
{
    partial class FillInMcTaskNewDataGridControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.radioCustom = new System.Windows.Forms.RadioButton();
            this.dateTimePickerEndTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerBeginTime = new System.Windows.Forms.DateTimePicker();
            this.labelEnd = new System.Windows.Forms.Label();
            this.labelBegin = new System.Windows.Forms.Label();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerBeginDate = new System.Windows.Forms.DateTimePicker();
            this.radioDay = new System.Windows.Forms.RadioButton();
            this.radioAll = new System.Windows.Forms.RadioButton();
            this.radioYear = new System.Windows.Forms.RadioButton();
            this.radioMonth = new System.Windows.Forms.RadioButton();
            this.radioWeek = new System.Windows.Forms.RadioButton();
            this.buttonExportExcel = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelToolBar = new System.Windows.Forms.Panel();
            this.panelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFilter
            // 
            this.panelFilter.Controls.Add(this.radioCustom);
            this.panelFilter.Controls.Add(this.dateTimePickerEndTime);
            this.panelFilter.Controls.Add(this.dateTimePickerBeginTime);
            this.panelFilter.Controls.Add(this.labelEnd);
            this.panelFilter.Controls.Add(this.labelBegin);
            this.panelFilter.Controls.Add(this.dateTimePickerEndDate);
            this.panelFilter.Controls.Add(this.dateTimePickerBeginDate);
            this.panelFilter.Controls.Add(this.radioDay);
            this.panelFilter.Controls.Add(this.radioAll);
            this.panelFilter.Controls.Add(this.radioYear);
            this.panelFilter.Controls.Add(this.radioMonth);
            this.panelFilter.Controls.Add(this.radioWeek);
            this.panelFilter.Location = new System.Drawing.Point(106, 4);
            this.panelFilter.Margin = new System.Windows.Forms.Padding(2);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(979, 49);
            this.panelFilter.TabIndex = 6;
            // 
            // radioCustom
            // 
            this.radioCustom.AutoSize = true;
            this.radioCustom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioCustom.Location = new System.Drawing.Point(417, 15);
            this.radioCustom.Name = "radioCustom";
            this.radioCustom.Size = new System.Drawing.Size(124, 20);
            this.radioCustom.TabIndex = 13;
            this.radioCustom.TabStop = true;
            this.radioCustom.Text = "Произвольный";
            this.radioCustom.UseVisualStyleBackColor = true;
            this.radioCustom.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // dateTimePickerEndTime
            // 
            this.dateTimePickerEndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerEndTime.Location = new System.Drawing.Point(895, 13);
            this.dateTimePickerEndTime.Name = "dateTimePickerEndTime";
            this.dateTimePickerEndTime.ShowUpDown = true;
            this.dateTimePickerEndTime.Size = new System.Drawing.Size(75, 22);
            this.dateTimePickerEndTime.TabIndex = 12;
            this.dateTimePickerEndTime.Value = new System.DateTime(2018, 6, 1, 0, 0, 0, 0);
            this.dateTimePickerEndTime.ValueChanged += new System.EventHandler(this.dateTimePickerEnd_ValueChanged);
            // 
            // dateTimePickerBeginTime
            // 
            this.dateTimePickerBeginTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerBeginTime.Location = new System.Drawing.Point(677, 13);
            this.dateTimePickerBeginTime.Name = "dateTimePickerBeginTime";
            this.dateTimePickerBeginTime.ShowUpDown = true;
            this.dateTimePickerBeginTime.Size = new System.Drawing.Size(74, 22);
            this.dateTimePickerBeginTime.TabIndex = 11;
            this.dateTimePickerBeginTime.Value = new System.DateTime(2018, 6, 1, 0, 0, 0, 0);
            this.dateTimePickerBeginTime.ValueChanged += new System.EventHandler(this.dateTimePickerBegin_ValueChanged);
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelEnd.Location = new System.Drawing.Point(764, 16);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(31, 16);
            this.labelEnd.TabIndex = 10;
            this.labelEnd.Text = "ПО:";
            // 
            // labelBegin
            // 
            this.labelBegin.AutoSize = true;
            this.labelBegin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelBegin.Location = new System.Drawing.Point(556, 16);
            this.labelBegin.Name = "labelBegin";
            this.labelBegin.Size = new System.Drawing.Size(20, 16);
            this.labelBegin.TabIndex = 9;
            this.labelBegin.Text = "С:";
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(796, 13);
            this.dateTimePickerEndDate.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(93, 22);
            this.dateTimePickerEndDate.TabIndex = 8;
            this.dateTimePickerEndDate.ValueChanged += new System.EventHandler(this.dateTimePickerEnd_ValueChanged);
            // 
            // dateTimePickerBeginDate
            // 
            this.dateTimePickerBeginDate.Enabled = false;
            this.dateTimePickerBeginDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerBeginDate.Location = new System.Drawing.Point(578, 13);
            this.dateTimePickerBeginDate.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerBeginDate.Name = "dateTimePickerBeginDate";
            this.dateTimePickerBeginDate.Size = new System.Drawing.Size(93, 22);
            this.dateTimePickerBeginDate.TabIndex = 7;
            this.dateTimePickerBeginDate.Value = new System.DateTime(2018, 6, 1, 0, 0, 0, 0);
            this.dateTimePickerBeginDate.ValueChanged += new System.EventHandler(this.dateTimePickerBegin_ValueChanged);
            // 
            // radioDay
            // 
            this.radioDay.AutoSize = true;
            this.radioDay.Checked = true;
            this.radioDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioDay.Location = new System.Drawing.Point(6, 15);
            this.radioDay.Margin = new System.Windows.Forms.Padding(2);
            this.radioDay.Name = "radioDay";
            this.radioDay.Size = new System.Drawing.Size(83, 21);
            this.radioDay.TabIndex = 5;
            this.radioDay.TabStop = true;
            this.radioDay.Text = "За сутки";
            this.radioDay.UseVisualStyleBackColor = true;
            this.radioDay.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // radioAll
            // 
            this.radioAll.AutoSize = true;
            this.radioAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioAll.Location = new System.Drawing.Point(362, 14);
            this.radioAll.Margin = new System.Windows.Forms.Padding(2);
            this.radioAll.Name = "radioAll";
            this.radioAll.Size = new System.Drawing.Size(50, 21);
            this.radioAll.TabIndex = 4;
            this.radioAll.Text = "Все";
            this.radioAll.UseVisualStyleBackColor = true;
            this.radioAll.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // radioYear
            // 
            this.radioYear.AutoSize = true;
            this.radioYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioYear.Location = new System.Drawing.Point(288, 14);
            this.radioYear.Margin = new System.Windows.Forms.Padding(2);
            this.radioYear.Name = "radioYear";
            this.radioYear.Size = new System.Drawing.Size(68, 21);
            this.radioYear.TabIndex = 3;
            this.radioYear.Text = "За год";
            this.radioYear.UseVisualStyleBackColor = true;
            this.radioYear.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // radioMonth
            // 
            this.radioMonth.AutoSize = true;
            this.radioMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioMonth.Location = new System.Drawing.Point(194, 14);
            this.radioMonth.Margin = new System.Windows.Forms.Padding(2);
            this.radioMonth.Name = "radioMonth";
            this.radioMonth.Size = new System.Drawing.Size(87, 21);
            this.radioMonth.TabIndex = 2;
            this.radioMonth.Text = "За месяц";
            this.radioMonth.UseVisualStyleBackColor = true;
            this.radioMonth.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // radioWeek
            // 
            this.radioWeek.AutoSize = true;
            this.radioWeek.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioWeek.Location = new System.Drawing.Point(93, 14);
            this.radioWeek.Margin = new System.Windows.Forms.Padding(2);
            this.radioWeek.Name = "radioWeek";
            this.radioWeek.Size = new System.Drawing.Size(97, 21);
            this.radioWeek.TabIndex = 1;
            this.radioWeek.Text = "За неделю";
            this.radioWeek.UseVisualStyleBackColor = true;
            this.radioWeek.CheckedChanged += new System.EventHandler(this.radioButtons_CheckedChanged);
            // 
            // buttonExportExcel
            // 
            this.buttonExportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonExportExcel.Image = global::AsnDataGrids.Lib.Properties.Resources.excel_3_32;
            this.buttonExportExcel.Location = new System.Drawing.Point(60, 9);
            this.buttonExportExcel.Name = "buttonExportExcel";
            this.buttonExportExcel.Size = new System.Drawing.Size(41, 41);
            this.buttonExportExcel.TabIndex = 14;
            this.buttonExportExcel.UseVisualStyleBackColor = true;
            this.buttonExportExcel.Click += new System.EventHandler(this.buttonExportExcel_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(2, 72);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1596, 314);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Image = global::AsnDataGrids.Lib.Properties.Resources.refresh_grey_36x36;
            this.buttonRefresh.Location = new System.Drawing.Point(0, 9);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(57, 41);
            this.buttonRefresh.TabIndex = 7;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelToolBar, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1600, 388);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // panelToolBar
            // 
            this.panelToolBar.Controls.Add(this.buttonExportExcel);
            this.panelToolBar.Controls.Add(this.buttonRefresh);
            this.panelToolBar.Controls.Add(this.panelFilter);
            this.panelToolBar.Location = new System.Drawing.Point(3, 3);
            this.panelToolBar.Name = "panelToolBar";
            this.panelToolBar.Size = new System.Drawing.Size(1089, 59);
            this.panelToolBar.TabIndex = 16;
            // 
            // FillInMcTaskNewDataGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FillInMcTaskNewDataGridControl";
            this.Size = new System.Drawing.Size(1600, 388);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelToolBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.RadioButton radioDay;
        private System.Windows.Forms.RadioButton radioAll;
        private System.Windows.Forms.RadioButton radioYear;
        private System.Windows.Forms.RadioButton radioMonth;
        private System.Windows.Forms.RadioButton radioWeek;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label labelEnd;
        private System.Windows.Forms.Label labelBegin;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerBeginDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerBeginTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndTime;
        private System.Windows.Forms.RadioButton radioCustom;
        private System.Windows.Forms.Button buttonExportExcel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelToolBar;
    }
}
