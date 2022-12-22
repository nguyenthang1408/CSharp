using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            P2.DataReceived += new SerialDataReceivedEventHandler(P_DataReceived2);
        }
        Setting st = new Setting();
        SerialPort P2 = new SerialPort();
        // ScanerHook scaner1;
        string str;
        //private void Scaner_ScanerEvent(ScanerHook.ScanerCodes codes)
        //{
        //    Delay(200);
        //    string scan_result = codes.Result;
        //    txt_empCode.Text = scan_result.ToString();
        //    string first_char = scan_result.Substring(0, 1);
        //    string numberOfcode = scan_result.Substring(1);
        //    bool a = true;
        //    for (int i = 0; i < numberOfcode.Length; i++)
        //    {
        //        a = char.IsNumber(numberOfcode, i);
        //        if (a == false)
        //        {
        //            break;
        //        }
        //    }
        //    if (first_char != "V" || a == false || scan_result.Length !=8)
        //    {
        //        txt_empCode.BackColor = Color.Red;
        //        lb_confirm.Text = "Mã không hợp lệ, thử lại !";
        //        lb_confirm.BackColor = Color.Red;
        //    }
        //    if (first_char == "V" && scan_result.Length == 8 && a == true)
        //    {
        //        txt_empCode.BackColor = Color.LawnGreen;
        //        lb_confirm.Text = "Xác nhận mã thành công !";
        //        lb_confirm.BackColor = Color.LawnGreen;
        //        Delay(2000);
        //        Close();
        //    }
        //}
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

        private void Form4_Load(object sender, EventArgs e)
        {
            P2.PortName = st.com;
            P2.BaudRate = int.Parse(st.baud);
            P2.Open();

            txt_empCode.Text = "";
            txt_empCode.BackColor = Color.White;
            lb_confirm.Text = "";


            //scaner1 = new ScanerHook();
            //scaner1.Start();
            //scaner1.ScanerEvent += Scaner_ScanerEvent;
        }

        private void P_DataReceived2(object sender, SerialDataReceivedEventArgs e)
        {
            Delay(100);
            str = P2.ReadExisting();
            Display(str);
        }

        private void Display(string str)
        {
            try
            {
                txt_empCode.Text = Convert.ToString(str);
                if (str.Length < 10)
                {
                    lb_confirm.Text = "Xác nhận mã thành công!";
                    lb_confirm.BackColor = Color.LawnGreen;
                    Delay(2000);
                    this.Close();
                }
                else
                {
                    lb_confirm.Text = "Mã sai, thử lại!";
                    lb_confirm.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            P2.Close();
        }
    }
}
