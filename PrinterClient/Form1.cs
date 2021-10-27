using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.Json;
using ExcelDataReader;
using Microsoft.Win32;
using System.Linq;
using System.Security.Principal;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;

namespace PrinterClient
{
    public partial class Form1 : Form
    {
        static TcpClient tcpConnection = new TcpClient();
        static NetworkStream tcpConnectionStream;
        static bool connected;
        static byte[] byteMessage;
        static bool fileSelected;
        public Form1(string[] arg)
        {
            InitializeComponent();
            InitializeTheme();
            if (arg.Count() > 0 && arg[0] == "ValmenyBoot")
            {
                using (RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true))
                {
                    if (rk.OpenSubKey("NekoPavel", true) != null)
                    {
                        openFileDialog.InitialDirectory = rk.OpenSubKey("NekoPavel", true).GetValue("SavePath").ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("Programmet startades inte via valmenyn!" + Environment.NewLine + @"Använd valmenyn som finns under \\dfs\gem$\Lit\IT-Service\Fabriken Solna\Pavels Program", "Fel!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                using (RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true))
                {
                    if (rk.OpenSubKey("NekoPavel", true) != null)
                    {
                        openFileDialog.InitialDirectory = rk.OpenSubKey("NekoPavel", true).GetValue("SavePath").ToString();
                    }
                    else
                    {
                        if (!WindowsIdentity.GetCurrent().Name.StartsWith("GAIA\\gaisys"))
                        {
                            MessageBox.Show("Programmet kördes inte som gaisys!" + Environment.NewLine + "Startar om", "Fel!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            ProcessStartInfo proc = new ProcessStartInfo();
                            proc.WorkingDirectory = Environment.CurrentDirectory;
                            proc.FileName = "runas.exe";
                            proc.Arguments = $"/user:gaia\\gaisys{WindowsIdentity.GetCurrent().Name.Substring(5)} \"{Assembly.GetEntryAssembly().Location}\"";
                            Process.Start(proc);
                            Environment.Exit(0);
                        }
                    }
                }
            }
        }
        private void InitializeTheme()
        {
            if (ThemeExists())
            {
                SetColours(OpenTheme(GetSelectedTheme()));
            }
        }

        private string GetSelectedTheme()
        {
            using (RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true))
            {
                return rk.OpenSubKey("NekoPavel", true).GetValue("ThemeName").ToString();
            }
        }

        private Color[] OpenTheme(string themeName)
        {
            using (RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true))
            {
                Color[] colours = {
                Color.FromArgb((int)rk.OpenSubKey($"NekoPavel\\{themeName}").GetValue("BackColour")),
                Color.FromArgb((int)rk.OpenSubKey($"NekoPavel\\{themeName}").GetValue("ForeColour")),
                Color.FromArgb((int)rk.OpenSubKey($"NekoPavel\\{themeName}").GetValue("TextColour"))
                };
                return colours;
            }
        }

        private bool ThemeExists()
        {
            using (RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey("SOFTWARE", true))
            {
                return (rk.OpenSubKey("NekoPavel", true) != null && rk.OpenSubKey("NekoPavel", true).GetValue("ThemeName") != null && rk.OpenSubKey("NekoPavel", true).OpenSubKey(rk.OpenSubKey("NekoPavel", true).GetValue("ThemeName").ToString()) != null);
            }
        }

        private void SetColours(Color[] colours)
        {
            foreach (var control in Controls)
            {
                ((Control)control).BackColor = colours[1];
                ((Control)control).ForeColor = colours[2];
            }
            BackColor = colours[0];
            ForeColor = colours[2];
            label1.BackColor = colours[0];
            openFileBtn.BackColor = colours[0];
        }

        public void WriteToTextbox(string input)
        {
            if (input != "")
            {
                this.Invoke(new Action(() =>
                {
                    outTextBox.AppendText(input + Environment.NewLine);
                }));
            }
        }

        private Computer GetComputerFromSysMan(string pcName)
        {
            try
            {
                IdLookup idLookup = JsonSerializer.Deserialize<IdLookup>(NetResponseToString("http://sysman.sll.se/SysMan/api/Client?name=" + pcName + "&take=1&skip=0&type=0&targetActive=1"));
                PcLookup pcLookup = JsonSerializer.Deserialize<PcLookup>(NetResponseToString("http://sysman.sll.se/SysMan/api/client?id=" + idLookup.result[0].id + "&name=" + idLookup.result[0].name + "&assetTag=" + idLookup.result[0].name));
                InfoLookup infoLookup = JsonSerializer.Deserialize<InfoLookup>(NetResponseToString("http://sysman.sll.se/SysMan/api/Reporting/Client?clientId=" + idLookup.result[0].id));

                if (pcLookup.serialNumber == null)
                {
                    pcLookup.serialNumber = infoLookup.serial;
                }
                return new Computer(pcLookup.name, pcLookup.serialNumber, pcLookup.macAddress);

            }
            catch (Exception e)
            {
                WriteToTextbox(e.Message);
                return null;
            }
        }
        private string NetResponseToString(string uriString)
        {
            Uri uri = new Uri(uriString);
            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            WebResponse webResponse = webRequest.GetResponse();
            Stream receiveStream = webResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(receiveStream, encode);
            Char[] read = new Char[256];

            int count = readStream.Read(read, 0, 256);
            string output = "";
            while (count > 0)
            {
                String Str = new String(read, 0, count);
                output += Str;
                count = readStream.Read(read, 0, 256);
            }
            readStream.Close();
            webResponse.Close();
            return output;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            runButton.Enabled = false;
            pcNameTextBox.Enabled = false;
            bool inputSuccess = false;
            string address = "10.127.21.135";
            int port = 31337;
            if (fileSelected)
            {
                try
                {
                    if (!connected)
                    {
                        WriteToTextbox("Kopplar upp mot utskriftserver");
                        TcpClient tempTcp = new TcpClient();
                        tempTcp.Connect(address, port);
                        NetworkStream tempTcpStream = tempTcp.GetStream();
                        int connectionPort = int.Parse(IncomingMessage(tempTcpStream));
                        tempTcp.Close();
                        tempTcp.Dispose();
                        tempTcpStream.Close();
                        tempTcpStream.Dispose();
                        tcpConnection.Connect(address, connectionPort);
                        tcpConnectionStream = tcpConnection.GetStream();
                        byteMessage = Encoding.Default.GetBytes("~connect~" + Environment.MachineName);
                        try
                        {
                            tcpConnectionStream.Write(byteMessage, 0, byteMessage.Length);
                        }
                        catch (Exception ex)
                        {
                            WriteToTextbox("Error: " + ex.ToString());
                            runButton.Enabled = true;
                            pcNameTextBox.Enabled = true;
                        }
                        WriteToTextbox("Uppkopplad");
                        connected = true;
                    }
                    DataSet result;
                    using (var stream = File.Open(pcNameTextBox.Text, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            result = reader.AsDataSet();
                        }
                    }
                    for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                    {
                        string tempPc = result.Tables[0].Rows[i][0].ToString();
                        WriteToTextbox($"Söker efter dator {tempPc} i Sysman...");
                        Computer pcObject = GetComputerFromSysMan(tempPc);
                        WriteToTextbox("Skickar datorinfo till server");
                        string pcObjectString = JsonSerializer.Serialize<Computer>(pcObject);
                        byteMessage = Encoding.Default.GetBytes("/print" + pcObjectString);
                        try
                        {
                            tcpConnectionStream.Write(byteMessage, 0, byteMessage.Length);
                        }
                        catch (Exception ex) {
                            WriteToTextbox("Error: " + ex.ToString());
                            runButton.Enabled = true;
                            pcNameTextBox.Enabled = true;
                        }
                        WriteToTextbox("Info skickad");
                    }
                    runButton.Enabled = true;
                    pcNameTextBox.Enabled = true;
                }
                catch (Exception ex)
                {
                    WriteToTextbox("Error: " + ex.ToString());
                    runButton.Enabled = true;
                    pcNameTextBox.Enabled = true;
                }
            }
            else
            {
                try
                {
                    pcNameTextBox.Text = pcNameTextBox.Text.Trim();
                    if (pcNameTextBox.Text.Length != 13)
                    {
                        WriteToTextbox("Error: Felaktig inmatning av datornamn");
                        runButton.Enabled = true;
                        pcNameTextBox.Enabled = true;
                    }
                    else
                    {
                        inputSuccess = true;
                    }

                }
                catch (Exception ex)
                {
                    WriteToTextbox("Error: " + ex.ToString());
                    runButton.Enabled = true;
                    pcNameTextBox.Enabled = true;
                }
                if (inputSuccess)
                {
                    WriteToTextbox("Söker efter dator i Sysman...");
                    Computer pcObject = GetComputerFromSysMan(pcNameTextBox.Text);
                    if (pcObject != null)
                    {
                        WriteToTextbox("Dator hittad");
                        try
                        {
                            if (!connected)
                            {
                                WriteToTextbox("Kopplar upp mot utskriftserver");
                                //En temporär uppkoppling skapas
                                TcpClient tempTcp = new TcpClient();
                                tempTcp.Connect(address, port);
                                NetworkStream tempTcpStream = tempTcp.GetStream();
                                int connectionPort = int.Parse(IncomingMessage(tempTcpStream));
                                tempTcp.Close();
                                tempTcp.Dispose();
                                tempTcpStream.Close();
                                tempTcpStream.Dispose();
                                tcpConnection.Connect(address, connectionPort);
                                tcpConnectionStream = tcpConnection.GetStream();

                                byteMessage = Encoding.Default.GetBytes("~connect~" + Environment.MachineName);
                                try
                                {
                                    tcpConnectionStream.Write(byteMessage, 0, byteMessage.Length);
                                }
                                catch (Exception ex)
                                {
                                    WriteToTextbox("Error: " + ex.ToString());
                                    runButton.Enabled = true;
                                    pcNameTextBox.Enabled = true;
                                }
                                WriteToTextbox("Uppkopplad");
                                connected = true;
                            }
                            WriteToTextbox("Skickar datorinfo till server");
                            string pcObjectString = JsonSerializer.Serialize<Computer>(pcObject);
                            byteMessage = Encoding.Default.GetBytes("/print" + pcObjectString);
                            try
                            {
                                tcpConnectionStream.Write(byteMessage, 0, byteMessage.Length);
                            }
                            catch (Exception ex)
                            {
                                WriteToTextbox("Error: " + ex.ToString());
                                runButton.Enabled = true;
                                pcNameTextBox.Enabled = true;
                            }
                            WriteToTextbox("Info skickad");

                            runButton.Enabled = true;
                            pcNameTextBox.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            WriteToTextbox("Error: " + ex.ToString());
                            runButton.Enabled = true;
                            pcNameTextBox.Enabled = true;
                        }
                    }
                    else
                    {
                        WriteToTextbox("Datorinfo kunde inte hämtas från Sysman, se felmeddelande.");
                    }

                }

            }
        }
        public string IncomingMessage(NetworkStream stream)
        {
            byte[] byteRead = new byte[256];
            string readMessage = "";
            int byteReadSize = 0;
            try
            {
                byteReadSize = stream.Read(byteRead, 0, byteRead.Length);
            }
            catch { }
            for (int j = 0; j < byteReadSize; j++)
            {
                readMessage += Convert.ToChar(byteRead[j]);
            }
            return readMessage;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (connected)
            {
                WriteToTextbox("Kopplar bort från servern");
                byteMessage = Encoding.Default.GetBytes("~disconnect~");
                try
                {
                    tcpConnectionStream.Write(byteMessage, 0, byteMessage.Length);
                }
                catch (Exception ex)
                {
                    WriteToTextbox("Error: " + ex.ToString());
                    runButton.Enabled = true;
                    pcNameTextBox.Enabled = true;
                }
                WriteToTextbox("Frånkopplad");
                tcpConnection.Close();
            }
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                pcNameTextBox.Text = openFileDialog.FileName;
                fileSelected = true;
            }
        }

        private void pcNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (fileSelected)
            {
                fileSelected = false;
                pcNameTextBox.Clear();
            }

        }
    }
}
