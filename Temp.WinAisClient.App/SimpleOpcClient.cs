using System;
using System.Threading;
using System.Threading.Tasks;
using Hylasoft.Opc.Ua;
using Hylasoft.Opc.Common;
using Opc;

namespace Temp.WinAisClient.App
{
    public class SimpleOpcClient
    {

        #region Fields

        private CancellationTokenSource _cmdCts; // Токен отмены
        private readonly Uri _opcUri; // URL OPC-сервера
        private const string CmdPath = "Data.Node.AIS.Task.Cmd"; // Путь к тэгу команды
        private const string RespPath = "Data.Node.AIS.Task.Response"; // Путь к тэгу ответа
        private Task _runCmdTask;
        private bool _init; // Флаг первой инициализации
        private readonly UaClient _client;

        #endregion

        #region Events

        public event EventHandler<TagEventArgs<string>> ValueChanged;
        public event EventHandler MonitoringCanceled;

        #endregion

        #region Constructors

        public SimpleOpcClient(Uri url)
        {
            _opcUri = url;
            _cmdCts = new CancellationTokenSource();
            _client = new UaClient(url);

        }

        #endregion

        public bool IsConnected => (_client.Status == OpcStatus.Connected);

        /// <summary>
        /// Подписывается на новые значения тэга CMD из OPC-сервера
        /// </summary>
        public void RunRespMonitoring()
        {

            // Защита от повоторной подписки 
            if (!_cmdCts.IsCancellationRequested && _init) return;

            // Создаем новый токен
            _cmdCts.Dispose();
            _cmdCts = new CancellationTokenSource();
            var token = _cmdCts.Token;

            _runCmdTask = Task.Factory.StartNew(() =>
                {

                        _client.Connect();

                        _client.Monitor<string>(RespPath, (readEvent, unsubscribe) =>
                        {
                            OnValueChanged(new TagEventArgs<string>(readEvent));
                        });

                        // Ожидаем отмены мониторинга 
                        //while (!token.IsCancellationRequested) { }
                        //OnMonitoringCanceled(EventArgs.Empty);
                    
                }, token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            _init = true;
        }

        /// <summary>
        /// Записать значение в тэг ответа OPC-сервера
        /// </summary>
        /// <param name="value"></param>
        public void WriteCmd(string value)
        {
            // TODO: вставить обработку исключений
            if (_client.Status != OpcStatus.Connected) _client.Connect();
            _client.Write(CmdPath, value);

        }

        #region OnMethods
        private void OnValueChanged(TagEventArgs<string> e)
        {
            ValueChanged?.Invoke(this, e);
        }

        private void OnMonitoringCanceled(EventArgs e)
        {
            MonitoringCanceled?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}