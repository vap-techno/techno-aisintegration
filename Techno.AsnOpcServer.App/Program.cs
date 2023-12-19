using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DAL.Core.TaskMapper;
using Graybox.OPC.ServerToolkit.CLRWrapper;
using Newtonsoft.Json;
using Serilog;
using Serilog.Exceptions;

namespace Techno.AsnOpcServer.App
{
    class Program
    {

        #region Поля для сворачивания-разворачивания окна

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

        #endregion

        // OPC - сервер
        static readonly OPCDAServer srv = new OPCDAServer();

        // Таймаут ожидания, чтобы serilog успел записаться
        private const int sleep = 10;

        // Идентификаторы тега команды и ответа
        private const int TAG_CMD = 1; // Тэг Команды Cmd
        private const int TAG_RESP = 2; // Тэг Ответа Response

        private static string respValue = ""; // Хранится значение, помещаемое в Response
        private static string cmdValue = ""; // Хранится значение, помещаемое в Cmd

        private static BL.Core.Manager manager; // Б.логика

        // Необходимо установить [MTAThread] для включения free threading - требуется для OPC Toolkit
        [MTAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("---------- OPC DA Сервер Techno.AsnOpcServer ---------");

            // This will be the CLSID and the AppID of our COM-object.
            var srvGuid = new Guid("3464aeca-ba04-4343-9a01-944d8e1aa873");

            #region Конфигурация
            // Выбираем стадию разработки dev или prod
            var cfgFileName = "TAOConfig.json";
            // TODO: Скорее всего тут таже проблема с путями
            //string cfgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cfgFileName);
            string cfgPath = Path.Combine(@"C:\Project\Config\", cfgFileName);

            // Считываем конфигурацию
            var cfg = Config(args, cfgPath);

            // Строка подключения к БД
            string conString = $@"Data Source=.\\SQLEXPRESS;Persist Security Info=False;User ID=asn_user; Password=asn_user;Initial Catalog={cfg.DbName}";
            if (cfg != null)
            {
                conString = cfg.Provider == "PostgreSQL"
                    ? $@"User ID=operator;Password=operator;Host=localhost;Port=5432;Database={cfg.DbName};Pooling=true;"
                    : $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={cfg.DbName};Data Source=.\SQLEXPRESS";
            }
            #endregion

            #region Инициализация логгера
            // Создаем директорию логгера, если ее не существует
            // TODO: При работе с доменом лог пишется к запускающему пользователю, поэтому у каждого лог свой, значит использовать Мои Документы нельзя
            // string pathDir = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "TAO Log Files");
            string pathDir = Path.Combine(@"C:\Project\", "TAO Log Files");
            Console.WriteLine($"Путь к файлу конфигурации {pathDir}");
            Directory.CreateDirectory(pathDir);

            // Инициализация логгера
            var path = Path.Combine(pathDir, "TechnoAsnOpc.log");
            if (cfg != null) path = Path.Combine(pathDir, cfg.LogFile);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(path, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 31)
                .Enrich.WithExceptionDetails()
                .CreateLogger(); 

            Log.Information("Запускаем логгер");
            #endregion

            // Операция по регистрации сервера
            var regRes = RegisterServer(args, srvGuid);
            if (regRes) return;

            #region GUI
            // Отслеживание закрытия окна
            _signalHandler += HandleConsoleSignal;
            ConsoleHelper.SetSignalHandler(_signalHandler, true);

            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");

            var icon = new NotifyIcon
            {
                Visible = true,
                //ContextMenu = menu,
                Icon = File.Exists(iconPath) ? new Icon(iconPath) : SystemIcons.Asterisk,
                Text = @"Techno.AsnOpcDa"
            };

            icon.DoubleClick += Icon_DoubleClick;
            // TODO: После тестов поставить 0
            ShowWindow(GetConsoleWindow(), 0); // На старте окно не скрываем 
            #endregion

            // Mapper
            var taskMapper = new TaskMapper();
            manager = new BL.Core.Manager(conString, taskMapper, Log.Logger);

            #region OPC
            /* ------------- Логика OPC DA ------------------ */

            // События OPC
            srv.Events.WriteItems += new WriteItemsEventHandler(Events_WriteItems);
            srv.Events.ReadItems += new ReadItemsEventHandler(Events_ReadItems);
            srv.Events.ServerReleased += new ServerReleasedEventHandler(Events_ServerReleased);
            srv.Events.CreateInstance += new CreateInstanceEventHandler(Event_CreateInstance);
            srv.Events.DestroyInstance += new DestroyInstanceEventHandler(Events_DestoryInstance);
            // Инициализация OPC
            srv.Initialize(srvGuid, 50, 50, ServerOptions.NoAccessPaths, '.', 25);

            // Создаем OPC-Тэги
            srv.CreateTag(0, "Node.Task.Cmd", AccessRights.readWritable, "");
            srv.CreateTag(1, "Node.Task.Response", AccessRights.readWritable, "");

            // Отмечаем OPC-сервер как запущенный COM-объект
            srv.RegisterClassObject();

            // Устанавливаем статовое значение тэгов (пустые)
            srv.BeginUpdate();
            srv.SetTag(TAG_CMD, respValue, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);
            srv.SetTag(TAG_RESP, cmdValue, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);
            srv.EndUpdate(false);

            // TODO: Не использоватаь srv.UpdateTags - из-за него падает программа

            #endregion

            Application.Run();

        }

        private static void Events_DestoryInstance(object sender, DestroyInstanceArgs e)
        {
            return;
        }

        private static void Event_CreateInstance(object sender, CreateInstanceArgs e)
        {
            return;
        }

        /// <summary>
        /// Регистрация сервера (если ключ -r), снятие регистрации (-u)
        /// </summary>
        /// <param name="args">Массив ключей вызова программы</param>
        /// <param name="srvGuid">GUID сервера</param>
        private static bool RegisterServer(string[] args, Guid srvGuid)
        {
            // Parse the command line args
            if (args.Length > 0)
            {
                try
                {
                    // Register the OPC server and return.
                    if (args[0].IndexOf("-r") != -1)
                    {
                        Log.Information("Регистрация сервера");
                        OPCDAServer.RegisterServer(
                            srvGuid,
                            "Techno",
                            "TehcnoAsnOpcServer",
                            "Techno.AsnOpc.Da",
                            "0.2");
                        return true;
                    }

                    // Remove the OPC server registration and return.
                    if (args[0].IndexOf("-u") != -1)
                    {
                        Log.Information("Удаление регистрации");
                        OPCDAServer.UnregisterServer(srvGuid);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Ошибка во время регистрации/отмены регистрации\n", ex);
                    return true;
                }
            }

            return false;
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
                        || _cfg.LogFile != null
                        || _cfg.Provider != null)
                    {
                        cfg = _cfg;
                    }
                }
                // Рабочий режим
                else 
                {
                    var _cfg = jsonConfigs["config"];

                    if (_cfg != null
                        || _cfg.DbName != null
                        || _cfg.LogFile != null
                        || _cfg.Provider != null)
                    {
                        cfg = _cfg;
                    }
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
            Log.Warning("Приложение закрыто с кодом {consoleSignal}", consoleSignal.ToString());
            if (consoleSignal == ConsoleSignal.CtrlC || consoleSignal == ConsoleSignal.CtrlBreak) Application.Exit();
        }

        /// <summary>
        /// Обработчик для события WriteItems 
        /// </summary>
        static void Events_WriteItems(object sender, WriteItemsArgs e)
        {

            e.CopyToCache = false; // Не писать значения в кэш

            for (int i = 0; i < e.Count; i++)
            {

                Console.WriteLine(e.Errors[i]); // Debug

                if (e.ItemIds[i].TagId == 0) continue;
                try
                {
                    // Cmd
                    if (e.ItemIds[i].TagId == TAG_CMD)
                    {

                        string val = e.Values[i].ToString();
                        if (val != String.Empty || val != "")
                        {

                            // Тестовые ошибки
                            if (val.ToLower() == "test error") throw new Exception("Test Error: OutOfMemory");

                            Log.Information("Принят запрос Cmd: {val}", val);
                            Thread.Sleep(sleep);

                            respValue = val.ToLower() == "ping" ? "pong" : manager.HandleRequest(val);

                            // Сбрасываем тэг команды
                            cmdValue = "";
                            srv.SetTag(TAG_CMD, cmdValue, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);

                            // Ошибка  запроса "Не удалось обработать запрос"
                            if (respValue.Contains("ErrCode") && respValue.Contains("-1073479676") ) throw new Exception("Неверный запрос");

                            // Ошибка  запроса "Не прошла валидацию"
                            if (respValue.Contains("ErrCode") && respValue.Contains("-2147024809")) throw new Exception("Не прошло валидацию");

                            Log.Information("Записываем в тэг Response: {respValue}", respValue);
                            Thread.Sleep(sleep);

                            // Пишем результат в тэг ответа
                            srv.BeginUpdate();
                            srv.SetTag(TAG_RESP, respValue, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);
                            srv.EndUpdate(true);

                            e.MasterError = ErrorCodes.Ok;
                            e.Errors[i] = ErrorCodes.Ok;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.Error(ex,"Ошибка обработки тэга Cmd \n");
                    Thread.Sleep(sleep);

                    if (ex.Message == "Test Error: OutOfMemory")
                    {
                        e.Errors[i] = ErrorCodes.OutOfMemory;
                    }
                    else if (ex.Message == "Неверный запрос")
                    {
                        e.Errors[i] = ErrorCodes.BadType;
                    }
                    else if (ex.Message == "Не прошло валидацию")
                    {
                        e.Errors[i] = ErrorCodes.InvalidArguments;
                    }
                    else
                    {
                        e.Errors[i] = (ErrorCodes)System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                        // TODO: Это проверить
                        //e.ItemIds[i].TagId = 0;
                    }
                    e.MasterError = ErrorCodes.False;
                }
            }
        }

        /// <summary>
        /// Обработчик события ReadItems
        /// </summary>
        static void Events_ReadItems(object sender, ReadItemsArgs e)
        {
            try
            {
                // Let the Toolkit know that we don't return tag values
                // in the ReadItemsArgs.
                e.ValuesReturned = false;
                // Start the trnsaction of the OPC server cache update.
                srv.BeginUpdate();
                // Iterate through the requested items.
                for (int i = 0; i < e.Count; i++)
                {
                    // Skip items with the zero TagId.
                    if (e.ItemIds[i].TagId == 0) continue;

                    // Cmd
                    if (e.ItemIds[i].TagId == TAG_CMD)
                        srv.SetTag(TAG_CMD, cmdValue, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);

                    // Response
                    if (e.ItemIds[i].TagId == TAG_RESP)
                        srv.SetTag(TAG_RESP, respValue, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);

                }
            }


            finally
            {
                // Finish the OPC server cache update transaction. We pass true,
                // because it is necessary to wait until this transaction completes,
                // before the control returns to the calling client.
                // If refuse to do so, then the client will possibly recieve the old
                // tag values, because the new ones will not be placed in the OPC server
                // cache yet.
                srv.EndUpdate(true);
            }
        }

        /// <summary>
        /// Событие ServerReleased наступает, когда все клиенты отключаются
        /// </summary>
        static void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            // Разрешаем OPC-серверу оставаться в памяти
            e.Suspend = false;
   
        }
    }
}
