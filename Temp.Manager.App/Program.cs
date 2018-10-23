using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Exceptions;
using BL.Core;
using System.Windows.Forms;
using AisOpcClient.Lib;
using DAL.Core.TaskMapper;


namespace Temp.Manager.App
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

            // Инициализация логгера
            string logFile = "TempAisManagerlog.txt";
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", logFile);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .WriteTo.File(path, rollingInterval: RollingInterval.Month)
                .Enrich.WithExceptionDetails()
                .CreateLogger();

            // Инициализация OPC-клиента
            var opcUrl = new Uri("opc.tcp://localhost:55000");
            var opcService = new OpcService(opcUrl, Log.Logger);

            // Строка подключения к БД
            const string conString =
                @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TestDapper;Data Source=.\SQLEXPRESS";

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

            string iconPath = Path.Combine(Environment.CurrentDirectory, "app.ico");

            var icon = new NotifyIcon
            {
                Visible = true,
                //ContextMenu = menu,
                Icon = File.Exists(iconPath) ? new Icon(iconPath) : SystemIcons.Asterisk,
                Text = @"Technologia AisTPS Integration"
            };

            icon.DoubleClick += Icon_DoubleClick;
 
            //ShowWindow(GetConsoleWindow(), 0); // На старте окно скрываем

            var options = new ManagerOptions()
            {
                DbConString = conString,
                OpcServerUri = opcUrl
            };

            var manager = new BL.Core.Manager(conString, taskMapper, opcService, Log.Logger);
            
            Application.Run();

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
        }
    }
}
