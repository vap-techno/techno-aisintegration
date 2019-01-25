using System.Collections.Generic;
using Newtonsoft.Json;


namespace Temp.WinGridWinCCExchange.App
{
    public class Config
    {

        /// <summary>
        /// Имя базы данных
        /// </summary>
        [JsonProperty(PropertyName = "DbName", Required = Required.Always)]
        public string DbName { get; set; }

        /// <summary>
        /// Наименование постов налива
        /// </summary>
        [JsonProperty(PropertyName = "PostList", Required = Required.Default)]
        public List<string> PostList { get; set; }

        /// <summary>
        /// Строка подключения к OPC-сереверу
        /// </summary>
        [JsonProperty(PropertyName = "OpcPath", Required = Required.Default)]
        public string OpcPath { get; set; }

        /// <summary>
        /// Тег для записи выбранной заявки
        /// </summary>
        [JsonProperty(PropertyName = "OpcExchangeTag", Required = Required.Default)]
        public OpcExchangeTags OpcExchangeTag { get; set; }
    }
}