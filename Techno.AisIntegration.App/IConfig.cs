namespace TechnoAisIntegration.App
{
    public interface IConfig
    {
        /// <summary>
        /// Имя базы данных
        /// </summary>
        string DbName { get; set; }

        /// <summary>
        /// Тип базы данных
        /// </summary>
        string Provider { get; set; }

        /// <summary>
        /// Имя файла логгера
        /// </summary>
        string LogFile { get; set; }
        

    }
}