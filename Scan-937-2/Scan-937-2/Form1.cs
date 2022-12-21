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

namespace Scan_937_2
{
    public partial class Form1 : Form
    {

        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;
        
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
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
                            Socket client = server.Accept();
                            clientList.Add(client);
                            button1.BackColor = Color.Green;
                            button1.Text = "Connecting";

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
            listView1.Items.Add(new ListViewItem() { Text = s });
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

      
    }
}
