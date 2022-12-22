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
    public partial class Thêm_Sản_Phẩm : Form
    {
        public string s100;
        string nhap = "nhap";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + System.Windows.Forms.Application.StartupPath + "\\Data.db;pooling=true;FailIfMissing=False");
        public Thêm_Sản_Phẩm()
        {
            InitializeComponent();
        }

        private void Thêm_Sản_Phẩm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int a = Convert.ToInt32(textBox4.Text);
                int b = Convert.ToInt32(textBox5.Text);
            }
            catch
            {
                MessageBox.Show("Số Lượng Và giá Phải là số");
                return;
            }
            try
            {
                con.Open();
                //string aa = string.Format("{0:n}", Convert.ToInt32(textBox5.Text));
                double c = Convert.ToDouble(textBox5.Text);
                double c1 = Convert.ToDouble(textBox4.Text);
                double c3 = c * c1;
                SQLiteCommand cm = new SQLiteCommand("insert into manager(mascan,tenmathang,quycach,nhasanxuat,gia,soluong,giatong,thoigian) values('" + s100.ToString() + "','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + c.ToString() + "','" + textBox4.Text + "','" + c3.ToString() + "','"+DateTime.Now.ToString()+"')", con);
                cm.ExecuteNonQuery();
                SQLiteCommand cm1 = new SQLiteCommand("insert into manager1(mascan,tenmathang,quycach,nhasanxuat,gia,soluong,giatong,thoigian,nhapxuat) values('" + s100.ToString() + "','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + c.ToString() + "','" + textBox4.Text + "','"+(Convert.ToInt32(c)*Convert.ToInt32(textBox4.Text))+"','"+DateTime.Now.ToString()+"','"+nhap.ToString()+"')", con);
                cm1.ExecuteNonQuery();
                MessageBox.Show("Thêm Thành Công");
                con.Close();
                textBox1.ResetText();
                textBox2.ResetText();
                textBox3.ResetText();
                textBox4.ResetText();
                textBox5.ResetText();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
    }
}
