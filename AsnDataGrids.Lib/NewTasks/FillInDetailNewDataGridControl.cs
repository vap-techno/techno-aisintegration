﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AsnDataGrids.Lib.NewTasks
{
    public partial class FillInDetailNewDataGridControl : UserControl
    {

        #region Fields

        private ContextMenuStrip _contextMenu = null;

        private string _aisId = ""; // Выбранный идентификатор задания
        private string _sectId = ""; // Выбранный идентификатор секции
        private uint _postNumber = 0; // Выбранный номер поста
        private readonly int _cmdType = 1; // Налив в АЦ
        private int _section = 0; // Выбранная секция
        private string _tno = ""; // Выбранный номер задания
        private string _pn = ""; // Выбранный номер цистерны
        private readonly string _tableName = "FillInTask"; // Таблица, из которой берем данные по временной выборки

        // Распложение файла конфигурации
        private readonly string[] _cfgPathArr = {
            @"C:\Project\Config\",
            @"C:\Program Files (x86)\Siemens\Automation\WinCC RT Advanced",
            Environment.CurrentDirectory
        };

        private readonly Config _cfg = null; // Конфигурация

        // Время начала в произвольной выборке
        private DateTime _customDateBegin = new DateTime();

        // Время окончания в произвольной выборке
        private DateTime _customDateEnd = new DateTime();

        private string _connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=TestDapper;Integrated Security=True";

        private const string ConfigFile = @"ConfigArmAisIntegration.json";

        #endregion

        #region SQL-запрос
        private const string SqlAll = @"SELECT [FillInTask].[Tdt] as 'Дата'
      ,[FillInTask].[AisTaskId] as 'ИД задания'
      ,[Sid] as 'ИД секции'
      ,[FillInTask].[Dn] as 'ФИО водителя'
      ,[FillInTask].[Pn] as 'Гос. номер АЦ'
      ,[Sn] as 'Номер секции АЦ'
      ,[FillInDetail].[Pn] as 'Продукт'
      ,[Pvp] as 'Объем продукта(план), м3'
      ,[Pmp] as 'Масса продукта(план), кг'
      ,[Lnp] as 'Номер поста(план)'
      ,[FillInTask].[Tno] as 'Номер задания'
      ,[FillInTask].[On] as 'ФИО оператора'
      ,[FillInDetail].[Ts] as 'TS(DB)'
      ,[FillInTask].[FillInTaskId] as 'ID задания (DB)'
      ,[FillInDetailId] as 'ID(DB)'
      ,[Ls].[Name] as 'Код состояния поста налива'
      ,[Fs].[Name] as 'Код статуса налива в секцию'
      ,[Fm].[Name] as 'Способ налива'
      ,[Ppf].[Name] as 'Признак производства продукта'
      ,[Bfn] as 'Наименование базового топлива'
      ,[An] as 'Наименование присадки'
      ,[Tn] as 'Номер резервуаара'
      ,[Atn] as 'Номер резервуара с присадкой'
      ,[Bfvp] as 'Объем базового топлива(план)'
      ,[Avp] as 'Объем присадки(план)'
      ,[Bfmp] as 'Масса базового топлива(план)'
      ,[Amp] as 'Масса присадки(план)'
      ,[Lnf] as 'Номер поста(факт)'
      ,[Rm] as 'Сообщение о работе'
      ,[Pv1] as 'Показание расходомера до налива продукта, кг'
      ,[Pv2] as 'Показание расходомера после налива продукта, кг'
      ,[Bfv1] as 'Показания расходомера до налива базового топлива'
      ,[Bfv2] as 'Показания расходомера после налива базового топлива'
      ,[Av1] as 'Показания расходомера до налива присадки'
      ,[Av2] as 'Показания арсходомера после налива присадки'
      ,[Pvf] as 'Объем продукта(факт), м3'
      ,[Bfvf] as 'Объем базового топлива(факт), м3'
      ,[Afv] as 'Объем присакди(факт), м3'
      ,[P] as 'Давление на линии, МПа'
      ,[Pmf] as 'Масса продукта(факт), кг'
      ,[Bfmf] as 'Масса базового топлива(факт), кг'
      ,[Amf] as 'Масса присадки(факт), кг'
      ,[Ptf] as 'Температура продуккта(факт)'
      ,[Bftf] as 'Температура базового топлива(факт)'
      ,[Atf] as 'Температура присадки(факт)'
      ,[Prf] as 'Плотность продукта(факт), кг/м3'
      ,[Bfrf] as 'Плотность базового топлива(факт), кг/м3'
      ,[Arf] as 'Плотность присадки(факт)'
      ,[Tadj] as 'Температура приведения плотности(факт)'
      ,[PrAdjF] as 'Приведенная плотность продукта(факт)'
      ,[BfAdjf] as 'Приведенная плотность базового топлива'
      ,[Dt1] as 'Дата начала налива'
      ,[Dt2] as 'Дата окончания налива'
      ,[FSpd] as 'Скорость налива'
      ,[TimeRest] as 'Остаток времени налива'
      FROM [FillInDetail]
      INNER JOIN [FillInTask]
	  ON [FillInDetail].[FillInTaskId] = [FillInTask].[FillInTaskId]
	  INNER JOIN [Fm]
	  ON [Fm].[FmId] = [FillInDetail].[Fm]
	  INNER JOIN [Ppf]
	  ON [Ppf].[PpfId] = [FillInDetail].[Ppf]
	  INNER JOIN [Ls]
	  ON [Ls].[LsId] = [FillInDetail].[Ls]
	  INNER JOIN [Fs]
	  ON [Fs].[FsId] = [FillInDetail].[Fs]
      WHERE [FillInDetail].[Fs] = 1 
        "; 
        

        private const string SqlSort = "\n ORDER BY [FillInTask].[Tdt] DESC";
        private const string SqlDay = SqlAll + "AND [FillInTask].[Tdt] > DATEADD(day,-1,GETDATE())" + SqlSort;
        private const string SqlWeek = SqlAll + "AND [FillInTask].[Tdt] > DATEADD(WEEK,-1,GETDATE())" + SqlSort;
        private const string SqlMonth = SqlAll + "AND [FillInTask].[Tdt] > DATEADD(month,-1,GETDATE())" + SqlSort;
        private const string SqlYear = SqlAll + "AND [FillInTask].[Tdt] > DATEADD(year,-1,GETDATE())" + SqlSort;

        #endregion

        #region Constructors

        public FillInDetailNewDataGridControl()
        {
            InitializeComponent();

            try
            {
                _cfg = GridFunctional.GetConfig(_cfgPathArr, ConfigFile);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Файл конфигурации {ConfigFile} не может быть считан \n {e}");
            }

            dataGridView1.AllowUserToAddRows = false;
            
            panelFilter.Controls[0].Focus();
            LockDateTimePickers();

            _customDateBegin = DateTime.Now;
            _customDateEnd = DateTime.Now;

            dateTimePickerBeginDate.Value = _customDateBegin;
            dateTimePickerBeginTime.Value = _customDateBegin;
            dateTimePickerEndDate.Value = _customDateBegin;
            dateTimePickerEndTime.Value = _customDateBegin;

            ReFillDataGrid(panelFilter.Controls);

            _contextMenu = new ContextMenuStrip();
            _contextMenu.ItemClicked += ConMenu_ItemClicked;

        }

        #endregion
        #region Methods

        /// <summary>
        /// Заполняет DataGrid в зависимости от выбранного radiobutton в коллекции
        /// </summary>
        /// <param name="controls"> Список контролов в панели, бросить сюда Panel.Controls </param>
        private void ReFillDataGrid(ControlCollection controls)
        {
            // Определяем отмеченный radiobutton и обновляем dataGrid
            RadioButton _rb = controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);

            if (_rb == null) return;
            switch (_rb.Name)
            {
                case "radioDay":
                    FillDataGridAsync(SqlDay);
                    break;
                case "radioWeek":
                    FillDataGridAsync(SqlWeek);
                    break;
                case "radioMonth":
                    FillDataGridAsync(SqlMonth);
                    break;
                case "radioYear":
                    FillDataGridAsync(SqlYear);
                    break;
                case "radioAll":
                    FillDataGridAsync(SqlAll);
                    break;
                case "radioCustom":
                    var qeury = GridFunctional.GetQuerySqlStringCustom(SqlAll, SqlSort,_tableName,_customDateBegin, _customDateEnd);
                    FillDataGridAsync(qeury);
                    break;
            }
        }

        /// <summary>
        /// Копирует в буфер обмена все данные из ячеек DataGrid, чтобы вставить их потом в Excel
        /// </summary>
        private void CopyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        /// <summary>
        /// Экспорт таблицы в Excel
        /// </summary>
        private void ExportToExcel()
        {
            // Creating a Excel object.
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;
                worksheet.Name = "Отгрузки";


                int cellRowIndex = 1;
                int cellColumnIndex = 2; // Со второй колонки - потому что при выделении грида, выделяется и "нулевая" колонка

                // Заполняем заголовок
                for (var i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    // Excel индексируется с 1,1

                    worksheet.Cells[cellRowIndex, cellColumnIndex] = dataGridView1.Columns[i].HeaderText;
                    cellColumnIndex++;
                }



                CopyAlltoClipboard(); // Скопировать всю таблицу в буфер обмена

                // Вставить в файл Экскеля
                Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[2, 1];
                CR.Select();
                worksheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

                // Диалог сохранения файла
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                    FilterIndex = 2
                };

                // Начальная папка для сохранения
                // TODO: По необходимости установить требуемый путь для сохранения
                string path = (Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents"));
                saveDialog.InitialDirectory = Directory.Exists(path) ? path : @"C:\";

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Экспорт завершен");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }

        }

        /// <summary>
        /// Блокирует элементы произвольной выборки
        /// </summary>
        public void LockDateTimePickers()
        {
            dateTimePickerBeginDate.Enabled = false;
            dateTimePickerBeginTime.Enabled = false;
            dateTimePickerEndDate.Enabled = false;
            dateTimePickerEndTime.Enabled = false;

            dateTimePickerBeginDate.Parent.Refresh();
        }

        /// <summary>
        /// Разблокирует элементы произвольной выборки
        /// </summary>
        public void UnlockDateTimePickers()
        {
            dateTimePickerBeginDate.Enabled = true;
            dateTimePickerBeginTime.Enabled = true;
            dateTimePickerEndDate.Enabled = true;
            dateTimePickerEndTime.Enabled = true;

            dateTimePickerBeginDate.Parent.Refresh();
        }


        #endregion

        #region AsyncMethods

        /// <summary>
        /// Отправляет запрос в SQL и заполняет dataGrid с помощью асинхронного запроса
        /// </summary>
        /// <param name="query">SQL - запрос</param>
        /// <returns></returns>
        private async void FillDataGridAsync(string query)
        {

            try
            {

                _connectionString = $@"Data Source=.\SQLEXPRESS;Persist Security Info=False;User ID=asn_user; Password=asn_user;Initial Catalog={_cfg.DbName}";

                // Делаем запрос в БД и формируем ответ на DataGrid
                var ds = await GridFunctional.GetDataSetAsync(_connectionString, query);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Невозможно отравить запрос \n {e.Message}");
                return;
            }

            // Оформление таблицы
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Resizable = DataGridViewTriState.True;

                //Блокируем поля сортировок, которые уходят Exception
                if (!(column.Name.IndexOf("TS(DB)", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("ID задания", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Дата", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("ИД задания", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("ИД секции", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Номер задания", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Гос. номер АЦ", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("ФИО водителя", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("ФИО оператора", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Продукт", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Код состояния поста налива", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Код статуса налива в секцию", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Номер поста(план)", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Номер поста(факт)", StringComparison.Ordinal) >= 0 ||
                      column.Name.IndexOf("Номер секции АЦ", StringComparison.Ordinal) >= 0))
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // Устанавливаем ширину строк "Дата начала" и "Дата окончания", чтобы влезло все
                if ((column.Name.IndexOf("Дата начала", StringComparison.Ordinal) >= 0) ||
                    column.Name.IndexOf("Дата окончания", StringComparison.Ordinal) >= 0)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Нажатие кнопки "Обновить" создает заново запрос
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ReFillDataGrid(panelFilter.Controls);
        }

        /// <summary>
        /// Обработка выбора временных фильтров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            LockDateTimePickers();

            if (sender is RadioButton radioButton && radioButton.Checked)
            {

                switch (radioButton.Name)
                {
                    case "radioDay":
                        FillDataGridAsync(SqlDay);
                        break;
                    case "radioWeek":
                        FillDataGridAsync(SqlWeek);
                        break;
                    case "radioMonth":
                        FillDataGridAsync(SqlMonth);
                        break;
                    case "radioYear":
                        FillDataGridAsync(SqlYear);

                        break;
                    case "radioAll":
                        FillDataGridAsync(SqlAll);
                        break;

                    case "radioCustom":

                        UnlockDateTimePickers();

                        var qeury = GridFunctional.GetQuerySqlStringCustom(SqlAll, SqlSort, _tableName, _customDateBegin, _customDateEnd);
                        FillDataGridAsync(qeury);
                        break;

                }
            }
        }

        private void dateTimePickerBegin_ValueChanged(object sender, EventArgs e)
        {
            _customDateBegin = dateTimePickerBeginDate.Value.Date + dateTimePickerBeginTime.Value.TimeOfDay;
            var qeury = GridFunctional.GetQuerySqlStringCustom(SqlAll, SqlSort, _tableName, _customDateBegin, _customDateEnd);
            FillDataGridAsync(qeury);
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            _customDateEnd = dateTimePickerEndDate.Value.Date + dateTimePickerEndTime.Value.TimeOfDay;
            var qeury = GridFunctional.GetQuerySqlStringCustom(SqlAll,SqlSort,_tableName,_customDateBegin, _customDateEnd);
            FillDataGridAsync(qeury);
        }

        private void buttonExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        //TODO: Разблокировать только при асинхронных запросах
        /// <summary>
        /// Через каждое срабатывание таймер обновляет запрос к БД и записывает в DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_cfg != null) ReFillDataGrid(panelFilter.Controls);
        }


        /// <summary>
        /// Двойной клик по гриду
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                // Запоминаем номер строки
                int rowPosition = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                if (rowPosition >= 0)
                {
                    dataGridView1.Rows[rowPosition].Selected = true;

                    _aisId = "";
                    _sectId = "";

                    for (int i = 0; i < dataGridView1.Rows[rowPosition].Cells.Count; i++)
                    {
                        if (dataGridView1.Columns[i].Name == "ИД задания")
                        {
                            _aisId = (string) dataGridView1.Rows[rowPosition].Cells[i].Value;
                        }

                        if (dataGridView1.Columns[i].Name == "ИД секции")
                        {
                            _sectId = (string)dataGridView1.Rows[rowPosition].Cells[i].Value;
                        }

                        if (dataGridView1.Columns[i].Name == "Номер секции АЦ")
                        {
                            _section = Convert.ToInt32(dataGridView1.Rows[rowPosition].Cells[i].Value);
                        }

                        if (dataGridView1.Columns[i].Name == "Номер задания")
                        {
                            _tno = (string)dataGridView1.Rows[rowPosition].Cells[i].Value;
                        }

                        if (dataGridView1.Columns[i].Name == "Гос. номер АЦ")
                        {
                            _pn = (string)dataGridView1.Rows[rowPosition].Cells[i].Value;
                        }


                    }
                }

                

                for (int i = _contextMenu.Items.Count - 1; i >= 1; --i) _contextMenu.Items.RemoveAt(i);

                if (_cfg.PostList == null) return;

                _cfg.PostList.ForEach( (item) => _contextMenu.Items.Add(item).Name = item);
                _contextMenu.Show(dataGridView1, e.X, e.Y);

            }
        }

        /// <summary>
        /// Обработчик события выбора элемента контестного меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
            DialogResult res = MessageBox.Show("Скопировать в " + e.ClickedItem.Name, "Копирование",
                MessageBoxButtons.OKCancel);

            if (res == DialogResult.OK)
            {

                // Вычисляем номер поста
                _postNumber = GridFunctional.GetPostNumber(e.ClickedItem.Name, _cfg);

                // Вычисляем значения тегов
                OpcExchangeTagValues values = new OpcExchangeTagValues()
                {
                    CmdStr = $"{_aisId};{_sectId};{_postNumber};{_cmdType};{_section};{_tno};{_pn}"
                };

                // Отправляем значения в OPC сервер
                try
                {
                    GridFunctional.SendRequestToOpcUa(_cfg, values);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Невозможно сделать запрос в OPC-сервер \n {ex}");
                }
            }



        }

        #endregion
    }

}

