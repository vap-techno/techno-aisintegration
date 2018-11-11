using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitaniumAS.Opc.Client;

namespace Temp.SimpleOpcDaClient2.App
{
    class Program
    {

        [MTAThread]
        static void Main(string[] args)
        {
            Bootstrap.Initialize();

            Console.ReadKey();
        }
    }
}
