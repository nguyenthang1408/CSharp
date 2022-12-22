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
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace Scan_937
{
    public partial class Form1 : Form
    {
        IPEndPoint IP;
        Socket client;
        SerialPort P = new SerialPort();
        Settings1 st = new Settings1();
        Form2 f2 = new Form2();
        
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
            P.DataReceived += new SerialDataReceivedEventHandler(P_DataReceived);
        }

        private void P_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Delay(200);
            string a = P.ReadExisting();
            DateTime localDate = DateTime.Now;
            

            if (button2.BackColor == Color.Green)
            {
                Send(a);
                listView1.Items.Add(new ListViewItem() { Text = "Connecting! " + localDate + a });
            }
            else
            {
                listView1.Items.Add(new ListViewItem() { Text = "Connecting! " + localDate + a });
                button2.BackColor = Color.Red;
                button2.Text = "Disconnect";
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string a = "Scan-1234";
            string a = textBox1.Text;
            DateTime localDate = DateTime.Now;
            
            if (button2.BackColor == Color.Green)
            {
                Send(a);
                listView1.Items.Add(new ListViewItem() { Text = "Connecting! " + localDate + "-" + a });
            }
            else
            {
                listView1.Items.Add(new ListViewItem() { Text = "Disconnect ! " + localDate + "-" + a });
                button2.BackColor = Color.Red;
                button2.Text = "Disconnect !";
            }
        }

        void Connect()
        {
            timer1.Start();

          
        }

        void Close()
        {
            if (button2.BackColor == Color.Red || button2.BackColor == Color.Green)
            {
                if (client.Connected)
                {
                    client.Close(); 
                }
            }
        }
        
        void Send(string a)
        {
            if (a != string.Empty)
            {
                client.Send(Serialize(a));
            }
            button2.Text = a;

        }


        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, obj);

            return stream.ToArray();
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            Thread listen = new Thread(() => {
                try
                {
                    client.Connect(IP);
                    button2.BackColor = Color.Green;
                    button2.Text = "Connecting";
                }
                catch
                {
                    button2.BackColor = Color.Red;
                    button2.Text = "Disconnect";
                }
            });
            listen.IsBackground = true;
            listen.Start();
          
        }

        private void cOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f2.ShowDialog();
        }
    

    }
}
