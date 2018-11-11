using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TitaniumAS.Opc.Client.Common;
using TitaniumAS.Opc.Client.Da;


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
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Uri url = UrlBuilder.Build("Techno.AsnOpc.Da.0.1");
            using (var server = new OpcDaServer(url))
            {
                // Connect to the server first.
                server.Connect();

                OpcDaGroup group = server.AddGroup("MyGroup");
                group.IsActive = true;

                var cmd = new OpcDaItemDefinition
                {
                    ItemId = "Node.Task.Cmd",
                    IsActive = true
                };
                var response = new OpcDaItemDefinition
                {
                    ItemId = "Node.Task.Response",
                    IsActive = true
                };

                OpcDaItemValue[] values = group.Read(group.Items, OpcDaDataSource.Device);


            }
        }
    }
}
