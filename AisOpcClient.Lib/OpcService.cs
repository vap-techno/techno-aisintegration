using System;
using System.Threading;
using System.Threading.Tasks;
using Hylasoft.Opc.Ua;
using Hylasoft.Opc.Common;
using Serilog;


namespace AisOpcClient.Lib
{

    public class OpcService 
    {

        #region Fields
        private CancellationTokenSource _cmdCts; // Токен отмены
        private readonly Uri _opcUri; // URL OPC-сервера
        private const string CmdPath = "Data.Node.AIS.Task.Cmd"; // Путь к тэгу команды
        private const string RespPath = "Data.Node.AIS.Task.Response"; // Путь к тэгу ответа

        private Task _connectTask; // Таск по отслеживанию соединения
        private int _conCounter = 0; // Циклический счетчик анализа соединения
        private const int ReconnetCount = 12; // Счетчик, после которого произойдет реконнект с сервером


        private readonly UaClient _client; // UA - клиент
        private readonly ILogger _logger; // Логгер
        

        #endregion

        #region Events
        public event EventHandler<TagEventArgs<string>> ValueChanged;
        #endregion

        #region ctors
        public OpcService(Uri url, ILogger logger )
        {
            _opcUri = url;
            _cmdCts = new CancellationTokenSource();
            _client = new UaClient(url);
            _client.Options.SubscriptionLifetimeCount = 100;
            _client.Options.SubscriptionKeepAliveCount = 10;
            _client.Options.SessionTimeout = 4294967295;
            _logger = logger;

            _client.ServerConnectionLost += _client_ServerConnectionLost1;

        }

        private void _client_ServerConnectionLost1(object sender, EventArgs e)
        {

        }

        #endregion

        /// <summary>
        /// Статус соединения
        /// </summary>
        public bool IsConnected => _client.Status == OpcStatus.Connected;

        public void Connect()
        {

            _cmdCts = new CancellationTokenSource();
            var token = _cmdCts.Token;

            _connectTask = Task.Factory.StartNew(async () =>
                {
                    _logger.Information("Мониторинг подключения запущен");

                    // Ожидаем отмены мониторинга 
                    while (!token.IsCancellationRequested)
                    {
                        await Task.Delay(5000, token);

                        // Время реконнекта достигнуто, если сервер будет недоступен,
                        // то будет выкинуто исключение
                        if (_conCounter++ > ReconnetCount)
                        {
                            try
                            {
                                _conCounter = 0;
                                _client.ReConnect();
                            }
                            catch (Exception e)
                            {
                                _logger.Warning("OPC-сервер: Неудачный реконнект.",e);
                            }
                        }

                        // Если соединение пропало - подключаемся и подписываемся заново
                        if (_client.Status == OpcStatus.NotConnected)
                        {
                            try
                            {
                                _client.Connect(); //блокирует поток, пока коннекта не будет

                                if (_client.Status == OpcStatus.Connected)
                                {
                                    RunCmdMonitoring();
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Warning("OPC-сервер: ошибка подключения");
                            }

                        }
                    }

                    _logger.Information("мониторинг подключения отключен");

                }, token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);

        }

        #region Methods

        /// <summary>
        /// Подписывается на новые значения тэга CMD из OPC-сервера
        /// </summary>
        public void RunCmdMonitoring()
        {
                _client.Monitor<string>(CmdPath,
                    (readEvent, unsubscribe) =>
                    {
                        OnValueChanged(new TagEventArgs<string>(readEvent));
                    });

                _logger.Information("OPC-сервер: Запущен мониторинг тэга команды");
        }

        /// <summary>
        /// Записать значение в тэг ответа OPC-сервера
        /// </summary>
        /// <param name="value"></param>
        public async void WriteResponseAsync(string value)
        {
            if (_client.Status == OpcStatus.Connected)
            { 
                try
                {
                    await _client.WriteAsync(RespPath, value);
                    _logger.Information("OPC-сервер: Запись в тэг Response: {value}", value);
                }
                catch (Exception e)
                {
                    _logger.Warning("Ошибка записи в OPC-сервер");
                    return;
                }
            }
        }

        /// <summary>
        /// Очистить значение командного тэга
        /// </summary>
        private async void EraseCmdTagAsync()
        {

            try
            {
                await _client.WriteAsync(CmdPath, "");
            }
            catch (Exception e)
            {
                _logger.Warning("OPC-сервер: Неудачное обнуление тэга");
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

            _logger.Information("OPC-сервер: принято новое значение {Value}", e.Tag.Value);

            // Очищаем командны тэг
            EraseCmdTagAsync();
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            _cmdCts?.Dispose();
            _connectTask?.Dispose();
            _client?.Dispose();
        }
        #endregion
    }
}