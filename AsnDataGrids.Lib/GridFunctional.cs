using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hylasoft.Opc.Da;


namespace AsnDataGrids.Lib
{
    public class GridFunctional
    {
        /// <summary>
        /// Возвращает порядковый номер поста, считая от 1, 0 - не найден или ошибка
        /// </summary>
        /// <param name="postNameStr"></param>v
        /// <param name="cfg"></param>
        /// <returns> Порядковый номер поста </returns>
        internal static uint GetPostNumber(string postNameStr, Config cfg)
        {
            // Определяем порядковый номер поста из списка их названий
            try
            {
                var index = cfg.PostList.FindIndex((item) => item == postNameStr);
                return index < 0 ? 0 : (uint)index + 1;
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
        internal static Config GetConfig(string path)
        {
            try
            {
                string json = File.ReadAllText(path, Encoding.GetEncoding(1251));
                return JsonConvert.DeserializeObject<Config>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Записать значение в OPC сервер
        /// </summary>
        /// <param name="cfg"> Конфигурация </param>
        /// <param name="values"> Значения тэгов </param>
        internal static void SendRequestToOpcUa(Config cfg, OpcExchangeTagValues values)
        {
            var opcUrl = new Uri(cfg.OpcPath);
            using (var client = new DaClient(opcUrl))
            {
                client.Connect();
                client.Write(cfg.OpcExchangeTag.CmdStr, values.CmdStr);
            }
        }

        #region Database Methods

        /// <summary>
        /// Возвращает набор данных из БД для DataGrid (синхронный)
        /// </summary>
        /// <param name="conString">Строка подлкючения к БД</param>
        /// <param name="query"> SQL-запрос </param>
        /// <returns></returns>
        internal DataSet GetDataSet(string conString, string query)
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
        /// Возвращает набор данных из БД для DataGrid (асинхронный)
        /// </summary>
        /// <param name="conString"> Строка подлкючения к БД </param>
        /// <param name="query"> SQL-запрос </param>
        /// <returns></returns>
        internal Task<DataSet> GetDataSetAsync(string conString, string query)
        {
            return Task.Run(() => GetDataSet(conString, query));
        }

        #endregion



    }
}