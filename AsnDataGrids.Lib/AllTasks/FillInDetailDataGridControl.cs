using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AsnDataGrids.Lib.AllTasks
{
    public partial class FillInDetailDataGridControl : UserControl
    {

        #region Fields

        // Время начала в произвольной выборке
        private DateTime _customDateBegin = new DateTime();

        // Время окончания в произвольной выборке
        private DateTime _customDateEnd = new DateTime();

        private string _connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=TestDapper;Integrated Security=True";

        private const string ConfigFile = @"ConfigArmAisIntegration.json";

        private const string SqlAll = @"SELECT [FillInDetailId] as 'ID(DB)'
      ,[FillInDetail].[Ts] as 'TS(DB)'
      ,[FillInTask].[FillInTaskId] as 'ID задания (DB)'
      ,[FillInTask].[Tdt] as 'Дата'
      ,[FillInTask].[AisTaskId] as 'ИД задания АИС ТПС'
      ,[Sid] as 'ИД секции'
      ,[FillInTask].[Tno] as 'Номер задания'
      ,[FillInTask].[Pn] as 'Гос. номер АЦ'
      ,[FillInTask].[Dn] as 'ФИО водителия'
      ,[FillInTask].[On] as 'ФИО оператора'
      ,[Ls].[Name] as 'Код состояния поста налива'
      ,[Fs].[Name] as 'Код статуса налива в секцию'
      ,[Lnp] as 'Номер поста(план)'
      ,[Sn] as 'Номер секции АЦ'
      ,[Fm].[Name] as 'Способ налива'
      ,[Ppf].[Name] as 'Признак производства продукта'
      ,[FillInDetail].[Pn] as 'Продукт'
      ,[Bfn] as 'Наименование базового топлива'
      ,[An] as 'Наименование присадки'
      ,[Tn] as 'Номер резервуаара'
      ,[Atn] as 'Номер резервуара с присадкой'
      ,[Pvp] as 'Объект продукта(план)'
      ,[Bfvp] as 'Объем базового топлива(план)'
      ,[Avp] as 'Объем присадки(план)'
      ,[Pmp] as 'Масса продукта(план)'
      ,[Bfmp] as 'Масса базового топлива(план)'
      ,[Amp] as 'Масса присадки(план)'
      ,[Lnf] as 'Номер поста(факт)'
      ,[Rm] as 'Сообщение о работе'
      ,[Pv1] as 'Показание расходомера до налива продукта'
      ,[Pv2] as 'Показание расходомера после налива продукта'
      ,[Bfv1] as 'Показания расходомера до налива базового топлива'
      ,[Bfv2] as 'Показания расходомера после налива базового топлива'
      ,[Av1] as 'Показания расходомера до налива присадки'
      ,[Av2] as 'Показания арсходомера после налива присадки'
      ,[Pvf] as 'Объем продукта(факт)'
      ,[Bfvf] as 'Объем базового топлива(факт)'
      ,[Afv] as 'Объем присакди(факт)'
      ,[P] as 'Давление на линии'
      ,[Pmf] as 'Масса продукта(факт)'
      ,[Bfmf] as 'Масса базового топлива(факт)'
      ,[Amf] as 'Масса присадки(факт)'
      ,[Ptf] as 'Температура продуккта(факт)'
      ,[Bftf] as 'Температура базового топлива(факт)'
      ,[Atf] as 'Температура присадки(факт)'
      ,[Prf] as 'Плотность продукта(факт)'
      ,[Bfrf] as 'Плотность базового топлива(факт)'
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
        ";

        private const string SqlSort = "\n ORDER BY [FillInTask].[Tdt] DESC";

        private const string SqlDay = SqlAll + "\n WHERE [FillInTask].[Tdt] > DATEADD(day,-1,GETDATE())" + SqlSort;
        private const string SqlWeek = SqlAll + "\n WHERE [FillInTask].[Tdt] > DATEADD(WEEK,-1,GETDATE())" + SqlSort;
        private const string SqlMonth = SqlAll + "\n WHERE [FillInTask].[Tdt] > DATEADD(month,-1,GETDATE())" + SqlSort;
        private const string SqlYear = SqlAll + "\n WHERE [FillInTask].[Tdt] > DATEADD(year,-1,GETDATE())" + SqlSort;

        #endregion

        #region Constructors
        public FillInDetailDataGridControl()
        {
            InitializeComponent();

            dataGridView1.AllowUserToAddRows = false;
            ReFillDataGrid(panelFilter.Controls);
            panelFilter.Controls[0].Focus();
            LockDateTimePickers();

            _customDateBegin = DateTime.Now;
            _customDateEnd = DateTime.Now;

            dateTimePickerBeginDate.Value = _customDateBegin;
            dateTimePickerBeginTime.Value = _customDateBegin;
            dateTimePickerEndDate.Value = _customDateBegin;
            dateTimePickerEndTime.Value = _customDateBegin;
        } 
        #endregion

        #region Methods

        /// <summary>
        /// Возвращает набор данных из БД для DataGrid (синхронный)
        /// </summary>
        /// <param name="conString">Строка подлкючения к БД</param>
        /// <param name="query"> SQL-запрос </param>
        /// <returns></returns>
        private DataSet GetDataSet(string conString, string query)
        {
            using (var connection = new SqlConnection(conString))
            using (var adapter = new SqlDataAdapter(query, connection))
            {
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }
        }

        /// <summary>
        /// Отправляет запрос в SQL и заполняет dataGrid
        /// </summary>
        /// <param name="query">SQL - запрос</param>
        private void FillDataGrid(string query)
        {

            // Читаем из файла конфигурации имя базы данных
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, ConfigFile);
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var obj = (Config)serializer.Deserialize(file, typeof(Config));
                    if (string.IsNullOrEmpty(obj.DbName)) return;
                    _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + obj.DbName +
                                        ";Integrated Security=True";
                }
            }
            catch (Exception e)
            {

                MessageBox.Show($"Неверный конфигурационный файл \n{e.Message}");
                return;
            }


            try
            {
                // Делаем запрос в БД и формируем ответ на DataGrid
                var ds = GetDataSet(_connectionString, query);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не могу сделать запрос \n{e.Message}");
                return;
            }


            // Оформление таблицы
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Resizable = DataGridViewTriState.True;

                // Устанавливаем ширину строк "Дата начала" и "Дата окончания", чтобы влезло все
                //if ((column.Name.IndexOf("Дата начала", StringComparison.Ordinal) >= 0) ||
                //    column.Name.IndexOf("Дата окончания", StringComparison.Ordinal) >= 0)
                //{
                //    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //}
            }


        }

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
                    var qeury = GetQuerySqlStringCustom(_customDateBegin, _customDateEnd);
                    FillDataGridAsync(qeury);
                    break;
            }
        }

        /// <summary>
        /// Генерирует строку запроса по произвольному периоду
        /// </summary>
        /// <param name="beginDateTime"> Начало </param>
        /// <param name="endDateTime"> Конец </param>
        /// <returns></returns>
        private string GetQuerySqlStringCustom(DateTime beginDateTime, DateTime endDateTime)
        {

            return SqlAll +
                   " \n WHERE [FillInTask].[Tdt] BETWEEN CAST('" +
                   String.Format(new CultureInfo("en-US"), "{0}", beginDateTime) +
                   "' as datetime) AND CAST('" +
                   String.Format(new CultureInfo("en-US"), "{0}", endDateTime) +
                   "' as datetime) " + SqlSort;

        }

        /// <summary>
        /// Блокирует элементы произвольной выборки
        /// </summary>
        private void LockDateTimePickers()
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
        private void UnlockDateTimePicker()
        {
            dateTimePickerBeginDate.Enabled = true;
            dateTimePickerBeginTime.Enabled = true;
            dateTimePickerEndDate.Enabled = true;
            dateTimePickerEndTime.Enabled = true;

            dateTimePickerBeginDate.Parent.Refresh();

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


        #endregion

        #region AsyncMethods

        /// <summary>
        /// Возвращает набор данных из БД для DataGrid (асинхронный)
        /// </summary>
        /// <param name="conString"> Строка подлкючения к БД </param>
        /// <param name="query"> SQL-запрос </param>
        /// <returns></returns>
        private Task<DataSet> GetDataSetAsync(string conString, string query)
        {
            return Task.Run(() => GetDataSet(conString, query));
        }

        /// <summary>
        /// Отправляет запрос в SQL и заполняет dataGrid с помощью асинхронного запроса
        /// </summary>
        /// <param name="query">SQL - запрос</param>
        /// <returns></returns>
        private async void FillDataGridAsync(string query)
        {
            // Читаем из файла конфигурации имя базы данных
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, ConfigFile);
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var obj = (Config)serializer.Deserialize(file, typeof(Config));
                    if (string.IsNullOrEmpty(obj.DbName)) return;
                    _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=" + obj.DbName +
                                        ";Integrated Security=True";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не могу сделать запрос \n{e.Message}");
                return;
            }


            try
            {
                // Делаем запрос в БД и формируем ответ на DataGrid
                var ds = await GetDataSetAsync(_connectionString, query);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не могу сделать запрос \n{e.Message}");
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
            var radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Checked)
            {

                switch (radioButton.Name)
                {
                    case "radioDay":
                        FillDataGrid(SqlDay);
                        break;
                    case "radioWeek":
                        FillDataGrid(SqlWeek);
                        break;
                    case "radioMonth":
                        FillDataGrid(SqlMonth);
                        break;
                    case "radioYear":
                        FillDataGrid(SqlYear);

                        break;
                    case "radioAll":
                        FillDataGrid(SqlAll);
                        break;

                    case "radioCustom":

                        UnlockDateTimePicker();
                        var qeury = GetQuerySqlStringCustom(_customDateBegin, _customDateEnd);
                        FillDataGrid(qeury);
                        break;

                }
            }
        }

        private void dateTimePickerBegin_ValueChanged(object sender, EventArgs e)
        {
            _customDateBegin = dateTimePickerBeginDate.Value.Date + dateTimePickerBeginTime.Value.TimeOfDay;
            var qeury = GetQuerySqlStringCustom(_customDateBegin, _customDateEnd);
            FillDataGrid(qeury);
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            _customDateEnd = dateTimePickerEndDate.Value.Date + dateTimePickerEndTime.Value.TimeOfDay;
            var qeury = GetQuerySqlStringCustom(_customDateBegin, _customDateEnd);
            FillDataGrid(qeury);
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
            ReFillDataGrid(panelFilter.Controls);
        }



        #endregion

    }

}
