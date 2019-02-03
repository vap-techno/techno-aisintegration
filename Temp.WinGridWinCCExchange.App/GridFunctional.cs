using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hylasoft.Opc.Da;
using Newtonsoft.Json;

namespace Temp.WinGridWinCCExchange.App
{
    public static class GridFunctional
    {
        /// <summary>
        /// Возвращает порядковый номер поста, считая от 1, 0 - не найден или ошибка
        /// </summary>
        /// <param name="postNameStr"></param>v
        /// <param name="cfg"></param>
        /// <returns> Порядковый номер поста </returns>
        public static uint GetPostNumber(string postNameStr, Config cfg)
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
        /// <param name="pathList">Список путей, где искать файл</param>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Возвращает объект или exception</returns>
        public static Config GetConfig(string[] pathList, string fileName)
        {

            var exc = new Exception();

            foreach (var p in pathList)
            {
                var path = Path.Combine(p, fileName);
                try
                {
                    string json = File.ReadAllText(path, Encoding.GetEncoding(1251));
                    return JsonConvert.DeserializeObject<Config>(json);
                }
                catch (Exception e)
                {
                    exc = e;
                    continue;
                }

            }

            throw exc;
        }

        /// <summary>
        /// Записать значение в OPC сервер
        /// </summary>
        /// <param name="cfg"> Конфигурация </param>
        /// <param name="values"> Значения тэгов </param>
        public static void SendRequestToOpcUa(Config cfg, OpcExchangeTagValues values)
        {
            var opcUrl = new Uri(cfg.OpcPath);
            using (var client = new DaClient(opcUrl))
            {
                client.Connect();
                client.Write(cfg.OpcExchangeTag.CmdStr, values.CmdStr);
            }
        }

        /// <summary>
        /// Возвращает набор данных из БД для DataGrid (синхронный)
        /// </summary>
        /// <param name="conString">Строка подлкючения к БД</param>
        /// <param name="query"> SQL-запрос </param>
        /// <returns></returns>
        public static DataSet GetDataSet(string conString, string query)
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
        public static Task<DataSet> GetDataSetAsync(string conString, string query)
        {
            return Task.Run(() => GetDataSet(conString, query));
        }

        /// <summary>
        /// Генерирует строку запроса по произвольному периоду
        /// </summary>
        /// <param name="tableName">Имя таблицы с полем Tdt</param>
        /// <param name="beginDateTime"> Начало </param>
        /// <param name="endDateTime"> Конец </param>
        /// <param name="sqlAll"> Основное тело запроса</param>
        /// <param name="sqlSort">Тело запроса сортировки</param>
        /// <returns></returns>
        public static string GetQuerySqlStringCustom(string sqlAll, string sqlSort, string tableName, DateTime beginDateTime, DateTime endDateTime)
        {

            return sqlAll +
                   $"AND [{tableName}].[Tdt] BETWEEN CAST('" +
                   String.Format(new CultureInfo("en-US"), "{0}", beginDateTime) +
                   "' as datetime) AND CAST('" +
                   String.Format(new CultureInfo("en-US"), "{0}", endDateTime) +
                   "' as datetime) " + sqlSort;
        }

    }
}