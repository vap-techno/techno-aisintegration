using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hylasoft.Opc.Common;
using Hylasoft.Opc.Da;
using System.IO;

namespace Temp.SimpleSyncOpcDaCLient.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var client = new DaClient(new Uri("opcda://Technologia.AsnOpc.Da.0.1")))
            {
                client.Connect();

                string fileName = "FillInValidCmd.json";
                string path = Path.Combine(@"C:\Temp\", fileName);

                string json = File.ReadAllText(path);

                client.Write("Node.Task.Cmd", json);

            }
        }
    }
}
