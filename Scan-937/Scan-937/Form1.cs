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

namespace Scan_937
{
    public partial class Form1 : Form
    {
        IPEndPoint IP;
        Socket client;
        
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.BackColor == Color.Green)
            {
                Send();
            }
            else
            {
                button2.BackColor = Color.Red;
                button2.Text = "Disconnect";
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

        void Send()
        {
            if (textBox1.Text != string.Empty)
            {
                client.Send(Serialize(textBox1.Text));
            }
            textBox1.Text = string.Empty;

        }



        void AddMassage(string s)
        {

            //string[] listScan = new string[7];
            //listScan[1] = "sadsa";
           // string[] a = listScan;
            listView1.Items.Add(new ListViewItem() { Text = s });
            textBox1.Clear();
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
    

    }
}
