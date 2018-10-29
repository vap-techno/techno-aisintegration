using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hylasoft.Opc.Ua;
using Newtonsoft.Json;

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

            if (e.Name == "Data.Node.AIS.Task.Cmd")
            {
                textBoxCmd.Invoke(new Action(() => { textBoxCmd.Text = e.Tag.Value; }));
            } else if (e.Name == "Data.Node.AIS.Task.Response")
            {
                textBoxResult.Invoke(new Action(() => { textBoxResult.Text = e.Tag.Value; }));
            }
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
            opcClient.RunMonitoring();
        }

        private void buttonSendCmd_Click(object sender, EventArgs e)
        {
            opcClient.WriteCmdAsync(JsonCmd);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelConnectionState.Text = opcClient.IsConnected ? "Yes" : "No";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            if (checkBoxRnd.Checked)
            {

                string GetIdFunc () => $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}";

                var fillInMc = new
                {
                    CMD = "FILL_IN_MC",
                    CID = GetIdFunc(),
                    DATA = new
                    {
                        ID = GetIdFunc(),
                        TDT = DateTime.Now,
                        TNo = GetIdFunc(),
                        ON = "Семен семеныч",
                        MM = 3,
                        LNP = "ПОСТ 1",
                        PN = "АИ98",
                        TN = "БАК 1",
                        PVP = 1000,
                        PMP = 9000
                    }
                };

                var status = new
                {
                    CMD = "STATUS",
                    CID = GetIdFunc(),
                    DATA = new
                    {
                        IDs = new[] {$"{GetIdFunc()}", "1", "2"}
                    }

                };

                var arr = new object[] {fillInMc, status};

                opcClient.WriteCmdAsync(JsonConvert.SerializeObject(arr));
            }
            else if (checkBoxCyclic.Checked && !string.IsNullOrEmpty(JsonCmd))
            {
                try
                {
                    opcClient.WriteCmdAsync(JsonCmd);
                }
                catch (Exception)
                {
                    return;
                }
                
            }
        }

        private void checkBoxRnd_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
