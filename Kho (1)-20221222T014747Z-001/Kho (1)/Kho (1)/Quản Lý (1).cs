using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Kho
{
    public partial class Quản_Lý : Form
    {
        Phiếu_Xuất_Kho f8 = new Phiếu_Xuất_Kho();
        string nhap = "nhap";
        string xuat = "xuat";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + System.Windows.Forms.Application.StartupPath + "\\Data.db;pooling=true;FailIfMissing=false");
        public Quản_Lý()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "1 USD = 22.755 VND";
            progressBar1.Value = 0;
            toolStripStatusLabel1.Text = "Tìm Kiếm Theo Tên Mặt Hàng";
            toolStripStatusLabel1.ForeColor = Color.Black;
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            textBox1.Show();
            textBox2.Hide();
            textBox3.Hide();
            con.Open();
            SQLiteCommand cm = new SQLiteCommand("update manager set giatong =gia*soluong where mascan = mascan", con);
            cm.ExecuteNonQuery();
            SQLiteDataAdapter add = new SQLiteDataAdapter("select giatong from manager", con);
            DataTable dtt = new DataTable();
            add.Fill(dtt);
            SQLiteDataAdapter ad = new SQLiteDataAdapter("select tenmathang[Tên mặt Hàng],quycach[Quy Cách],nhasanxuat[Nhà Sản Xuất],gia[Giá(USD)],soluong[Số Lượng],giatong[Giá Tổng(USD)],mascan[Mã scan] from manager", con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dataGridView1.DataSource = dt;
            label3.Text = (dataGridView1.RowCount - 1).ToString();
            con.Close();
        }

        private void Quản_Lý_Load(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel2.Text = "1 USD = 22.755 VND";
                progressBar1.Value = 0;
                toolStripStatusLabel1.Text = "Tìm Kiếm Theo Tên Mặt Hàng";
                toolStripStatusLabel1.ForeColor = Color.Black;
                textBox1.Enabled = true;
                textBox2.Enabled = false;
                textBox1.Show();
                textBox2.Hide();
                textBox3.Hide();
                con.Open();
                SQLiteCommand cm = new SQLiteCommand("update manager set giatong =gia*soluong where mascan = mascan", con);
                cm.ExecuteNonQuery();
                SQLiteDataAdapter add = new SQLiteDataAdapter("select giatong from manager", con);
                DataTable dtt = new DataTable();
                add.Fill(dtt);
                SQLiteDataAdapter ad = new SQLiteDataAdapter("select tenmathang[Tên mặt Hàng],quycach[Quy Cách],nhasanxuat[Nhà Sản Xuất],gia[Giá(USD)],soluong[Số Lượng],giatong[Giá Tổng(USD)],mascan[Mã scan] from manager", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                label3.Text = (dataGridView1.RowCount - 1).ToString();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string a = dateTimePicker1.Value.ToShortDateString();
            int b = a.IndexOf("/", 1);
            string c = a.Substring(b + 1);
            toolStripStatusLabel2.Text = "";
            progressBar1.Value = 0;
            toolStripStatusLabel1.Text = "Tìm Kiếm Theo Mã Thẻ";
            toolStripStatusLabel1.ForeColor = Color.Black;
            textBox1.Enabled = false;
            textBox2.Enabled = true;
            textBox2.Show();
            textBox1.Hide();
            textBox3.Hide();
            try
            {
                SQLiteDataAdapter ad = new SQLiteDataAdapter("select tenmathang[Tên Mặt Hàng],nhasanxuat[Nhà Sản Xuất],quycach[Quy Cách],soluong[Số Lượng],mathe[Mã Thẻ],nguoimuon[Người Mượn],ngaymuon[Ngày Mượn],ngaytra[Ngày Trả] from muontra", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                label3.Text = (dataGridView1.RowCount - 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string a = dateTimePicker1.Value.ToShortDateString();
            int b = a.IndexOf("/", 1);
            string c = a.Substring(b + 1);
            toolStripStatusLabel2.Text = "";
            progressBar1.Value = 0;
            toolStripStatusLabel1.Text = "Tìm Kiếm Theo Mã Thẻ";
            toolStripStatusLabel1.ForeColor = Color.Black;
            textBox1.Enabled = false;
            textBox2.Enabled = true;
            textBox2.Hide();
            textBox1.Hide();
            textBox3.Show();
            try
            {
                SQLiteDataAdapter ad = new SQLiteDataAdapter("select tenmathang[Tên Mặt Hàng],nhasanxuat[Nhà Sản Xuất],quycach[Quy Cách],soluong[Số Lượng],mathe[Mã Thẻ],nguoimuon[Người Mượn],ngaymuon[Ngày Mượn],ngaytra[Ngày Trả] from muontra1", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                label3.Text = (dataGridView1.RowCount - 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int dataCount = dataGridView1.Rows.Count;
            if (dataCount > 1)
            {
                SaveFileDialog MyFile = new SaveFileDialog();
                MyFile.Filter = "hjh(*.CSV)|*CSV";

                MyFile.FileName = "Kho" + DateTime.Now.ToString("yyyyMMdd");

                if (MyFile.ShowDialog() == DialogResult.OK)
                {
                    string PathStr = MyFile.FileName + ".CSV";
                    progressBar1.Maximum = dataCount - 1;
                    progressBar1.Value = 0;
                    if (!File.Exists(MyFile.FileName + ".CSV"))
                    {
                        try
                        {
                            StreamWriter CSVWriter = new StreamWriter(PathStr, true, Encoding.UTF8);
                            if (toolStripStatusLabel2.Text == "1 USD = 22.755 VND")
                            {
                                CSVWriter.WriteLine("Mã Scan,Tên Mặt Hàng,Quy Cách,Nhà Sản Xuất,Giá,Số Lượng,Giá Tổng");
                                CSVWriter.Flush();
                                CSVWriter.Close();
                            }
                            if (toolStripStatusLabel1.Text == "Tìm Kiếm Theo Mã Thẻ")
                            {
                                CSVWriter.WriteLine("Tên Mặt Hàng,Nhà Sản Xuất,Quy Cách,Số Lượng,Mã Thẻ,Người Mượn,Ngày Mượn,Ngày Trả");
                                CSVWriter.Flush();
                                CSVWriter.Close();
                            }
                            if (toolStripStatusLabel1.Text == "Kiểm kê nhập hàng trong tháng Hiện Tại")
                            {
                                CSVWriter.WriteLine("Tên Mặt Hàng,Giá,Số Lượng Tổng,Giá Tổng");
                                CSVWriter.Flush();
                                CSVWriter.Close();
                            }
                            if (toolStripStatusLabel1.Text == "Kiểm kê xuất hàng trong tháng Hiện Tại")
                            {
                                CSVWriter.WriteLine("Tên Mặt Hàng,Giá,Số Lượng Tổng,Giá Tổng");
                                CSVWriter.Flush();
                                CSVWriter.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        try
                        {
                            StreamWriter CSVWriter = new StreamWriter(PathStr, true, Encoding.UTF8);
                            if (toolStripStatusLabel2.Text == "1 USD = 22.755 VND")
                            {
                                CSVWriter.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString() + "," + dataGridView1.Rows[i].Cells[1].Value.ToString() + "," + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + "," + dataGridView1.Rows[i].Cells[4].Value.ToString() + "," + dataGridView1.Rows[i].Cells[5].Value.ToString() + "," + dataGridView1.Rows[i].Cells[6].Value.ToString());
                                CSVWriter.Flush();
                                CSVWriter.Close();
                            }
                            if (toolStripStatusLabel1.Text == "Tìm Kiếm Theo Mã Thẻ")
                            {
                                CSVWriter.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString() + "," + dataGridView1.Rows[i].Cells[1].Value.ToString() + "," + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString() + "," + dataGridView1.Rows[i].Cells[4].Value.ToString() + "," + dataGridView1.Rows[i].Cells[5].Value.ToString() + "," + dataGridView1.Rows[i].Cells[6].Value.ToString() + "," + dataGridView1.Rows[i].Cells[7].Value.ToString());
                                CSVWriter.Flush();
                                CSVWriter.Close();
                            }
                            if (toolStripStatusLabel1.Text == "Kiểm kê nhập hàng trong tháng Hiện Tại")
                            {
                                CSVWriter.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString() + "," + dataGridView1.Rows[i].Cells[1].Value.ToString() + "," + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString());
                                CSVWriter.Flush();
                                CSVWriter.Close();
                            }
                            if (toolStripStatusLabel1.Text == "Kiểm kê xuất hàng trong tháng Hiện Tại")
                            {
                                CSVWriter.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString() + "," + dataGridView1.Rows[i].Cells[1].Value.ToString() + "," + dataGridView1.Rows[i].Cells[2].Value.ToString() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString());
                                CSVWriter.Flush();
                                CSVWriter.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        progressBar1.Value++;
                    }
                    toolStripStatusLabel1.Text = "Tải Về Thành Công...!!";
                    toolStripStatusLabel1.ForeColor = Color.Green;
                }
            }
            else
            {
                MessageBox.Show("Không Có Dữ Liệu");
                toolStripStatusLabel1.Text = "Tải Về Thất Bại....";
                toolStripStatusLabel1.ForeColor = Color.Red;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SQLiteDataAdapter ad = new SQLiteDataAdapter("select ID,tenmathang[Tên mặt Hàng],nhasanxuat[Nhà Sản Xuất],quycach[Quy Cách],soluong[Số Lượng],mathe[Mã Thẻ],nguoimuon[Người Mượn],ngaymuon[Ngày Mượn],ngaytra[Ngày Trả] from muontra where mathe like '%" + textBox2.Text + "%'", con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dataGridView1.DataSource = dt;
            label3.Text = (dataGridView1.RowCount - 1).ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                SQLiteDataAdapter ad = new SQLiteDataAdapter("select mascan[Mã scan],tenmathang[Tên mặt Hàng],nhasanxuat[Nhà Sản Xuất],gia[Giá($)],soluong[Số Lượng],giatong[Giá Tổng($)],quycach[Quy Cách] from manager where tenmathang like '%" + textBox1.Text + "%'", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                label3.Text = (dataGridView1.RowCount - 1).ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string a = dateTimePicker1.Value.ToShortDateString();
            int b = a.IndexOf("/", 1);
            string c = a.Substring(b + 1);
            SQLiteDataAdapter ad = new SQLiteDataAdapter("select tenmathang[Tên mặt Hàng],nhasanxuat[Nhà Sản Xuất],quycach[Quy Cách],soluong[Số Lượng],mathe[Mã Thẻ],nguoimuon[Người Mượn],ngaymuon[Ngày Mượn],ngaytra[Ngày Trả] from muontra1 where mathe like '%" + textBox3.Text + "%'and (ngaymuon like '%" + c.ToString() + "%'or ngaytra like '%" + c.ToString() + "%')", con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            dataGridView1.DataSource = dt;
            label3.Text = (dataGridView1.RowCount - 1).ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (toolStripStatusLabel1.Text == "Kiểm kê nhập hàng trong tháng Hiện Tại")
            {
                SQLiteDataAdapter ad = new SQLiteDataAdapter("select mascan[Mã scan],tenmathang[Tên mặt Hàng],nhasanxuat[Nhà Sản Xuất],gia[Giá($)],soluong[Số Lượng],giatong[Giá Tổng($)],quycach[Quy Cách] from manager1 where thoigian like '%" + dateTimePicker1.Text + "%' and nhapxuat = '" + nhap.ToString() + "'", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                label3.Text = (dataGridView1.RowCount - 1).ToString();
            }
            if (toolStripStatusLabel1.Text == "Kiểm kê xuất hàng trong tháng Hiện Tại")
            {
                SQLiteDataAdapter ad = new SQLiteDataAdapter("select mascan[Mã scan],tenmathang[Tên mặt Hàng],nhasanxuat[Nhà Sản Xuất],gia[Giá($)],soluong[Số Lượng],giatong[Giá Tổng($)],quycach[Quy Cách] from manager1 where thoigian like '%" + dateTimePicker1.Text + "%' and nhapxuat = '" + xuat.ToString() + "'", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                label3.Text = (dataGridView1.RowCount - 1).ToString();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "Kiểm kê nhập hàng trong tháng Hiện Tại";
                string a = dateTimePicker1.Value.ToShortDateString();
                int b = a.IndexOf("/", 1);
                string c = a.Substring(b + 1);
                SQLiteDataAdapter ad = new SQLiteDataAdapter("select tenmathang[Tên Mặt Hàng],gia[Giá],sum(soluong)[Số Lượng Tổng],sum(giatong)[Giá Tổng] from manager1 where thoigian like '%" + c.ToString() + "%' and nhapxuat = '" + nhap.ToString() + "' group by quycach", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                label3.Text = (dataGridView1.RowCount - 1).ToString();
                SQLiteDataAdapter ad1 = new SQLiteDataAdapter("select sum(giatong) from manager1 where thoigian like '%" + c.ToString() + "%' and nhapxuat = '" + nhap.ToString() + "'", con);
                DataTable dtt1 = new DataTable();
                ad1.Fill(dtt1);
                string s = dtt1.Rows[0][0].ToString();
                toolStripStatusLabel2.Text = "Tổng Tiền Nhập Tháng : " + c.ToString() + " là " + s.ToString() + " USD";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel1.Text = "Kiểm kê xuất hàng trong tháng Hiện Tại";
                toolStripStatusLabel2.Text = "";
                string a = dateTimePicker1.Value.ToShortDateString();
                int b = a.IndexOf("/", 1);
                string c = a.Substring(b + 1);
                SQLiteDataAdapter ad = new SQLiteDataAdapter("select tenmathang[Tên Mặt Hàng],Gia[giá],sum(soluong)[Số Lượng Tổng],sum(giatong)[Giá Tổng] from manager1 where thoigian like '%" + c.ToString() + "%' and nhapxuat ='" + xuat.ToString() + "' group by quycach", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dataGridView1.DataSource = dt;
                label3.Text = (dataGridView1.RowCount - 1).ToString();
                SQLiteDataAdapter ad1 = new SQLiteDataAdapter("select sum(giatong) from manager1 where thoigian like '%" + c.ToString() + "%' and nhapxuat = '" + xuat.ToString() + "'", con);
                DataTable dtt1 = new DataTable();
                ad1.Fill(dtt1);
                string s = dtt1.Rows[0][0].ToString();
                toolStripStatusLabel2.Text = "Tổng Tiền xuất Tháng : " + c.ToString() + " là " + s.ToString() + " USD";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            f8.ShowDialog();
        }
    }
}
