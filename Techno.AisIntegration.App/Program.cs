using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using AisOpcClient.Lib;
using DAL.Core.TaskMapper;
using Newtonsoft.Json;
using Serilog;
using Serilog.Exceptions;
using Newtonsoft.Json.Linq;

namespace TechnoAisIntegration.App
{
    class Program
    {

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        // Флаг скрыть-открыть окно
        private static bool _isHided = false;

        /// <summary>
        /// Коды событий закрытия окна консоли
        /// </summary>
        internal enum ConsoleSignal
        {
            CtrlC = 0,
            CtrlBreak = 1,
            Close = 2,
            LogOff = 5,
            Shutdown = 6
        }

        internal static class ConsoleHelper
        {
            [DllImport("Kernel32", EntryPoint = "SetConsoleCtrlHandler")]
            public static extern bool SetSignalHandler(SignalHandler handler, bool add);
        }
        internal delegate void SignalHandler(ConsoleSignal consoleSignal);
        private static SignalHandler _signalHandler;


        static void Main(string[] args)
        {
            #region Configuration
            // Выбираем стадию разработки dev или prod
            var cfgFileName = "TAIConfig.json";
            string cfgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cfgFileName);

            // Считываем конфигурацию
            var cfg = Config(args, cfgPath);

            // Строка подключения к БД
            string conString = $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TestDapper;Data Source=.\SQLEXPRESS";
            if (cfg != null)
            {
                conString = cfg.Provider == "PostgreSQL"
                    ? $@"User ID=postgres;Password=xxxxxx;Host=localhost;Port=5432;Database={cfg.DbName};Pooling=true;"
                    : $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={cfg.DbName};Data Source=.\SQLEXPRESS";
            } 
            #endregion

            // Создаем директорию логгера, если ее не существует
            string pathDir = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "TAI Log Files");
            Directory.CreateDirectory(pathDir);

            // Инициализация логгера
            var path = Path.Combine(pathDir,"TechnoAisIntegration.log");
            if (cfg != null) path = Path.Combine(pathDir, cfg.LogFile);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .WriteTo.File(path, rollingInterval: RollingInterval.Month)
                .Enrich.WithExceptionDetails()
                .CreateLogger();
            
            // Инициализация OPC-клиента
            var opcUrl = new Uri("opc.tcp://localhost:55000");
            var opcService = new OpcService(opcUrl, Log.Logger);

            // Mapper
            var taskMapper = new TaskMapper();

            // Отслеживание закрытия окна
            _signalHandler += HandleConsoleSignal;
            ConsoleHelper.SetSignalHandler(_signalHandler, true);

            // Notify Icon
            //var menu = new ContextMenu();
            //var mnuShow = new MenuItem("Show");
            //var mnuHide = new MenuItem("Hide");
            //menu.MenuItems.Add(0, mnuShow);
            //menu.MenuItems.Add(1, mnuHide);

            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");

            var icon = new NotifyIcon
            {
                Visible = true,
                //ContextMenu = menu,
                Icon = File.Exists(iconPath) ? new Icon(iconPath) : SystemIcons.Asterisk,
                Text = @"Technologia AisTPS Integration"
            };

            icon.DoubleClick += Icon_DoubleClick;
 
            ShowWindow(GetConsoleWindow(), 0); // На старте окно скрываем

            var manager = new BL.Core.Manager(conString, taskMapper, opcService, Log.Logger);

            Application.Run();

        }

        /// <summary>
        /// Возвращает класс конфигурации приложения из файла JSON, при ошибке и по-умолчанию вернет тестовую конфигурацию
        /// </summary>
        /// <param name="args">dev - тестовый режим, prod - рабочий режим</param>
        /// <param name="cfgPath">Путь к файлу конфигурации</param>
        /// <returns></returns>
        private static Configuration Config(string[] args, string cfgPath)
        {
            try
            {
                // Читаем конфигурацию из файла
                string cfgJson = File.ReadAllText(cfgPath);
                Dictionary<string, Configuration> jsonConfigs = JsonConvert.DeserializeObject<Dictionary<string, Configuration>>(cfgJson);

                Configuration cfg = null;

                // Тестовый режим    
                if (args.Length != 0 && args[0] == "dev")
                {
                    var _cfg = jsonConfigs["dev"];

                    if (_cfg != null
                        || _cfg.DbName != null
                        || _cfg.Provider != null
                        || _cfg.Provider != null)
                    {
                        cfg = _cfg;
                    }
                }
                // Рабочий режим
                else if (args.Length != 0 && args[0] == "prod")
                {
                    var _cfg = jsonConfigs["prod"];

                    if (_cfg != null
                        || _cfg.DbName != null
                        || _cfg.Provider != null
                        || _cfg.Provider != null)
                    {
                        cfg = _cfg;
                    }
                }
                else
                {
                    cfg = new Configuration()
                    {
                        DbName = "TestDapper",
                        LogFile = "Dev_TechnoAisIntegration.log",
                        Provider = "SQLEXPRESS"
                    };
                }

                return cfg;
            }
            catch (Exception e)
            {

                string message = $"Невозможно загрузить конфигурацию. \n " +
                                 $"Программа запускается в тестовом режиме.\n Обратитесь к разработчику ПО и сообщите код ошибки:\n" +
                                 $"{e}";

                Console.WriteLine(message);

                // Показываем окно консоли, чтобы пользователь увидел ошибку
                ShowWindow(GetConsoleWindow(), _isHided ? 0 : 1);

                return new Configuration()
                {
                    DbName = "TestDapper",
                    LogFile = "Dev_TechnoAisIntegration.log",
                    Provider = "SQLEXPRESS"
                };
            }
        }

        /// <summary>
        /// Обработчик события двойного клика по трею
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Icon_DoubleClick(object sender, EventArgs e)
        {
            _isHided = !_isHided;

            // 0 - скрыть окно, 1 - показать
            ShowWindow(GetConsoleWindow(), _isHided ? 0 : 1);
        }


        /// <summary>
        /// Обработчик события закрытия окна консоли
        /// </summary>
        /// <param name="consoleSignal"></param>
        private static void HandleConsoleSignal(ConsoleSignal consoleSignal)
        {
            Log.Warning("Приложение закрыто с кодом {consoleSignal}",consoleSignal.ToString());
            if (consoleSignal == ConsoleSignal.CtrlC || consoleSignal == ConsoleSignal.CtrlBreak) Application.Exit();
        }
    }
}
