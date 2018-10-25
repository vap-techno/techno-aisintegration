using System;
using System.Threading;
using System.Threading.Tasks;
using Hylasoft.Opc.Ua;
using Hylasoft.Opc.Common;
using Serilog;


namespace AisOpcClient.Lib
{

    public class OpcService : IDisposable
    {

        #region Fields
        private CancellationTokenSource _cmdCts; // Токен отмены
        private readonly Uri _opcUri; // URL OPC-сервера
        private const string CmdPath = "Data.Node.AIS.Task.Cmd"; // Путь к тэгу команды
        private const string RespPath = "Data.Node.AIS.Task.Response"; // Путь к тэгу ответа
        private Task _runCmdTask;
        private bool _init; // Флаг первой инициализации
        private readonly UaClient _client; // UA - клиент
        private readonly ILogger _logger;

        #endregion

        #region Events
        public event EventHandler<TagEventArgs<string>> ValueChanged;
        public event EventHandler MonitoringCanceled;
        #endregion

        #region ctors
        public OpcService(Uri url, ILogger logger )
        {
            _opcUri = url;
            _cmdCts = new CancellationTokenSource();
            _client = new UaClient(url);
            _client.ServerConnectionLost += _client_ServerConnectionLost;
            _logger = logger;


        }

        #endregion

        /// <summary>
        /// Статус соединения
        /// </summary>
        public bool IsConnected => _client.Status == OpcStatus.Connected;

        #region Methods
        /// <summary>
        /// Подписывается на новые значения тэга CMD из OPC-сервера
        /// </summary>
        public void RunCmdMonitoring()
        {

            //Защита от повоторной подписки

            //if (_runCmdTask != null
            //    && _runCmdTask.Status != TaskStatus.Canceled
            //    && _runCmdTask.Status != TaskStatus.Faulted
            //    && _runCmdTask.Status != TaskStatus.RanToCompletion) return;

            //Создаем новый токен
            //_cmdCts.Cancel();
            

            if (_runCmdTask != null && _runCmdTask.Status != TaskStatus.Canceled) return;

            _cmdCts.Dispose();
            _cmdCts = new CancellationTokenSource();
            var token = _cmdCts.Token;

            _runCmdTask = Task.Factory.StartNew( async () =>
                {
                    _logger.Information("мониторинг подключения запущен");

                    // Ожидаем отмены мониторинга 
                    while (!token.IsCancellationRequested)
                    {
                        await Task.Delay(5000, token);

                        if (_client.Status == OpcStatus.NotConnected)
                        {
                            try
                            {
                                _client.Connect(); //блокирует поток, пока коннекта не будет
                            }
                            catch (Exception ex)
                            {
                                _logger.Warning("ошибка подключения к OPC-серверу");
                                continue;
                            }

                            _client.Monitor<string>(CmdPath, (readEvent, unsubscribe) =>
                            {
                                OnValueChanged(new TagEventArgs<string>(readEvent));
                            });

                            _logger.Information("OPC-сервер: Запущен мониторинг тэга команды");
                        }
                    }

                    //Отписаться от _client.Monitor<string>(CmdPath,(readEvent, unsubscribe) => { OnValueChanged(new TagEventArgs<string>(readEvent)); });
                    OnMonitoringCanceled(EventArgs.Empty);
                    _logger.Information("мониторинг подключения отключен");
                }, token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        /// <summary>
        /// Записать значение в тэг ответа OPC-сервера
        /// </summary>
        /// <param name="value"></param>
        public void WriteResponse(string value)
        {
            if (_client.Status != OpcStatus.Connected)
            { 
                try
                {
                    _client.Connect();
                }
                catch (Exception e)
                {
                    _logger.Warning("ошибка подключения к OPC-серверу");
                    return;
                }
            }

            _client.Write(RespPath, value);

            _logger.Information("OPC-сервер: Запись в тэг Response: {value}", value);
            
        }

        /// <summary>
        /// Очистить значение командного тэга
        /// </summary>
        private void EraseCmdTag()
        {
            // TODO: вставить обработку исключений
            //if (_client.Status != OpcStatus.Connected) _client.Connect();
            _client.Write(CmdPath, "");
        }

        private void _client_ServerConnectionLost(object sender, EventArgs e)
        {
            

            if (_client.Status == OpcStatus.NotConnected)
            {
                //_logger.Warning("OPC-сервер: соединение потеряно");
                //_cmdCts.Cancel();
                //RunCmdMonitoring();
            }
            
        }

        #endregion

        #region OnMethods
        private void OnValueChanged(TagEventArgs<string> e)
        {
            // Игнорируем генерацию события, если тэг сброшен "" или OPC-сервер только запустился (VT_EMPTY)
            if (string.IsNullOrEmpty(e.Tag.Value) || e.Tag.Value == "VT_EMPTY")
            {
                _logger.Debug("OPC-сервер: пустое значение");
                return;
            }

            ValueChanged?.Invoke(this, e);

            // Очищаем командны тэг
            EraseCmdTag();

            _logger.Information("OPC-сервер: принято новое значение {Value}", e.Tag.Value);
        }

        private void OnMonitoringCanceled(EventArgs e)
        {
            MonitoringCanceled?.Invoke(this, EventArgs.Empty);
            _logger.Information("OPC-сервер: Команда на отмену мониторинга переменой команды");
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            _cmdCts?.Dispose();
            _runCmdTask?.Dispose();
            _client?.Dispose();
        }
        #endregion
    }
}