using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.InMemory;

namespace Temp.MonitorMemoryFromOpcDaServer.App
{
    class Program
    {

        static AutoResetEvent eventStop = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            // Инициализация MMF-файлов
            MemoryExchange mmfCmd = new MemoryExchange(MemoryFileName.Cmd);
            MemoryExchange mmfResp = new MemoryExchange(MemoryFileName.Response);

            // Период опроса MMF-файлов
            const int pollingPeriod = 100;

            // Метка времени последнего опроса
            long lastTicks = 0;

            // Мониторим буфер Response и сравниваем его содержимое по метке времени, если буфер обновился, то выкидываем новое значение
            while (true)
            {

                // Засыпаем на заданные период и проверяем состояние буфера
                Thread.Sleep(pollingPeriod);

                var req = mmfCmd.Read();
                var ticks = req.Ticks;

                // Если значение в буфере команды обновилось - отправляем ответ
                if (ticks != lastTicks)
                {
                    // TODO: Пока что отзеркаливаем
                    mmfResp.Write(req.Content);
                    lastTicks = ticks;
                }
            }

            eventStop.WaitOne();

        }
    }
}
