using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Techno.AsnOpcDa.App
{
    public class ProcessMonitor
    {
        private readonly string _processName;

        // Ctor
        public ProcessMonitor(string processName)
        {
            this._processName = processName;
        }


        // Methods

        /// <summary>
        /// Возвращает статус процесса
        /// </summary>
        /// <returns></returns>
        public bool IsProcessRunning()
        {
            
            Process[] p1 = Process.GetProcesses();
            foreach (Process pro in p1)
            {
                if ((pro.ProcessName.Contains(_processName))) return true;
            }

            return false;
        }

    }
}