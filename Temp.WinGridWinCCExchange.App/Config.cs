using System.Collections.Generic;

namespace Temp.WinGridWinCCExchange.App
{
    public class Config
    {

        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string DbName { get; set; }
        
        /// <summary>
        /// Наименование постов налива
        /// </summary>
        public List<string> PostList { get; set; }

        /// <summary>
        /// Строка подключения к OPC-сереверу
        /// </summary>
        public string OpcPath { get; set; }

        /// <summary>
        /// Тег для записи выбранной заявки
        /// </summary>
        public OpcExchangeTags OpcExchangeTag { get; set; }

    }



}