using System;

namespace BL.Core
{
    public class ManagerOptions
    {
        /// <summary>
        /// URL OPC-сервера
        /// </summary>
        public Uri OpcServerUri { get; set; }

        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public string DbConString { get; set; }
        
        // TODO: Вставить остальные настройки, такие как имена тэгов команды и ответа
    }
}