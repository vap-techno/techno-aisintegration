using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hylasoft.Opc.Common;
using Hylasoft.Opc.Ua;

namespace Temp.OpcClient.App
{
    class Program
    {

        const string CmdPath = "Data.Node.AIS.Task.Cmd"; // Путь к тэгу команды
        const string RespPath = "Data.Node.AIS.Task.Response"; // Путь к тэгу ответа
        private static UaClient _client;
        private static long counter = 0;
        private static int reconnectCounter = 0;
        private static bool _init = false;

        static void Main(string[] args)
        {
           var opcUrl = new Uri("opc.tcp://localhost:55000");
           _client = new UaClient(opcUrl);

            Connect();

            while (true){}
        }

        private static void _client_ServerConnectionRestored(object sender, EventArgs e)
        {
            Console.WriteLine("ServerConnectionRestored");
        }

        private static void _client_ServerConnectionLost(object sender, EventArgs e)
        {
            Console.WriteLine("ServerConnectionLost");
        }

        static async void EraseTagAsync(string tag, UaClient client)
        {
            await client.WriteAsync(tag, "");
        }


        public static void Connect()
        {

            var cmdCts = new CancellationTokenSource();
            var token = cmdCts.Token;

            var connectTask = Task.Factory.StartNew(async () =>
            {
                // Ожидаем отмены мониторинга 
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(5000, token);
                    if (reconnectCounter++ > 12)
                    {
                        try
                        {
                            reconnectCounter = 0;
                            _client.ReConnect();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        
                    }

                    if (_client.Status == OpcStatus.NotConnected)
                    {
                        try
                        {
                            _client.Connect(); //блокирует поток, пока коннекта не будет

                            if (_client.Status == OpcStatus.Connected)
                            {
                                Run();
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Не могу соединиться с OPC");
                            continue;
                        }
                    }
                }

            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public static void Run()
        {
            _client.Monitor<string>(CmdPath,
                (readEvent, unsubscribe) =>
                {
                    OnValueChanged(new TagEventArgs<string>(readEvent));
                });
        }

        private static void OnValueChanged(TagEventArgs<string> e)
        {
            // Игнорируем генерацию события, если тэг сброшен "" или OPC-сервер только запустился (VT_EMPTY)
            if (string.IsNullOrEmpty(e.Tag.Value) || e.Tag.Value == "VT_EMPTY")
            {
                Console.WriteLine("OPC-сервер: пустое значение");
                return;
            }

            Console.WriteLine("OPC-сервер: принято новое значение {0}", e.Tag.Value);
            Console.WriteLine(++counter);

            // Очищаем командны тэг
            EraseTagAsync(CmdPath, _client);
        }
    }

    
}
