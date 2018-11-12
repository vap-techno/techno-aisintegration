using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using DAL.InMemory;
using Graybox.OPC.ServerToolkit.CLRWrapper;
using Serilog;

namespace Techno.AsnOpcDa.App
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

        private const int TAG_CMD = 1;
        private const int TAG_RESP = 2;

        private static readonly string TAI_PATH =
            Path.Combine(@"C:\Program Files\TechnoAisIntegration", "TechnoAisIntegration.App.exe");

        // This int will be set to 1 when it's time to exit.
        static int stop = 0;
        // Count of OPC tags to create.
        static readonly int tag_count = 2;
        // The TagId identifiers of OPC tags.
        static int[] tag_ids = new int[tag_count];

        private static bool firstStart = false;

        // Метки времени для анализа данных в буфере MMF
        private static long ticks = 0;
        private static long lastTicks = 0;

        private static string content = "";

        static readonly OPCDAServer srv = new OPCDAServer();

        // The process entry point. Set [MTAThread] to enable free threading.
        // Free threading is required by the OPC Toolkit.
        [MTAThread]
        static void Main(string[] args)
        {

            Console.WriteLine("---------- OPC DA Сервер Techno.Asn.Opc.Da ---------");

            #region
            var icon = new NotifyIcon
            {
                Visible = true,
                //ContextMenu = menu,
                Icon = SystemIcons.Application,
                Text = @"Techno.AsnOpcDa"
            };

            icon.DoubleClick += Icon_DoubleClick;

            ShowWindow(GetConsoleWindow(), 1); // На старте окно скрываем

            #endregion

            // This will be the CLSID and the AppID of our COM-object.
            var srvGuid = new Guid("3464aeca-ba04-4343-9a01-944d8e1aa873");

            // Parse the command line args
            if (args.Length > 0)
            {
                try
                {
                    // Register the OPC server and return.
                    if (args[0].IndexOf("-r") != -1)
                    {
                        OPCDAServer.RegisterServer(
                            srvGuid,
                            "Techno",
                            "TehcnoAisIntegration",
                            "Techno.AsnOpc.Da",
                            "0.1");
                        return;
                    }
                    // Remove the OPC server registration and return.
                    if (args[0].IndexOf("-u") != -1)
                    {
                        OPCDAServer.UnregisterServer(srvGuid);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }


            /* ------------- Инициализая логгера ------------- */

            // Создаем директорию логгера, если ее не существует
            string pathDir = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Asn OPC Log Files");
            Directory.CreateDirectory(pathDir);

            // Инициализация логгера
            var path = Path.Combine(pathDir, "AsnOpcDa.log");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .WriteTo.File(path, fileSizeLimitBytes: 262144000)
                .CreateLogger();

            Log.Information("Запускаем логгер");

            /* Монитор процесса TAI */
            var monitor = new ProcessMonitor("TechnoAisIntegration.App");

            // Создаем callback монитора
            TimerCallback tm = new TimerCallback(TryTaiStart);
            
            // Создаем таймер
            System.Threading.Timer timer = new System.Threading.Timer(tm, monitor, 0, 5000);


            /* ------------- Логика OPC DA ------------------ */

            // Advise for the OPC Toolkit events.
            srv.Events.WriteItems += new WriteItemsEventHandler(Events_WriteItems);
            //srv.Events.ReadItems += new ReadItemsEventHandler(Events_ReadItems);
            srv.Events.ServerReleased += new ServerReleasedEventHandler(Events_ServerReleased);
            // Initialize the OPC server object and the OPC Toolkit.
            srv.Initialize(srvGuid, 50, 50, ServerOptions.NoAccessPaths, '.', 25);

            // Create the OPC tags.
            // Create a tag.
            srv.CreateTag(0, "Node.Task.Cmd", AccessRights.readWritable, "");
            srv.CreateTag(1, "Node.Task.Response", AccessRights.readWritable, "");


            // Mark the OPC server COM object as running.
            srv.RegisterClassObject();

            //srv.BeginUpdate();
            srv.SetTag(TAG_CMD, "", Quality.Good, FileTime.UtcNow);
            srv.SetTag(TAG_RESP, "fff", Quality.Good, FileTime.UtcNow);

            int[] tagsArr = new[] {TAG_CMD, TAG_RESP};
            srv.UpdateTags(tagsArr, Quality.Good, FileTime.UtcNow, ErrorCodes.Abort, true);

            //srv.EndUpdate(true);


            Application.Run();

            // Wait until the OPC server is released by the clients.
            // Periodically update tags values while the OPC server is not released.
            // TODO: Если нет необходимости что-то делать в цикле, то переделать на AutoResetEvent или TPL
            while (System.Threading.Interlocked.CompareExchange(ref stop, 1, 1) == 0)
            {

                //System.Threading.Thread.Sleep(500);
                //Console.WriteLine();
                // Begin the update of the OPC server cache.
                //srv.BeginUpdate();

                // Finish the update of the OPC server cache. We pass false,
                // because its unnecessary for this update to be synchronous.
                //srv.EndUpdate(true);
            }
            // Mark the OPC server COM object as stopped.
//            srv.RevokeClassObject();

        }

        /// <summary>
        /// A handler for the WriteItems event of the OPCDAServer object.
        /// We do not update the OPC server cache here.
        /// </summary>
        static void Events_WriteItems(object sender, WriteItemsArgs e)
        {

            e.CopyToCache = false;

            for (int i = 0; i < e.Count; i++)
            {

                Console.WriteLine(e.Errors[i]);

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
                            if (val.ToLower() == "test error") throw new Exception("Test ");


                            MemoryExchange mmfCmd = new MemoryExchange(MemoryFileName.Cmd);
                            MemoryExchange mmfResp = new MemoryExchange(MemoryFileName.Response);

                            var res = mmfCmd.Write(val);

                           Log.Information("Запись в MMF память Cmd: {res}", res);

                            // Ожидаем ответа в блокирующем режиме от TAI

                            int maxPeriods = 50; // Максимальное количество периодов перед таймаутом
                            int pollingPeriod = 200; //
                            int pollingCounter = 0;

                            var resp = new MmContent();

                            // Мониторим буфер Response и сравниваем его содержимое по метке времени, если буфер обновился, то выкидываем новое значение
                            while (ticks == lastTicks)
                            {
                                if (pollingCounter > maxPeriods) throw new Exception("Превышено время ожидания ответа от TAI");
                                
                                // Засыпаем на заданные период и проверяем состояние буфера
                                Thread.Sleep(pollingPeriod);

                                resp = mmfResp.Read();
                                ticks = resp.Ticks;

                                pollingCounter++;
                            }

                            Log.Debug("Метка времени lastTicks = {lastTicks}, ticks = {ticks}, ", lastTicks, ticks);

                            lastTicks = ticks;

                            

                            Log.Debug("Принят пакет с меткой времени {Ticks}  и содержимымым: \n {Content}", resp.Ticks, resp.Content);

                            srv.SetTag(TAG_CMD, "", Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);

                            srv.BeginUpdate();
                            srv.SetTag(TAG_RESP, resp.Content, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);
                            srv.EndUpdate(true);

                            content = resp.Content;


                            //Task.Factory.StartNew(() =>
                            //{
                            //    srv.BeginUpdate();
                            //    srv.SetTag(TAG_CMD, "", Quality.Bad, FileTime.UtcNow, ErrorCodes.False);
                            //    srv.EndUpdate(false);
                            //});

                            // Ожидаем записи в кэш
                            Thread.Sleep(200);


                        }
                    }

                    e.MasterError = ErrorCodes.Ok;
                    e.Errors[i] = ErrorCodes.Ok;

                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    if (ex.Message == "Превышено время ожидания ответа от TAI")
                    {
                        e.Errors[i] = ErrorCodes.NotFound;
                    }
                    else if (ex.Message == "Test Error: OutOfMemory")
                    {
                        e.Errors[i] = ErrorCodes.OutOfMemory;
                    }
                    else
                    { 
                        e.Errors[i] = (ErrorCodes)System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                        //e.ItemIds[i].TagId = 0;
                    }
                    e.MasterError = ErrorCodes.False;
                }
            }
        }

        /// <summary>
        /// A handler for the ServerReleased event of the OPCDAServer object.
        /// </summary>
        static void Events_ServerReleased(object sender, ServerReleasedArgs e)
        {
            // Make the OPC server object 'suspended'.
            // No new OPC server instances can be created by the clients
            // from this moment.
            e.Suspend = true;
            // Signal the main thread, that it's time to exit.
            System.Threading.Interlocked.Exchange(ref stop, 1);

            // Mark the OPC server COM object as stopped.
            srv.RevokeClassObject();

            Application.Exit();
        }

        /// <summary>
        /// Запускает TAI, если он не запущен
        /// </summary>
        /// <param name="pm"></param>
        static void TryTaiStart(object pm)
        {
            var monitor = (ProcessMonitor) pm;
            if (monitor != null)
            {
                if (monitor.IsProcessRunning()) return;

                try
                {
                    Process.Start(TAI_PATH);
                }
                catch (Exception e)
                {
                    Log.Error("Не могу запустить TAI по пути: {TAI_PATH}", TAI_PATH);
                    Log.Debug("Ошибка {e}", e);
                }
                
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

    }
}
