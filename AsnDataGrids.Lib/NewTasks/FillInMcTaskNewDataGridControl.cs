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
    public partial class FillInMcTaskNewDataGridControl : UserControl
    {

        #region Fields

        private string _aisId = ""; // Выбранный идентификатор задания
        private string _sectId = ""; // Выбранный идентификатор секции
        private uint _postNumber = 0; // Выбранный номер поста
        private readonly int _cmdType = 2; // Налив КМХ АЦ
        private readonly string _tableName = "FillInMcTask"; // Таблица, из которой берем данные по временной выборки

        // Распложение файла конфигурации
        private  readonly  string[] _cfgPathArr = {
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

        private const string SqlAll = @"SELECT [FillInMcTaskId] as 'ID(DB)'
      ,[Ts] as 'TS(DB)'
      ,[Cid] as 'ID команды'
      ,[AisTaskId] as 'ID задания АИС ТПС'
      ,[Ls].[Name] as 'Код состояния поста налива'
      ,[Fs].[Name] as 'Код статуса налива в секцию'
      ,[Tdt] as 'Дата'
      ,[Tno] as 'Номер задания'
      ,[On] as 'ФИО оператора'
      ,[Mm] as 'Способ измерения'
      ,[Lnp] as 'Номер поста(план)'
      ,[Pn] as 'Продукт'
      ,[Tn] as 'Номер резервуаара'
      ,[Pvp] as 'Объект продукта(план)'
      ,[Pmp] as 'Масса продукта(план)'
      ,[Lnf] as 'Номер поста(факт)'
      ,[Rm] as 'Сообщение о работе'
      ,[Pv1] as 'Показание расходомера до налива продукта'
      ,[Pv2] as 'Показание расходомера после налива продукта'
      ,[Pvf] as 'Объем продукта(факт)'
      ,[Pmf] as 'Масса продукта(факт)'
      ,[Ptf] as 'Температура продуккта(факт)'
      ,[Prf] as 'Плотность продукта(факт)'
      ,[Tadj] as 'Температура приведения плотности(факт)'
      ,[PrAdjF] as 'Приведенная плотность продукта(факт)'
      ,[Dt1] as 'Дата начала налива'
      ,[Dt2] as 'Дата окончания налива'
      ,[FSpd] as 'Скорость налива'
      ,[TimeRest] as 'Остаток времени налива'
      FROM [FillInMcTask]
      INNER JOIN [Ls]
      ON [Ls].[LsId] = [FillInMcTask].[Ls]
      INNER JOIN [Fs]
      ON [Fs].[FsId] = [FillInMcTask].[Fs]
      WHERE [FillInMcTask].[Fs] = 1 
        ";

        

        private const string SqlSort = "\n ORDER BY [FillInMcTask].[Tdt] DESC";

        private const string SqlDay = SqlAll + "AND [FillInMcTask].[Tdt] > DATEADD(day,-1,GETDATE())" + SqlSort;
        private const string SqlWeek = SqlAll + "AND [FillInMcTask].[Tdt] > DATEADD(WEEK,-1,GETDATE())" + SqlSort;
        private const string SqlMonth = SqlAll + "AND [FillInMcTask].[Tdt] > DATEADD(month,-1,GETDATE())" + SqlSort;
        private const string SqlYear = SqlAll + "AND [FillInMcTask].[Tdt] > DATEADD(year,-1,GETDATE())" + SqlSort;

        #endregion

        #region Constructors
        public FillInMcTaskNewDataGridControl()
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
                    var qeury = GridFunctional.GetQuerySqlStringCustom(SqlAll,SqlSort,_tableName,_customDateBegin, _customDateEnd);
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
                _connectionString = $@"Data Source=.\SQLEXPRESS;Initial Catalog={_cfg.DbName};Integrated Security=True";

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
                if (column.Name.IndexOf("Отгружено", StringComparison.Ordinal) >= 0)
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

                        var qeury = GridFunctional.GetQuerySqlStringCustom(SqlAll,SqlSort,_tableName,_customDateBegin, _customDateEnd);
                        FillDataGridAsync(qeury);
                        break;

                }
            }
        }

        private void dateTimePickerBegin_ValueChanged(object sender, EventArgs e)
        {
            _customDateBegin = dateTimePickerBeginDate.Value.Date + dateTimePickerBeginTime.Value.TimeOfDay;
            var qeury = GridFunctional.GetQuerySqlStringCustom(SqlAll,SqlSort,_tableName,_customDateBegin, _customDateEnd);
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
            // Двойной щелчок ЛКМ
            if ((e.Button & MouseButtons.Left) != 0)
            {
                // Запоминаем номер 
                int rowPosition = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                if (rowPosition >= 0)
                {
                    // Выделяем строку
                    dataGridView1.Rows[rowPosition].Selected = true;

                    _aisId = "";
                    _sectId = "";

                    for (int i = 0; i < dataGridView1.Rows[rowPosition].Cells.Count; i++)
                    {

                        if (dataGridView1.Columns[i].Name == "ИД задания АИС ТПС")
                        {
                            _aisId = (string)dataGridView1.Rows[rowPosition].Cells[i].Value;
                        }

                        if (dataGridView1.Columns[i].Name == "ИД секции")
                        {
                            _sectId = (string)dataGridView1.Rows[rowPosition].Cells[i].Value;
                        }

                    }

                    // Открываем контекстное меню
                    ContextMenuStrip conMenu = new ContextMenuStrip();

                    // Создаем контекстное меню на основе данных из конфигурации
                    try
                    {
                        var path = Path.Combine(Environment.CurrentDirectory, ConfigFile);

                        _cfg.PostList?.ForEach((item) => conMenu.Items.Add(item).Name = item);

                        // Отображаем контекстное меню
                        conMenu.Show(dataGridView1, e.X, e.Y);

                        // Выбор поста
                        conMenu.ItemClicked += ConMenu_ItemClicked;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show($"Неверный конфигурационный файл \n {exc.Message}");
                    }

                }
            }
        }

        /// <summary>
        /// Обработчик события выбора элемента контестного меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var con = sender as ContextMenuStrip;
            con?.Hide();

            DialogResult res = MessageBox.Show("Скопировать в " + e.ClickedItem.Name, "Копирование",
                MessageBoxButtons.OKCancel);

            if (res == DialogResult.OK)
            {

                // Вычисляем номер поста
                _postNumber = GridFunctional.GetPostNumber(e.ClickedItem.Name, _cfg);

                // Вычисляем значения тегов
                OpcExchangeTagValues values = new OpcExchangeTagValues()
                {
                    CmdStr = $"{_aisId};{_sectId};{_postNumber};{_cmdType}"
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
