using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hylasoft.Opc.Ua;

namespace Temp.WinAisClient.App
{
    public partial class Form1 : Form
    {

        private const string CmdPath = "Data.Node.AIS.Task.Cmd"; // Путь к тэгу команды
        private const string RespPath = "Data.Node.AIS.Task.Response"; // Путь к тэгу ответа
        private readonly SimpleOpcClient opcClient;

        public string JsonCmd { get; set; }

        public Form1()
        {
            InitializeComponent();

            opcClient = new SimpleOpcClient(new Uri("opc.tcp://localhost:55000"));
            opcClient.ValueChanged += OpcClient_ValueChanged;
        }

        private void OpcClient_ValueChanged(object sender, TagEventArgs<string> e)
        {
            textBoxResult.Invoke(new Action(() => { textBoxResult.Text = e.Tag.Value; }));
        }

        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                AddExtension = true,
                Multiselect = false,
                Filter = "JSON files (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    labelPathToFile.Text = fileName;
                    JsonCmd = System.IO.File.ReadAllText(fileName);
                }
            }


        }

        private void buttonConnectToOpc_Click(object sender, EventArgs e)
        {
            opcClient.RunRespMonitoring();
        }

        private void buttonSendCmd_Click(object sender, EventArgs e)
        {
            opcClient.WriteCmd(JsonCmd);
        }
    }
}
