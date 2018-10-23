using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using BL.Core;

namespace Temp.Manager.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const string conString =
                @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TestDapper;Data Source=.\SQLEXPRESS";

            var opcUrl = new Uri("opc.tcp://localhost:55000");

            string logFile = "TempAisManagerlog.txt";
            string path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", logFile);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .WriteTo.File(path)
                .CreateLogger();

            var options = new ManagerOptions()
            {
                DbConString = conString,
                OpcServerUri = opcUrl
            };

            var manager = new BL.Core.Manager(options, Log.Logger);

            Console.ReadKey();

        }
    }
}
