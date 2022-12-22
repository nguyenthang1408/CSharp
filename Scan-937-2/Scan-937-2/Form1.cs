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
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace Scan_937_2
{
    public partial class Form1 : Form
    {

        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;
        Form2 f2 = new Form2();
        SerialPort P = new SerialPort();
        Settings1 st = new Settings1();
        
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            P.DataReceived += new SerialDataReceivedEventHandler(P_DataReceived);
            Connect();
        }


        private void P_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (button1.Text == "NG")
            {
                Delay(200);
                string a = P.ReadExisting();
                DateTime localDate = DateTime.Now;
                listView1.Items.Add(new ListViewItem() { Text = "Connecting! " + localDate + a });

                if (button1.BackColor == Color.Green)
                {

                }
                else
                {
                    button1.BackColor = Color.Red;
                    button1.Text = "Disconnect";
                }
            }
            else
            {
                MessageBox.Show("Please Reset NG");
            }
        }

        #region**********延时**********
        [DllImport("kernel32.dll")]
        static extern uint GetTickCount();
        static void Delay(uint i)   //延時
        {
            uint start = GetTickCount();
            while (GetTickCount() - start < i)
            {
                Application.DoEvents();
                CheckForIllegalCrossThreadCalls = false;
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime localDate = DateTime.Now;

        }

        void Connect()
        {
            clientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, 9999);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            server.Bind(IP);
   

            Thread Listen = new Thread(() => {
                 while (true)
                 {
                    try
                    {
                            server.Listen(100);
                            button1.BackColor = Color.Green;
                            button1.Text = "Connecting...";
                            Socket client = server.Accept();
                            clientList.Add(client);
                           

                            Thread receive = new Thread(Receive);
                            receive.IsBackground = true;
                            receive.Start(client);
             
                    }
                    catch
                    {
                        button1.BackColor = Color.Red;
                        button1.Text = "Disconnect";
                        IP = new IPEndPoint(IPAddress.Any, 9999);
                        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    }
                 }

                
               
            });
            Listen.IsBackground = true;
            Listen.Start();
        }


        void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    string massage = (string)Deserialize(data);
                    AddMassage(massage);

                    Thread.Sleep(1000);
                }
            }
            catch
            {
              
            }
        }


        void AddMassage(string s)
        {
            string a = button1.Text;
            DateTime localDate = DateTime.Now;
            listView1.Items.Add(new ListViewItem() { Text = s + "-" + localDate });
            if (textBox1.Text == "")
            {
                button2.Text = s;
            }
            else if (textBox2.Text == "")
            {
                button3.Text = s;
            }
            else if (textBox3.Text == "")
            {
                button4.Text = s;
            }
            else if (textBox4.Text == "")
            {
                button5.Text = s;
            }
            else if (textBox5.Text == "")
            {
                button6.Text = s;
            }
            else if (textBox6.Text == "")
            {
                button7.Text = s;
            }
            else if (textBox7.Text == "")
            {
                button8.Text = s;
            }
            
            
        }

        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                while (true)
                {
                    server.Listen(100);
                    Socket client = server.Accept();
                    clientList.Add(client);

                    Thread receive = new Thread(Receive);
                    receive.IsBackground = true;
                    receive.Start(client);
                }
            }
            catch
            {
                IP = new IPEndPoint(IPAddress.Any, 9999);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                button1.BackColor = Color.Red;
                button1.Text = "Connecting";
            }
        }

        private void cOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f2.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ListCOM = SerialPort.GetPortNames();
            int[] ListBoaud = { 2400, 4800, 9600, 19200, 115200 };
            Array.Sort(ListCOM);
            for (int i = 0; i < ListCOM.Length; i++)
            {
                f2.comboBox1.Items.Add(ListCOM[i]);
            }
            for (int i = 0; i < ListBoaud.Length; i++)
            {
                f2.comboBox2.Items.Add(ListBoaud[i]);
            }
            f2.comboBox1.Text = st.com;
            f2.comboBox2.Text = st.baudrate;

            button1.Text = "Scan-1234";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Text = "";
            textBox1.Text = "";
            button8.Text = "";
            button2.Text = "";
            button2.BackColor = Color.Transparent;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            button8.Text = "";
            button1.Text = "";
            button3.Text = "";
            button3.BackColor = Color.Transparent;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            button8.Text = "";
            button1.Text = "";
            button4.Text = "";
            button4.BackColor = Color.Transparent;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            button8.Text = "";
            button1.Text = "";
            button5.Text = "";
            button5.BackColor = Color.Transparent;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            button8.Text = "";
            button1.Text = "";
            button6.Text = "";
            button6.BackColor = Color.Transparent;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
            button8.Text = "";
            button1.Text = "";
            button7.Text = "";
            button7.BackColor = Color.Transparent;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
            button8.Text = "";
            button1.Text = "";
            button8.Text = "";
            button8.BackColor = Color.Transparent;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                if (button1.Text != "NG")
                {

                    if (textBox1.Text == "" && textBox8.Text == button2.Text)
                    {
                        textBox1.Text = textBox8.Text + "OK";
                        button2.Text = "OK";
                        button2.BackColor = Color.Green;
                        button1.Text = "";
                        textBox8.Text = "";
                        return;
                    }

                    else if (textBox2.Text == "" && textBox8.Text == button3.Text)
                    {
                        textBox2.Text = textBox8.Text + "OK";
                        button3.Text = "OK";
                        button3.BackColor = Color.Green;
                        button1.Text = "";
                        textBox8.Text = "";
                        return;
                    }
                    else if (textBox3.Text == "" && textBox8.Text == button4.Text)
                    {
                        textBox3.Text = textBox8.Text + "OK";
                        button4.Text = "OK";
                        button4.BackColor = Color.Green;
                        button1.Text = "";
                        textBox8.Text = "";
                        return;
                    }
                    else if (textBox4.Text == "" && textBox8.Text == button5.Text)
                    {
                        textBox4.Text = textBox8.Text + "OK";
                        button5.Text = "OK";
                        button5.BackColor = Color.Green;
                        button1.Text = "";
                        textBox8.Text = "";
                        return;
                    }
                    else if (textBox5.Text == "" && textBox8.Text == button6.Text)
                    {
                        textBox5.Text = textBox8.Text + "OK";
                        button6.Text = "OK";
                        button6.BackColor = Color.Green;
                        button1.Text = "";
                        textBox8.Text = "";
                        return;
                    }
                    else if (textBox6.Text == "" && textBox8.Text == button7.Text)
                    {
                        textBox6.Text = textBox8.Text + "OK";
                        button7.Text = "OK";
                        button7.BackColor = Color.Green;
                        button1.Text = "";
                        textBox8.Text = "";
                        return;
                    }
                    else if (textBox7.Text == "" && textBox8.Text == button8.Text)
                    {
                        textBox7.Text = textBox8.Text + "OK";
                        button8.Text = "OK";
                        button8.BackColor = Color.Green;
                        button1.Text = "";
                        textBox8.Text = "";
                        return;
                    }





                    else if (button2.Text != "" && button2.BackColor != Color.Green)
                    {
                        textBox1.Text = "NG";
                        button2.Text = "NG";
                        button2.BackColor = Color.Red;
                        textBox8.Text = "";
                        return;
                    }
                    else if (button3.Text != "" && button3.BackColor != Color.Green)
                    {
                        textBox2.Text = "NG";
                        button3.Text = "NG";
                        button3.BackColor = Color.Red;
                        textBox8.Text = "";
                        return;
                    }
                    else if (button4.Text != "" && button4.BackColor != Color.Green)
                    {
                        textBox3.Text = "NG";
                        button4.Text = "NG";
                        button4.BackColor = Color.Red;
                        textBox8.Text = "";
                        return;
                    }
                    else if (button5.Text != "" && button5.BackColor != Color.Green)
                    {
                        textBox4.Text = "NG";
                        button5.Text = "NG";
                        button5.BackColor = Color.Red;
                        textBox8.Text = "";
                        return;
                    }
                    else if (button6.Text != "" && button6.BackColor != Color.Green)
                    {
                        textBox5.Text = "NG";
                        button6.Text = "NG";
                        button6.BackColor = Color.Red;
                        textBox8.Text = "";
                        return;
                    }
                    else if (button7.Text != "" && button7.BackColor != Color.Green)
                    {
                        textBox6.Text = "NG";
                        button7.Text = "NG";
                        button7.BackColor = Color.Red;
                        textBox8.Text = "";
                        return;
                    }
                    else if (button8.Text != "" && button8.BackColor != Color.Green)
                    {
                        textBox7.Text = "NG";
                        button8.Text = "NG";
                        button8.BackColor = Color.Red;
                        textBox8.Text = "";
                        return;
                    }


                }
                else
                {
                    MessageBox.Show("please reset NG");
                }
            }
        }
            

      
    }
}
