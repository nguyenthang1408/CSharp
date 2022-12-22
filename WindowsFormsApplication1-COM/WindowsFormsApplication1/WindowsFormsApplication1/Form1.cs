using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Ports;

namespace WindowsFormsApplication1
{
    public enum SoftElemType
    {
        //AM600
        ELEM_QX = 0,     //QX元件   //Các thành phần QX
        ELEM_MW = 1,     //MW元件   // Phẩn tử MW
        ELEM_X = 2,		 //X元件(对应QX200~QX300)  //Phần tử X (tương ứng với QX200 ~ QX300)
        ELEM_Y = 3,		 //Y元件(对应QX300~QX400)  // Phần tử X (tương ứng với QX300 ~ QX400)

        //H3U
        REGI_H3U_Y = 0x20,       //Y元件的定义	    //  Định nghĩa phần tử Y
        REGI_H3U_X = 0x21,		//X元件的定义	    //  Định nghĩa phần tử X					
        REGI_H3U_S = 0x22,		//S元件的定义		//  Định nghĩa phần tử S		
        REGI_H3U_M = 0x23,		//M元件的定义		//  Định nghĩa phần tử M					
        REGI_H3U_TB = 0x24,		//T位元件的定义		// 	Định nghĩa phần tử T-bit	
        REGI_H3U_TW = 0x25,		//T字元件的定义		// 	Định nghĩa các thành phần từ T	
        REGI_H3U_CB = 0x26,		//C位元件的定义		// 	Định nghĩa phần tử C-bit	
        REGI_H3U_CW = 0x27,		//C字元件的定义		//	Định nghĩa các thành phần từ C	
        REGI_H3U_DW = 0x28,		//D字元件的定义		// 	Định nghĩa các thành phần từ D	
        REGI_H3U_CW2 = 0x29,	    //C双字元件的定义 // Định nghĩa của phần tử từ kép C
        REGI_H3U_SM = 0x2a,		//SM
        REGI_H3U_SD = 0x2b,		//
        REGI_H3U_R = 0x2c,		//
        //H5u
        REGI_H5U_Y = 0x30,       //Y元件的定义	
        REGI_H5U_X = 0x31,		//X元件的定义							
        REGI_H5U_S = 0x32,		//S元件的定义				
        REGI_H5U_M = 0x33,		//M元件的定义	
        REGI_H5U_B = 0x34,       //B元件的定义
        REGI_H5U_D = 0x35,       //D字元件的定义
        REGI_H5U_R = 0x36,       //R字元件的定义

    }
    public partial class Form1 : Form
    {
        #region //标准库
        [DllImport("StandardModbusApi.dll", EntryPoint = "Init_ETH_String", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Init_ETH_String(string sIpAddr, int nNetId = 0, int IpPort = 502);

        [DllImport("StandardModbusApi.dll", EntryPoint = "Exit_ETH", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Exit_ETH(int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H3u_Write_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H3u_Write_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H3u_Read_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H3u_Read_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H3u_Read_Soft_Elem_Float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H3u_Read_Soft_Elem_Float(SoftElemType eType, int nStartAddr, int nCount, float[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Write_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Write_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Read_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Read_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Read_Soft_Elem_Float", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Read_Soft_Elem_Float(SoftElemType eType, int nStartAddr, int nCount, float[] pValue, int nNetId = 0);


        [DllImport("StandardModbusApi.dll", EntryPoint = "Am600_Write_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Am600_Write_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "Am600_Read_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Am600_Read_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        #endregion

        string stri;
        int start = 0;
        SerialPort P = new SerialPort();
        Setting st = new Setting();
        Form2 f2 = new Form2();
        Form4 f4 = new Form4();
        ScanerHook scaner2 = new ScanerHook();
        Thread thrd;
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\kepkep.db;pooling=true;FailIfMissing=false");
        public Form1()
        {
            InitializeComponent();
            f4.ShowDialog();
            //scaner2.ScanerEvent += Scaner_ScanerEvent;
            P.DataReceived += new SerialDataReceivedEventHandler(P_DataReceived);
        }

        //private void Scaner_ScanerEvent(ScanerHook.ScanerCodes codes)
        //{
        //    WriteD200_Excute();
        //    {
        //        Delay(100);
        //        string scan_result = codes.Result;
        //        txt_serial.Text = scan_result.ToString();
        //        ReadD201_Results_PLC();
        //        con.Open();
        //        SQLiteCommand cmd = new SQLiteCommand("INSERT into data(Serial,StationName,EmpNO,Results,DateTime) values ('" + txt_serial.Text + "','" + lb_stationName.Text + "','" + txt_empNO.Text + "','" + btn_hienthi.Text + "','" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "')", con);
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //        POST();
        //    }
        //    WriteD204_Reset();
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectPLC();
            WriteD204_Reset();
            txt_empNO.Text = f4.txt_empCode.Text;

            P.PortName = st.com;
            P.BaudRate = int.Parse(st.baud);
            P.Open();

            //scaner2.Start();
            timer1.Start();

            thrd = new Thread(ThreadTask);
            thrd.IsBackground = true;
            thrd.Start();
        }

        private void P_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Delay(200);
            stri = P.ReadExisting();
            Display(stri);
        }

        private void Display(string stri)
        {
            //WriteD200_Excute();   
            if (start == 1)
            {
                WriteD200_Excute();
                txt_serial.Text = Convert.ToString(stri);
                txt_serial.BackColor = Color.DarkKhaki;
                ReadD201_Results_PLC();
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("INSERT into data(Serial,StationName,EmpNO,Results,DateTime) values ('" + txt_serial.Text + "','" + lb_stationName.Text + "','" + txt_empNO.Text + "','" + btn_hienthi.Text + "','" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
                POST();
                WriteD204_Reset();
            }

            //ReadD201_Results_PLC();
            //con.Open();
            //SQLiteCommand cmd = new SQLiteCommand("INSERT into data(Serial,StationName,EmpNO,Results,DateTime) values ('" + txt_serial.Text + "','" + lb_stationName.Text + "','" + txt_empNO.Text + "','" + btn_hienthi.Text + "','" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "')", con);
            //cmd.ExecuteNonQuery();
            //con.Close();
            //POST();
            //WriteD204_Reset();
        }

        private void ThreadTask()
        {

            while (true)
            {
                ReadD200_Start_PLC();
                if (lb_time.Text == "07:00:00" || lb_time.Text == "19:00:00")  
                {
                    P.Close();
                    //scaner2.Stop();
                    f4.ShowDialog();
                    txt_empNO.Text = f4.txt_empCode.Text;
                    //scaner2.Start();
                    P.Open();
                }

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime datetime = DateTime.Now.Add(new TimeSpan());
            {
                lb_time.Text = string.Format("{0:HH:mm:ss}", datetime);
                lb_date.Text = string.Format("{0:dd/MM/yyyy}", datetime);
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void ConnectPLC()
        {
            int nNetId = 0;
            int nIpPort = 502;
            bool result = Init_ETH_String("192.168.1.1", nNetId, nIpPort);
            try
            {
                if (result == true)
                {
                    lb_conncetPLC.Text = "PLC Connected ✔";
                    lb_conncetPLC.ForeColor = Color.Green;
                }
                else
                {
                    lb_conncetPLC.Text = "PLC Disconnected ❌";
                    lb_conncetPLC.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                   
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                thrd.Abort();
                Delay(300);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void ReadD201_Results_PLC()
        {
            Delay(1000);
            int nStartAddr = 201;
            int nCount = 2;
            byte[] pValue = new byte[2];    //缓冲区
            int nNetId = 0;
            int nRet = H3u_Read_Soft_Elem(SoftElemType.REGI_H3U_DW, nStartAddr, nCount, pValue, nNetId);

            ushort nValue = (ushort)(pValue[0] + pValue[1] * 256);

            if (nValue == 10)
                {   
                    btn_hienthi.BackColor = Color.DarkKhaki;
                    btn_hienthi.Text = "PASS";
                }
            else if (nValue == 11)
                {
                    btn_hienthi.BackColor = Color.IndianRed;
                    btn_hienthi.Text = "FAIL";
                }
            else
                {
                    btn_hienthi.BackColor = Color.GhostWhite;
                    btn_hienthi.Text = "Chua co phan hoi tu PLC";
                }
        }
        
        private void WriteD204_Reset() 
        {
            {
                int nStartAddr = 204;                   //寄存器地址
                int nCount = 1;                         //寄存器个数
                byte[] pValue = new byte[2];            //缓冲区
                byte[] pvalue2 = new byte[2];
                int nNetId = 0;                         //连接id
                ushort nValue = 1, nValue1 = 0;

                pValue[0] = (byte)(nValue % 256);
                pValue[1] = (byte)(nValue / 256);

                pvalue2[0] = (byte)(nValue1 % 256);
                pvalue2[1] = (byte)(nValue1 / 256);

                int nRet;
                nRet = H3u_Write_Soft_Elem(SoftElemType.REGI_H3U_DW, nStartAddr, nCount, pValue, nNetId);
                Thread.Sleep(20);
                nRet = H3u_Write_Soft_Elem(SoftElemType.REGI_H3U_DW, nStartAddr, nCount, pvalue2, nNetId);
            }
        }

        public void ReadD200_Start_PLC()
        {
            int nStartAddr = 200;
            int nCount = 2;
            byte[] pValue = new byte[2];             //缓冲区
            int nNetId = 0;
            int nRet = H3u_Read_Soft_Elem(SoftElemType.REGI_H3U_DW, nStartAddr, nCount, pValue, nNetId);
            ushort nValue = (ushort)(pValue[0] + pValue[1] * 256);

            if (nValue == 1)
            {
                start = 1;
                txt_response.Text = "Waiting...";
                txt_serial.Text = "Quét mã sản phẩm";
                btn_hienthi.Text = "Waiting...";
                btn_hienthi.BackColor = Color.WhiteSmoke;
                txt_response.BackColor = Color.WhiteSmoke;
                txt_serial.BackColor = Color.WhiteSmoke;
                //scaner2.Start();
                Delay(1000);
            }
            if (nValue == 0)
            {
                start = 0;
                //scaner2.Stop();
            }
        }
        
        public void POST()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://10.222.48.213:8888/v2/pass/mes/tsc/TSC_VN/clipThroughStation"); // create a request using URL
            request.Method = "POST";       
            request.Timeout = 20000;
            ServicePointManager.Expect100Continue = false;
            request.Accept = "application/json";
            request.ContentType = "application/json;";       //set the content type property of the Webrequest
            string postData = @"{""sn"":""macode"",
""stationName"":""tenmay"",
""empNo"":""manhanvien"",
""result"":""ketqua""
}";                            
            //var json = JsonConvert.SerializeObject(postData); 
            //ASCIIEncoding endcoding = new ASCIIEncoding();
            //Byte[] bytes = endcoding.GetBytes(json);
            var str1 = postData.Replace("macode", txt_serial.Text.Trim());
            var str2 = str1.Replace("tenmay", lb_stationName.Text);
            var str3 = str2.Replace("manhanvien", txt_empNO.Text);
            var str4 = str3.Replace("ketqua", btn_hienthi.Text);
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(str4);           //write the data to request stream 
                }
                var reponse = (HttpWebResponse)request.GetResponse();                        //get the response 
                using (var streamReader = new StreamReader(reponse.GetResponseStream()))    //open the stream using a stream Reader
                {
                    var resultResponse = streamReader.ReadToEnd();
                    txt_response.Text = resultResponse;
                    if (resultResponse.Contains("200") == true)
                    {
                        txt_response.BackColor = Color.DarkKhaki;
                    }
                    else
                    {
                        txt_response.BackColor = Color.IndianRed;
                    }
                }
                reponse.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("POST() Error:" +ex.Message);
                txt_response.Text = Convert.ToString(ex);
                txt_response.BackColor = Color.IndianRed;
            }
        }

        private void WriteD200_Excute()
        {
            {
                int nStartAddr = 200;                   //寄存器地址
                int nCount = 1;                         //寄存器个数
                byte[] pValue = new byte[2];            //缓冲区
                int nNetId = 0;                         //连接id
                ushort nValue = 2;

                //把要写的数据存入缓冲区，备写
                pValue[0] = (byte)(nValue % 256);
                pValue[1] = (byte)(nValue / 256);
                //调用api写数据
                int nRet;
                nRet = H3u_Write_Soft_Elem(SoftElemType.REGI_H3U_DW, nStartAddr, nCount, pValue, nNetId);
            }
        }

        private void cOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
             MessageBox.Show("Setting.settings: com: COM2 + baud: 115200","Configured COM Connection ");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            P.Close();
        }

        /*
        private void button3_Click(object sender, EventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://10.222.48.213:8888/v2/pass/mes/tsc/TSC_VN/clipThroughStation"); // create a request using URL
            request.Method = "POST"; // method
            request.Timeout = 25000;
            ServicePointManager.Expect100Continue = false;
            //request.Accept = "application/json";
            request.ContentType = "application/json;"; //x-www-form-urlencoded // set the content type property of the Webrequest
            string postData = @"{""sn"":"""",
""stationName"":"""",
""empNo"":"""",
""result"":""""
}"; // example json data
            int length = postData.Length;
            textBox1.Text = Convert.ToString(length);
            //var json = JsonConvert.SerializeObject(postData); 
            //ASCIIEncoding endcoding = new ASCIIEncoding();
            //Byte[] bytes = endcoding.GetBytes(json);
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData); // write the data to request stream
            }
            try
            {
                var reponse = (HttpWebResponse)request.GetResponse(); // get the response 
                using (var streamReader = new StreamReader(reponse.GetResponseStream())) // open the stream using a stream Reader
                {
                    var resultResponse = streamReader.ReadToEnd(); // read the response
                    MessageBox.Show("" + resultResponse);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        */
    }
}
