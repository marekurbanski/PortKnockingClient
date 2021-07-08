using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Configuration;


namespace PortKnockingClient
{
    public partial class ClientKnockerForm : Form
    {
        public ClientKnockerForm()
        {
            InitializeComponent();
        }

        // Metoda logująca dane w zakładce z Logami
        private void Log(String text)
        {
            textBoxLogs.AppendText(text + "\r\n");
        }

        static void SendUdp(int srcPort, string dstIp, int dstPort, byte[] data)
        {
            using (UdpClient c = new UdpClient(srcPort))
                c.Send(data, data.Length, dstIp, dstPort);
        }

        static void SendTcp(string dstIp, int dstPort, byte[] data)
        {
            byte value1 = 64;
            TcpClient client = new TcpClient(dstIp, dstPort);
            NetworkStream stream = client.GetStream();

            byte[] header = { 0x01 };
            stream.Write(header, 0, header.Length);
            stream.Flush();

            //stream.Write(null, 0, 1);
            //stream.WriteByte(1);


        }


        private void sendToolStripMenuItem_Click(object sender, EventArgs e)
        {

            String address = "";
            String type = "";
            String port = "" ;
            String count = "";
            String delay = "";
            int z = 0;
            string[] items;

            StreamWriter sw2 = new StreamWriter(".last.knock");
            sw2.WriteLine(commandsText.Text);
            sw2.Close();


            tabControl.SelectTab(2);
            Log("Initialize...");
            Log("Starting");
            for (int i = 0; i< commandsText.Lines.Length-1; i++)
            {
                z++;
                if (z > 100)
                {
                    i = commandsText.Lines.Length + 1;
                }

                if (commandsText.Lines[i].Trim() != "")
                {
                    items = commandsText.Lines[i].Split(';');
                    address = items[0];
                    port = items[1];
                    count = items[2];
                    delay = items[3];

                    for (int j = 1; j <= Convert.ToInt32(count); j++)
                    {
                        try
                        {
                            Log("Command " + (i + 1).ToString() + ": " + commandsText.Lines[i]);

                            SendUdp(Convert.ToInt32(port), address, Convert.ToInt32(port), Encoding.ASCII.GetBytes("."));
                            System.Threading.Thread.Sleep(Convert.ToInt32(delay));
                        }
                        catch
                        {
                            Log("Error parsing line ");
                        }
                    }
                }
            }
            Log("Finished");
        }


        private void load_on_run()
        {
            var fileContent = string.Empty;
            if(File.Exists(".last.knock"))
            {
                using (StreamReader reader = new StreamReader(".last.knock"))
                {
                    fileContent = reader.ReadToEnd();
                }
                commandsText.Text = fileContent;
            }
        }

        private void loadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
               // openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "knock files (*.knock)|*.knock|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            commandsText.Text = fileContent;
            StreamWriter sw2 = new StreamWriter(".last.knock");
            sw2.WriteLine(commandsText.Text);
            sw2.Close();
            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private void saveConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "knock files (*.knock)|*.knock|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();

                    String filepath = saveFileDialog1.FileName;
                    StreamWriter sw = new StreamWriter(@filepath);
                    sw.WriteLine(commandsText.Text);
                    sw.Close();

                    StreamWriter sw2 = new StreamWriter(".last.knock");
                    sw2.WriteLine(commandsText.Text);
                    sw2.Close();

                }
            }
        }

        private void ClientKnockerForm_Shown(object sender, EventArgs e)
        {
            load_on_run();
        }

        private void newConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            commandsText.Clear();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tool for sending UDP packets for port knocking.\r\n\r\nAny suggestions? Write to me: marek@www2.pl");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
