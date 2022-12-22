using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Kho
{
    public partial class Nhập_Hàng : Form
    {
        public string a, b, c,d,e;
        int g = 1;
        string nhap = "nhap";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + System.Windows.Forms.Application.StartupPath + "\\Data.db;pooling=true;FailIFMising=false");
        public Nhập_Hàng()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SQLiteCommand cm = new SQLiteCommand("update manager set soluong =soluong+'" + textBox1.Text + "',giatong = gia*('" + textBox1.Text + "'+soluong) where quycach ='" + label2.Text + "'", con);
                cm.ExecuteNonQuery();
                SQLiteCommand cm1 = new SQLiteCommand("insert into manager1(mascan,tenmathang,quycach,nhasanxuat,gia,soluong,giatong,thoigian,nhapxuat) values('" + d.ToString() + "','" + a.ToString() + "','" + label2.Text+ "','" + b.ToString() + "','" + c.ToString() + "','" + textBox1.Text + "','"+(Convert.ToInt32(c)*Convert.ToInt32(textBox1.Text))+"','" + DateTime.Now.ToString() + "','"+nhap.ToString()+"')", con);
                cm1.ExecuteNonQuery();
                con.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Nhập_Hàng_Load(object sender, EventArgs e)
        {
            label2.Hide();
        }
    }
}
