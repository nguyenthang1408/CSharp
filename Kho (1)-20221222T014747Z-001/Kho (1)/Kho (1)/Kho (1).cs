using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Data.SQLite;
using System.Threading;

namespace Kho
{
    public partial class Kho : Form
    {
        SerialPort sp = new SerialPort();
        ScanerHook c = new ScanerHook();
        Quản_Lý f2 = new Quản_Lý();
        Setting f3 = new Setting();
        Quản_Lý_Mượn_Trả f4 = new Quản_Lý_Mượn_Trả();
        Nhập_Hàng f5 = new Nhập_Hàng();
        Thêm_Sản_Phẩm f6 = new Thêm_Sản_Phẩm();
        Xuất_Hàng f7 = new Xuất_Hàng();
        Phiếu_Xuất_Kho f8 = new Phiếu_Xuất_Kho();
        Settings1 st = new Settings1();
        public string s1;
        bool m;
        string xuat ="xuat";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + System.Windows.Forms.Application.StartupPath + "\\Data.db;pooling=true;FailIfMissing=False");
        public Kho()
        {
            InitializeComponent();
            c.ScanerEvent += listener_ScanerEvent;
            sp.DataReceived += new SerialDataReceivedEventHandler(seri);
        }
        //quét mã bằng cổng COM
        void seri(object sender, SerialDataReceivedEventArgs e)
        {
            textBox1.Text = sp.ReadExisting();
            f6.s100 = textBox1.Text;
            string s = textBox1.Text;
            con.Open();
            try
            {
                if (toolStripStatusLabel2.Text != "")
                {
                    if (textBox1.Text != "")
                    {
                        //Nhập Hàng

                        if (toolStripStatusLabel2.Text == "Đã Chọn Nhập Hàng")
                        {
                            string str = "select quycach from manager";
                            SQLiteDataAdapter ad = new SQLiteDataAdapter(str, con);
                            DataTable dt = new DataTable();
                            ad.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                s1 = dt.Rows[i][0].ToString();
                                //kiểm tra trùng quy cách
                                if (m = s.Contains(s1))
                                {
                                    DialogResult thongbao;
                                    f5.label2.Text = s1.ToString();
                                    thongbao = MessageBox.Show("Thêm 1 Đơn vị Số Lượng Sản Phẩm", "thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (thongbao == DialogResult.Yes)
                                    {
                                        SQLiteDataAdapter ad1 = new SQLiteDataAdapter("select tenmathang,nhasanxuat,gia from manager where quycach ='" + s1 + "'", con);
                                        DataTable dtt1 = new DataTable();
                                        ad1.Fill(dtt1);
                                        string a = dtt1.Rows[0][0].ToString();
                                        string b = dtt1.Rows[0][1].ToString();
                                        string c = dtt1.Rows[0][2].ToString();
                                        f5.a = a;
                                        f5.b = b;
                                        f5.c = c;
                                        f5.d = s;
                                        f5.e = s1;
                                        //form nhập hàng
                                        f5.ShowDialog();
                                        MessageBox.Show("Thêm Thành Công!!");
                                        con.Close();
                                        return;
                                    }
                                    if (thongbao == DialogResult.No)
                                    {
                                        con.Close();
                                        return;
                                    }
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                s1 = dt.Rows[i][0].ToString();
                                m = s.Contains(s1);
                                //quy cách sai thì báo sản phẩm không có trong kho
                                if (m == false)
                                {
                                    DialogResult thongbao;
                                    thongbao = MessageBox.Show("Sản Phẩm Chưa Có !! Thêm Sản Phẩm", "thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (thongbao == DialogResult.Yes)
                                    {
                                        //form thêm sản phẩm
                                        f6.ShowDialog();
                                        con.Close();
                                        return;
                                    }
                                    if (thongbao == DialogResult.No)
                                    {
                                        this.Close();
                                        con.Close();
                                    }
                                }
                            }
                        }

                        // Xuất Hàng

                        DialogResult thongbao1;
                        if (toolStripStatusLabel2.Text == "Đã Chọn Xuất Hàng")
                        {
                            string str = "select quycach from manager";
                            SQLiteDataAdapter ad = new SQLiteDataAdapter(str, con);
                            DataTable dt = new DataTable();
                            ad.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                s1 = dt.Rows[i][0].ToString();
                                //Kiểm tra trùng quy cách 
                                if (m = s.Contains(s1))
                                {
                                    thongbao1 = MessageBox.Show("Xuất Hàng", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (thongbao1 == DialogResult.Yes)
                                    {
                                        //lấy số lượng theo quy cách 
                                        SQLiteDataAdapter addd = new SQLiteDataAdapter("select soluong from manager where quycach='" + s1.ToString() + "'", con);
                                        DataTable dttt = new DataTable();
                                        addd.Fill(dttt);
                                        int soluong = Convert.ToInt32(dttt.Rows[0][0].ToString());
                                        // nếu số lượng bằng 0 thì xóa dòng đó
                                        if (soluong == 0)
                                        {
                                            MessageBox.Show("Kho Đã Hết");
                                            con.Close();
                                            return;
                                        }
                                        //form xuất hàng
                                        f7.ShowDialog();
                                        //lây dữ liệu
                                        SQLiteDataAdapter add2 = new SQLiteDataAdapter("select tenmathang,nhasanxuat,gia from manager where quycach = '" + s1.ToString() + "'", con);
                                        DataTable dtd2 = new DataTable();
                                        add2.Fill(dtd2);
                                        string a1 = dtd2.Rows[0][0].ToString();
                                        string a2 = dtd2.Rows[0][1].ToString();
                                        string a3 = dtd2.Rows[0][2].ToString();
                                        //update xuất hàng trừ số lượng
                                        SQLiteCommand cm1 = new SQLiteCommand("update manager set soluong=soluong-'" + Convert.ToInt32(f7.textBox1.Text) + "',giatong = (gia * (soluong-'" + Convert.ToInt32(f7.textBox1.Text) + "')) where quycach='" + s1.ToString() + "'and (soluong-'" + Convert.ToInt32(f7.textBox1.Text) + "')>=0", con);
                                        cm1.ExecuteNonQuery();
                                        SQLiteDataAdapter addd1 = new SQLiteDataAdapter("select soluong from manager where quycach='" + s1.ToString() + "'", con);
                                        DataTable dttt1 = new DataTable();
                                        addd1.Fill(dttt1);
                                        int soluong1 = Convert.ToInt32(dttt1.Rows[0][0].ToString());
                                        //thêm vào bảng manager1 lịch sử   
                                        //số lượng trước sau khi update có thay đổi không 
                                        if (soluong == soluong1)
                                        {
                                            MessageBox.Show("Số Lượng Nhập Xuất Lớn Hơn Số Lượng Trong Kho");
                                            con.Close();
                                            return;
                                        }
                                        SQLiteCommand cm2 = new SQLiteCommand("insert into manager1(mascan,tenmathang,quycach,nhasanxuat,gia,soluong,giatong,thoigian,nhapxuat) values('" + s.ToString() + "','" + a1.ToString() + "','" + s1.ToString() + "','" + a2.ToString() + "','" + a3.ToString() + "','" + f7.textBox1.Text + "','" + (Convert.ToInt32(a3.ToString()) * Convert.ToInt32(f7.textBox1.Text)) + "','" + DateTime.Now.ToString() + "','" + xuat.ToString() + "')", con);
                                        cm2.ExecuteNonQuery();
                                        MessageBox.Show("Đã Xuất : " + textBox1.Text + "");
                                        con.Close();
                                        f7.textBox1.Clear();
                                        return;
                                    }
                                    if (thongbao1 == DialogResult.No)
                                    {
                                        this.Close();
                                        con.Close();
                                    }
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                s1 = dt.Rows[i][0].ToString();
                                //kiểm tra không có quy cách mã trong kho thì thông báo
                                m = s.Contains(s1);
                                if (m == false)
                                {
                                    MessageBox.Show("Sản Phẩm Không Có Trong Kho !!");
                                    con.Close();
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã Trống");
                        con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Vui Lòng Chọn Nhập, Xuất Hàng Hoặc Trả Hàng");
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //quét mã bằng cổng USB
        void listener_ScanerEvent(ScanerHook.ScanerCodes codes)
        {
            textBox1.Text = codes.Result;
            f6.s100 = textBox1.Text;
            string s = textBox1.Text;
            con.Open();
            try
            {
                if (toolStripStatusLabel2.Text != "")
                {
                    if (textBox1.Text != "")
                    {
                        //Nhập Hàng

                        if (toolStripStatusLabel2.Text == "Đã Chọn Nhập Hàng")
                        {
                            string str = "select quycach from manager";
                            SQLiteDataAdapter ad = new SQLiteDataAdapter(str, con);
                            DataTable dt = new DataTable();
                            ad.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                s1 = dt.Rows[i][0].ToString();
                                //kiểm tra trùng quy cách
                                if (m = s.Contains(s1))
                                {
                                    DialogResult thongbao;
                                    f5.label2.Text = s1.ToString();
                                    thongbao = MessageBox.Show("Thêm 1 Đơn vị Số Lượng Sản Phẩm", "thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (thongbao == DialogResult.Yes)
                                    {
                                        SQLiteDataAdapter ad1 = new SQLiteDataAdapter("select tenmathang,nhasanxuat,gia from manager where quycach ='"+s1+"'",con);
                                        DataTable dtt1 = new DataTable();
                                        ad1.Fill(dtt1);
                                        string a = dtt1.Rows[0][0].ToString();
                                        string b = dtt1.Rows[0][1].ToString();
                                        string c = dtt1.Rows[0][2].ToString();
                                        f5.a = a;
                                        f5.b = b;
                                        f5.c = c;
                                        f5.d = s;
                                        f5.e = s1;
                                        //form nhập hàng
                                        f5.ShowDialog();
                                        MessageBox.Show("Thêm Thành Công!!");
                                        con.Close();
                                        return;
                                    }
                                    if (thongbao == DialogResult.No)
                                    {
                                        con.Close();
                                        return;
                                    }
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                s1 = dt.Rows[i][0].ToString();
                                m = s.Contains(s1);
                                //quy cách sai thì báo sản phẩm không có trong kho
                                if (m == false)
                                {
                                    DialogResult thongbao;
                                    thongbao = MessageBox.Show("Sản Phẩm Chưa Có !! Thêm Sản Phẩm", "thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (thongbao == DialogResult.Yes)
                                    {
                                        //form thêm sản phẩm
                                        f6.ShowDialog();
                                        con.Close();
                                        return;
                                    }
                                    if (thongbao == DialogResult.No)
                                    {
                                        this.Close();
                                        con.Close();
                                    }
                                }
                            }
                        }

                        // Xuất Hàng

                        DialogResult thongbao1;
                        if (toolStripStatusLabel2.Text == "Đã Chọn Xuất Hàng")
                        {
                            string str = "select quycach from manager";
                            SQLiteDataAdapter ad = new SQLiteDataAdapter(str, con);
                            DataTable dt = new DataTable();
                            ad.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                s1 = dt.Rows[i][0].ToString();
                                //Kiểm tra trùng quy cách 
                                if (m = s.Contains(s1))
                                {
                                    thongbao1 = MessageBox.Show("Xuất Hàng", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (thongbao1 == DialogResult.Yes)
                                    {
                                        //lấy số lượng theo quy cách 
                                        SQLiteDataAdapter addd = new SQLiteDataAdapter("select soluong from manager where quycach='" + s1.ToString() + "'",con);
                                        DataTable dttt = new DataTable();
                                        addd.Fill(dttt);
                                        int soluong =Convert.ToInt32(dttt.Rows[0][0].ToString());
                                       // nếu số lượng bằng 0 thì xóa dòng đó
                                        if (soluong == 0)
                                        {
                                            MessageBox.Show("Kho Đã Hết");
                                            con.Close();
                                            return;
                                        }
                                        //form xuất hàng
                                        f7.ShowDialog();
                                        //lây dữ liệu
                                        SQLiteDataAdapter add2 = new SQLiteDataAdapter("select tenmathang,nhasanxuat,gia from manager where quycach = '"+s1.ToString()+"'",con);
                                        DataTable dtd2 = new DataTable();
                                        add2.Fill(dtd2);
                                        string a1 = dtd2.Rows[0][0].ToString();
                                        string a2 = dtd2.Rows[0][1].ToString();
                                        string a3 = dtd2.Rows[0][2].ToString();
                                        //update xuất hàng trừ số lượng
                                        SQLiteCommand cm1 = new SQLiteCommand("update manager set soluong=soluong-'" + Convert.ToInt32(f7.textBox1.Text) + "',giatong = (gia * (soluong-'" + Convert.ToInt32(f7.textBox1.Text) + "')) where quycach='" + s1.ToString() + "'and (soluong-'" + Convert.ToInt32(f7.textBox1.Text) + "')>=0", con);
                                        cm1.ExecuteNonQuery();
                                        SQLiteDataAdapter addd1 = new SQLiteDataAdapter("select soluong from manager where quycach='" + s1.ToString() + "'", con);
                                        DataTable dttt1 = new DataTable();
                                        addd1.Fill(dttt1);
                                        int soluong1 = Convert.ToInt32(dttt1.Rows[0][0].ToString());
                                        //thêm vào bảng manager1 lịch sử   
                                        //số lượng trước sau khi update có thay đổi không 
                                        if (soluong == soluong1)
                                        {
                                            MessageBox.Show("Số Lượng Nhập Xuất Lớn Hơn Số Lượng Trong Kho");
                                            con.Close();
                                            return;
                                        }
                                        SQLiteCommand cm2 = new SQLiteCommand("insert into manager1(mascan,tenmathang,quycach,nhasanxuat,gia,soluong,giatong,thoigian,nhapxuat) values('" + s.ToString() + "','" + a1.ToString() + "','" + s1.ToString() + "','" + a2.ToString() + "','" + a3.ToString() + "','" + f7.textBox1.Text + "','" + (Convert.ToInt32(a3.ToString()) * Convert.ToInt32(f7.textBox1.Text)) + "','" + DateTime.Now.ToString() + "','" + xuat.ToString() + "')", con);
                                        cm2.ExecuteNonQuery();
                                        MessageBox.Show("Đã Xuất : " + textBox1.Text + "");
                                        con.Close();
                                        f7.textBox1.Clear();
                                        return;
                                    }
                                    if (thongbao1 == DialogResult.No)
                                    {
                                        this.Close();
                                        con.Close();
                                    }
                                }
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                s1 = dt.Rows[i][0].ToString();
                                //kiểm tra không có quy cách mã trong kho thì thông báo
                                m = s.Contains(s1);
                                if (m == false)
                                {
                                    MessageBox.Show("Sản Phẩm Không Có Trong Kho !!");
                                    con.Close();
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã Trống");
                        con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Vui Lòng Chọn Nhập, Xuất Hàng Hoặc Trả Hàng");
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void khoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectToolStripMenuItem.ForeColor = Color.Black;
            nhậpHàngToolStripMenuItem.ForeColor = Color.Black;
            xuấtHàngToolStripMenuItem.ForeColor = Color.Black;
            quảnLýMượnTrảToolStripMenuItem.ForeColor = Color.Black;
            khoToolStripMenuItem.ForeColor = Color.Red;
            f2.ShowDialog();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!sp.IsOpen)
                {
                    sp.PortName = f3.comboBox1.Text;
                    sp.BaudRate = int.Parse(f3.comboBox2.Text);
                    sp.Open();
                    toolStripStatusLabel1.Text = "Kết Nối Thành Công !";
                    toolStripStatusLabel1.ForeColor = Color.Green;
                    connectToolStripMenuItem.ForeColor = Color.Green;
                     nhậpHàngToolStripMenuItem.ForeColor = Color.Black;
                     xuấtHàngToolStripMenuItem.ForeColor = Color.Black;
                     quảnLýMượnTrảToolStripMenuItem.ForeColor = Color.Black;
                     settingToolStripMenuItem.ForeColor = Color.Black;
                     khoToolStripMenuItem.ForeColor = Color.Black;
                }
                else
                {
                    sp.Close();
                }
            }
            catch
            {
                toolStripStatusLabel1.Text = "Kết Nối Thất Bại !";
                toolStripStatusLabel1.ForeColor = Color.Red;
                connectToolStripMenuItem.ForeColor = Color.Red;
                nhậpHàngToolStripMenuItem.ForeColor = Color.Black;
                xuấtHàngToolStripMenuItem.ForeColor = Color.Black;
                quảnLýMượnTrảToolStripMenuItem.ForeColor = Color.Black;
                settingToolStripMenuItem.ForeColor = Color.Black;
                khoToolStripMenuItem.ForeColor = Color.Black;
                return;
            }
        }

        private void Kho_Load(object sender, EventArgs e)
        {
            c.Start();
            string[] ListCOM = SerialPort.GetPortNames();
            int[] ListBoaud = { 2400, 4800, 9600, 19200, 115200 };
            Array.Sort(ListCOM);
            for (int i = 0; i < ListCOM.Length; i++)
            {
                f3.comboBox1.Items.Add(ListCOM[i]);
            }
            for (int i = 0; i < ListBoaud.Length; i++)
            {
                f3.comboBox2.Items.Add(ListBoaud[i]);
            }
            f3.comboBox1.Text = st.COM;
            f3.comboBox2.Text = st.Baurate;
            toolStripStatusLabel3.Text = st.COM;
            toolStripStatusLabel4.Text = "Baurate:" + st.Baurate;
        }

        private void nhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Đã Chọn Nhập Hàng";
            nhậpHàngToolStripMenuItem.ForeColor = Color.Red;
            xuấtHàngToolStripMenuItem.ForeColor = Color.Black;
            quảnLýMượnTrảToolStripMenuItem.ForeColor = Color.Black;
            connectToolStripMenuItem.ForeColor = Color.Black;
            settingToolStripMenuItem.ForeColor = Color.Black;
            khoToolStripMenuItem.ForeColor = Color.Black;
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = f3.comboBox1.Text;
            toolStripStatusLabel4.Text = "Baurate:" + f3.comboBox2.Text;
            settingToolStripMenuItem.ForeColor = Color.Red;
            nhậpHàngToolStripMenuItem.ForeColor = Color.Black;
            xuấtHàngToolStripMenuItem.ForeColor = Color.Black;
            quảnLýMượnTrảToolStripMenuItem.ForeColor = Color.Black;
            connectToolStripMenuItem.ForeColor = Color.Black;
            khoToolStripMenuItem.ForeColor = Color.Black;
            f3.ShowDialog();
        }

        private void xuấtHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Đã Chọn Xuất Hàng";
            xuấtHàngToolStripMenuItem.ForeColor = Color.Red;
            nhậpHàngToolStripMenuItem.ForeColor = Color.Black;
            quảnLýMượnTrảToolStripMenuItem.ForeColor = Color.Black;
            connectToolStripMenuItem.ForeColor = Color.Black;
            settingToolStripMenuItem.ForeColor = Color.Black;
            khoToolStripMenuItem.ForeColor = Color.Black;
        }

        private void quảnLýMượnTrảToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                xuấtHàngToolStripMenuItem.ForeColor = Color.Black;
                nhậpHàngToolStripMenuItem.ForeColor = Color.Black;
                quảnLýMượnTrảToolStripMenuItem.ForeColor = Color.Red;
                connectToolStripMenuItem.ForeColor = Color.Black;
                settingToolStripMenuItem.ForeColor = Color.Black;
                khoToolStripMenuItem.ForeColor = Color.Black;
                c.Stop();
                f4.ShowDialog();
                c.Start();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }
    }
}
