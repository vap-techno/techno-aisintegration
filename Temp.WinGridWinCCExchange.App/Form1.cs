using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hylasoft.Opc.Common;
using Hylasoft.Opc.Da;
using Hylasoft.Opc.Ua;
using Newtonsoft.Json;

namespace Temp.WinGridWinCCExchange.App
{
    public partial class Form1 : Form
    {

        private string _aisId = "";
        private string _sectId = "";
        private uint _postNumber = 0;
        
        private const string ConfigFile = @"ConfigArmAisIntegration.json";


        private string _connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=TestDapper;Integrated Security=True";

        private const string SqlAll = @"SELECT [FillInTask].[Tdt] as 'Дата'
      ,[FillInTask].[AisTaskId] as 'ИД задания АИС ТПС'
      ,[Sid] as 'ИД секции'
      ,[FillInTask].[Dn] as 'ФИО водителия'
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
        ";

        public Form1()
        {
            InitializeComponent();
        }

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
            }

        }

        /// <summary>
        /// Возвращает порядковый номер поста, считая от 1, 0 - не найден или ошибка
        /// </summary>
        /// <param name="postNameStr"></param>
        /// <returns> Порядковый номер поста </returns>
        private uint GetPostNumber(string postNameStr)
        {
            // Определяем порядковый номер поста из списка их названий
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, ConfigFile);
                var cfg = GetConfig(path);
                var index = cfg.PostList.FindIndex((item) => item == postNameStr);
                return index < 0 ? 0 : (uint) index + 1;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Неверный конфигурационный файл \n {exc.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Считать JSON файл конфигурации
        /// </summary>
        /// <param name="path"> Путь к файлу </param>
        /// <returns></returns>
        private Config GetConfig(string path)
        {
            try
            {
                string json = File.ReadAllText(path, Encoding.GetEncoding(1251));
                var cfg = JsonConvert.DeserializeObject<Config>(json);
                return cfg;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Записать значение в OPC сервер
        /// </summary>
        /// <param name="cfg"> Конфигурация </param>
        /// <param name="values"> Значения тэгов </param>
        private void SendRequestToOpcUa(Config cfg, OpcExchangeTagValues values)
        {
            var opcUrl = new Uri(cfg.OpcPath);
            using (var client = new DaClient(opcUrl))
            {
                client.Connect();
                client.Write(cfg.OpcExchangeTag.CmdStr, values.CmdStr);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            FillDataGrid(SqlAll);
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

            DialogResult res = MessageBox.Show("Привязать к " + e.ClickedItem.Name, "Привязка",
                MessageBoxButtons.OKCancel);

            if (res == DialogResult.OK)
            {

                // Вычисляем номер поста
                _postNumber = GetPostNumber(e.ClickedItem.Name);
                
                // Вычисляем значения тегов
                OpcExchangeTagValues values = new OpcExchangeTagValues()
                {
                    CmdStr = $"{_aisId};{_sectId};{_postNumber}"
                };

                // Отправляем значения в OPC сервер
                try
                {
                    var path = Path.Combine(Environment.CurrentDirectory, ConfigFile);
                    var cfg = GetConfig(path);
                    SendRequestToOpcUa(cfg, values);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не могу отправить запрос \n {ex}");
                }
            }
        }

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
                            _aisId = (string) dataGridView1.Rows[rowPosition].Cells[i].Value;
                        }

                        if (dataGridView1.Columns[i].Name == "ИД секции")
                        {
                            _sectId = (string) dataGridView1.Rows[rowPosition].Cells[i].Value;
                        }

                    }

                    // Открываем контекстное меню
                    ContextMenuStrip conMenu = new ContextMenuStrip();

                    // Создаем контекстное меню на основе данных из конфигурации
                    try
                    {
                        var path = Path.Combine(Environment.CurrentDirectory, ConfigFile);
                        var cfg = GetConfig(path);
                        cfg.PostList.ForEach((item) => conMenu.Items.Add(item).Name = item);

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

    }
    
}
