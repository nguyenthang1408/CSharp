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
using System.IO.Ports;
using System.Threading;

namespace Kho
{
    public partial class Quản_Lý_Mượn_Trả : Form
    {
        SerialPort sp = new SerialPort();
        Settings1 st = new Settings1();
        Setting f3 = new Setting();
        ScanerHook c = new ScanerHook();
        string s, s1, s2, s3, s4;
        bool m;
        int so;
        SQLiteConnection con = new SQLiteConnection("Data Source=" + System.Windows.Forms.Application.StartupPath + "\\Data.db;pooling=true;FailIFMising=false");
        public Quản_Lý_Mượn_Trả()
        {
            InitializeComponent();
            c.ScanerEvent += listener_ScanerEvent;
            CheckForIllegalCrossThreadCalls = false;
        }
        //quét mã bằng COM
        void seri(object sender, SerialDataReceivedEventArgs e)
        {
            textBox1.Text = sp.ReadExisting();
            s = textBox1.Text;
            try
            {
                if (radioButton1.Checked == true)
                {
                    string str = "select quycach from manager";
                    SQLiteDataAdapter ad = new SQLiteDataAdapter(str, con);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s1 = dt.Rows[i][0].ToString();
                        if (s.Contains(s1))
                        {
                            //lấy tên mặt hàng và nhà sản xuất theo mã quy cách
                            SQLiteDataAdapter ad1 = new SQLiteDataAdapter("select tenmathang,nhasanxuat from manager  where quycach='" + s1.ToString() + "'", con);
                            DataTable dt1 = new DataTable();
                            ad1.Fill(dt1);
                            s2 = dt1.Rows[0][0].ToString();
                            s3 = dt1.Rows[0][1].ToString();
                            con.Open();
                            //lấy quy cách theo mã thẻ
                            string str1222 = "select quycach from muontra where mathe = '" + textBox3.Text + "'";
                            SQLiteDataAdapter ad1222 = new SQLiteDataAdapter(str1222, con);
                            DataTable dt1222 = new DataTable();
                            ad1222.Fill(dt1222);
                            for (int c = 0; c < dt1222.Rows.Count; c++)
                            {
                                s4 = dt1222.Rows[c][0].ToString();
                                if (s1 == s4)
                                {
                                    // thêm vào bảng lưu lịch sử mượn trả
                                    SQLiteCommand cm22 = new SQLiteCommand("insert into muontra1(tenmathang,nhasanxuat,quycach,soluong,mathe,nguoimuon,ngaymuon) values('" + s2.ToString() + "','" + s3.ToString() + "','" + s1.ToString() + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + DateTime.Now.ToString() + "')", con);
                                    cm22.ExecuteNonQuery();
                                    // sửa số Lượng Thêm vào
                                    SQLiteCommand cmmm = new SQLiteCommand("update muontra set soluong =soluong+'" + Convert.ToInt32(textBox4.Text) + "'where quycach='" + s4.ToString() + "'and mathe='" + textBox3.Text + "'", con);
                                    cmmm.ExecuteNonQuery();
                                    toolStripStatusLabel1.Text = "Đã Thêm Số Lượng " + textBox4.Text + " " + s2.ToString() + "";
                                    toolStripStatusLabel1.ForeColor = Color.Green;
                                    con.Close();
                                    //thêm xong xóa đã nhập
                                    textBox1.Clear();
                                    textBox2.Clear();
                                    textBox3.Clear();
                                    textBox4.Clear();
                                    radioButton1.Checked = false;
                                    radioButton3.Checked = false;
                                    radioButton1.Enabled = false;
                                    radioButton3.Enabled = false;
                                    return;
                                }
                            }
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                s1 = dt.Rows[j][0].ToString();
                                if (s.Contains(s1))
                                {
                                    //thêm hàng đã mượn
                                    SQLiteCommand cm = new SQLiteCommand("insert into muontra(tenmathang,nhasanxuat,quycach,soluong,mathe,nguoimuon,ngaymuon) values('" + s2.ToString() + "','" + s3.ToString() + "','" + s1.ToString() + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + DateTime.Now.ToString() + "')", con);
                                    cm.ExecuteNonQuery();
                                    //thêm vào bảng lưu lịch sử mượn hàng
                                    SQLiteCommand cmm22 = new SQLiteCommand("insert into muontra1(tenmathang,nhasanxuat,quycach,soluong,mathe,nguoimuon,ngaymuon) values('" + s2.ToString() + "','" + s3.ToString() + "','" + s1.ToString() + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + DateTime.Now.ToString() + "')", con);
                                    cmm22.ExecuteNonQuery();
                                    toolStripStatusLabel1.Text = "Đã Mượn Hàng Thành Công " + s2.ToString() + "";
                                    toolStripStatusLabel1.ForeColor = Color.Green;
                                    con.Close();
                                    //ghi xong xóa
                                    textBox1.Clear();
                                    textBox2.Clear();
                                    textBox3.Clear();
                                    textBox4.Clear();
                                    radioButton1.Checked = false;
                                    radioButton3.Checked = false;
                                    radioButton1.Enabled = false;
                                    radioButton3.Enabled = false;
                                    return;
                                }
                            }
                        }

                    }
                    //kiểm tra quy cách nhập vào trùng với tất cả quy cách trong kho hay không
                    MessageBox.Show("Sản Phẩm Không Có Trong Kho!");
                    toolStripStatusLabel1.Text = "Sản Phẩm Không Có Trong Kho!";
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    con.Close();
                    //
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    radioButton1.Checked = false;
                    radioButton3.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton3.Enabled = false;
                    return;

                }
                if (radioButton3.Checked == true)
                {
                    //lấy tất cả quy cách trong bảng
                    string str = "select quycach from muontra";
                    SQLiteDataAdapter ad = new SQLiteDataAdapter(str, con);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    DialogResult thongbao11;
                    //kiểm tra trùng mã quy cách
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s1 = dt.Rows[i][0].ToString();
                        if (m = s.Contains(s1))
                        {
                            string s100 = s1;
                            thongbao11 = MessageBox.Show("Trả Hàng", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (thongbao11 == DialogResult.Yes)
                            {
                                //lấy tên mặt hàng, nhà sản xuất theo quy cách
                                SQLiteDataAdapter ad1 = new SQLiteDataAdapter("select tenmathang,nhasanxuat from manager  where quycach='" + s100.ToString() + "'", con);
                                DataTable dt1 = new DataTable();
                                ad1.Fill(dt1);
                                s2 = dt1.Rows[0][0].ToString();
                                s3 = dt1.Rows[0][1].ToString();
                                //lấy mã thẻ theo quy cách
                                SQLiteDataAdapter add1 = new SQLiteDataAdapter("select mathe from muontra where quycach = '" + s100.ToString() + "'", con);
                                DataTable dttt1 = new DataTable();
                                add1.Fill(dttt1);
                                //kiểm tra trùng mã quy cách và mã thẻ
                                for (int b = 0; b < dttt1.Rows.Count; b++)
                                {
                                    string s10 = dttt1.Rows[b][0].ToString();
                                    if (s10 == textBox3.Text)
                                    {
                                        //lấy số lượng theo quy cách trước khi update
                                        SQLiteDataAdapter a1 = new SQLiteDataAdapter("select soluong from muontra where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'", con);
                                        DataTable d = new DataTable();
                                        a1.Fill(d);
                                        string s6 = d.Rows[0][0].ToString();
                                        con.Open();
                                        // update ngày trả và số lượng theo quy cách và mã thẻ
                                        SQLiteCommand cm1 = new SQLiteCommand("update muontra set ngaytra='" + DateTime.Now.ToString() + "',soluong=soluong-'" + textBox4.Text + "' where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'and (soluong-'" + textBox4.Text + "')>=0", con);
                                        cm1.ExecuteNonQuery();
                                        toolStripStatusLabel1.Text = "Đã Trả Kho " + textBox4.Text + "";
                                        toolStripStatusLabel1.ForeColor = Color.Green;
                                        // thêm vào bảng lịch sử mượn trả
                                        SQLiteCommand cm22 = new SQLiteCommand("insert into muontra1(tenmathang,nhasanxuat,quycach,soluong,mathe,nguoimuon,ngaytra) values('" + s2.ToString() + "','" + s3.ToString() + "','" + s1.ToString() + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + DateTime.Now.ToString() + "')", con);
                                        cm22.ExecuteNonQuery();
                                        //lấy số lượng sau khi đã upate xong
                                        SQLiteDataAdapter a11 = new SQLiteDataAdapter("select soluong from muontra where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'", con);
                                        DataTable d1 = new DataTable();
                                        a11.Fill(d1);
                                        string s7 = d1.Rows[0][0].ToString();
                                        //kiểm tra trước và sau khi update có thay đổi không
                                        if (s6 == s7)
                                        {
                                            toolStripStatusLabel1.Text = "Số Lượng Nhập Vào Lớn Hơn Đã Mượn";
                                            toolStripStatusLabel1.ForeColor = Color.Red;
                                            MessageBox.Show("Số Lượng Nhập Vào Lớn Hơn Số Lượng Đã Mượn!");
                                            con.Close();
                                            textBox1.Clear();
                                            textBox2.Clear();
                                            textBox3.Clear();
                                            textBox4.Clear();
                                            radioButton1.Checked = false;
                                            radioButton3.Checked = false;
                                            radioButton1.Enabled = false;
                                            radioButton3.Enabled = false;
                                            return;
                                        }
                                        if (s7 == "0")
                                        {
                                            SQLiteDataAdapter ad2 = new SQLiteDataAdapter("select tenmathang from muontra where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'", con);
                                            DataTable dtt2 = new DataTable();
                                            ad2.Fill(dtt2);
                                            string tenmathang = dtt2.Rows[0][0].ToString();
                                            //kiểm tra nếu số lượng bằng 0 thì xóa cột đó
                                            SQLiteCommand cm = new SQLiteCommand("delete from muontra where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'", con);
                                            cm.ExecuteNonQuery();
                                            MessageBox.Show("Đã Trả Hết " + tenmathang + "");
                                        }
                                        toolStripStatusLabel1.Text = "Đã Trả Kho " + s2.ToString() + "";
                                        toolStripStatusLabel1.ForeColor = Color.Green;
                                        con.Close();
                                        // reset dữ liệu nhập
                                        textBox1.Clear();
                                        textBox2.Clear();
                                        textBox3.Clear();
                                        textBox4.Clear();
                                        radioButton1.Checked = false;
                                        radioButton3.Checked = false;
                                        radioButton1.Enabled = false;
                                        radioButton3.Enabled = false;
                                        return;
                                    }
                                }
                                // không trùng mã thẻ đã mượn thẻ thông báo
                                MessageBox.Show("Mã Thẻ Sai Hoặc Không Mượn Hàng");
                                toolStripStatusLabel1.Text = "Mã Thẻ Sai Hoặc Không Mượn Hàng";
                                toolStripStatusLabel1.ForeColor = Color.Red;
                                con.Close();
                                //reset
                                textBox1.Clear();
                                textBox2.Clear();
                                textBox3.Clear();
                                textBox4.Clear();
                                radioButton1.Checked = false;
                                radioButton3.Checked = false;
                                radioButton1.Enabled = false;
                                radioButton3.Enabled = false;
                                return;
                            }

                            if (thongbao11 == DialogResult.No)
                            {
                                con.Close();
                                //reset
                                textBox1.Clear();
                                textBox2.Clear();
                                textBox3.Clear();
                                textBox4.Clear();
                                radioButton1.Checked = false;
                                radioButton3.Checked = false;
                                radioButton1.Enabled = false;
                                radioButton3.Enabled = false;
                                return;
                            }
                        }
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //kiểm tra không trùng quy cách có sản phẩm 
                        s1 = dt.Rows[i][0].ToString();
                        m = s.Contains(s1);
                        if (m == false)
                        {
                            MessageBox.Show("Sản Phẩm Không Có Trong Kho Hàng Mượn !!");
                            toolStripStatusLabel1.Text = "Sản Phẩm Không Có Trong Kho Hàng Mượn !!";
                            toolStripStatusLabel1.ForeColor = Color.Red;
                            con.Close();
                            //
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                            radioButton1.Checked = false;
                            radioButton3.Checked = false;
                            radioButton1.Enabled = false;
                            radioButton3.Enabled = false;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }
        //quét mã bằng cổng USB
        void listener_ScanerEvent(ScanerHook.ScanerCodes codes)
        {
                textBox1.Text = codes.Result;
                s = textBox1.Text;
                try
                {
                    if (radioButton1.Checked == true)
                    {
                        string str = "select quycach from manager";
                        SQLiteDataAdapter ad = new SQLiteDataAdapter(str, con);
                        DataTable dt = new DataTable();
                        ad.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            s1 = dt.Rows[i][0].ToString();
                            if (s.Contains(s1))
                            {
                                //lấy tên mặt hàng và nhà sản xuất theo mã quy cách
                                SQLiteDataAdapter ad1 = new SQLiteDataAdapter("select tenmathang,nhasanxuat from manager  where quycach='" + s1.ToString() + "'", con);
                                DataTable dt1 = new DataTable();
                                ad1.Fill(dt1);
                                s2 = dt1.Rows[0][0].ToString();
                                s3 = dt1.Rows[0][1].ToString();
                                con.Open();
                                // lấy mã thẻ

                                //lấy quy cách theo mã thẻ
                                string str1222 = "select quycach from muontra where mathe = '" + textBox3.Text + "'";
                                SQLiteDataAdapter ad1222 = new SQLiteDataAdapter(str1222, con);
                                DataTable dt1222 = new DataTable();
                                ad1222.Fill(dt1222);
                                for (int c = 0; c < dt1222.Rows.Count; c++)
                                {
                                    s4 = dt1222.Rows[c][0].ToString();
                                    if (s1 == s4)
                                    {
                                        // thêm vào bảng lưu lịch sử mượn trả
                                        SQLiteCommand cm22 = new SQLiteCommand("insert into muontra1(tenmathang,nhasanxuat,quycach,soluong,mathe,nguoimuon,ngaymuon) values('" + s2.ToString() + "','" + s3.ToString() + "','" + s1.ToString() + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + DateTime.Now.ToString() + "')", con);
                                        cm22.ExecuteNonQuery();
                                        // sửa số Lượng Thêm vào
                                        SQLiteCommand cmmm = new SQLiteCommand("update muontra set soluong =soluong+'" + Convert.ToInt32(textBox4.Text) + "'where quycach='" + s4.ToString() + "'and mathe='" + textBox3.Text + "'", con);
                                        cmmm.ExecuteNonQuery();
                                        toolStripStatusLabel1.Text = "Đã Thêm Số Lượng " + textBox4.Text + " " + s2.ToString() + "";
                                        toolStripStatusLabel1.ForeColor = Color.Green;
                                        con.Close();
                                        //thêm xong xóa đã nhập
                                        textBox1.Clear();
                                        textBox2.Clear();
                                        textBox3.Clear();
                                        textBox4.Clear();
                                        radioButton1.Checked = false;
                                        radioButton3.Checked = false;
                                        radioButton1.Enabled = false;
                                        radioButton3.Enabled = false;
                                        return;
                                    }
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    s1 = dt.Rows[j][0].ToString();
                                    if (s.Contains(s1))
                                    {
                                        //thêm hàng đã mượn
                                        SQLiteCommand cm = new SQLiteCommand("insert into muontra(tenmathang,nhasanxuat,quycach,soluong,mathe,nguoimuon,ngaymuon) values('" + s2.ToString() + "','" + s3.ToString() + "','" + s1.ToString() + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + DateTime.Now.ToString() + "')", con);
                                        cm.ExecuteNonQuery();
                                        //thêm vào bảng lưu lịch sử mượn hàng
                                        SQLiteCommand cmm22 = new SQLiteCommand("insert into muontra1(tenmathang,nhasanxuat,quycach,soluong,mathe,nguoimuon,ngaymuon) values('" + s2.ToString() + "','" + s3.ToString() + "','" + s1.ToString() + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + DateTime.Now.ToString() + "')", con);
                                        cmm22.ExecuteNonQuery();
                                        toolStripStatusLabel1.Text = "Đã Mượn Hàng Thành Công " + s2.ToString() + "";
                                        toolStripStatusLabel1.ForeColor = Color.Green;
                                        con.Close();
                                        //ghi xong xóa
                                        textBox1.Clear();
                                        textBox2.Clear();
                                        textBox3.Clear();
                                        textBox4.Clear();
                                        radioButton1.Checked = false;
                                        radioButton3.Checked = false;
                                        radioButton1.Enabled = false;
                                        radioButton3.Enabled = false;
                                        return;
                                    }
                                }
                            }

                        }
                        //kiểm tra quy cách nhập vào trùng với tất cả quy cách trong kho hay không
                        MessageBox.Show("Sản Phẩm Không Có Trong Kho!");
                        toolStripStatusLabel1.Text = "Sản Phẩm Không Có Trong Kho!";
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        con.Close();
                        //
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        radioButton1.Checked = false;
                        radioButton3.Checked = false;
                        radioButton1.Enabled = false;
                        radioButton3.Enabled = false;
                        return;

                    }
                    if (radioButton3.Checked == true)
                    {
                        //lấy tất cả quy cách trong bảng
                        string str = "select quycach from muontra";
                        SQLiteDataAdapter ad = new SQLiteDataAdapter(str, con);
                        DataTable dt = new DataTable();
                        ad.Fill(dt);
                        DialogResult thongbao11;
                        //kiểm tra trùng mã quy cách
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            s1 = dt.Rows[i][0].ToString();
                            if (m = s.Contains(s1))
                            {
                                string s100 = s1;
                                thongbao11 = MessageBox.Show("Trả Hàng", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (thongbao11 == DialogResult.Yes)
                                {
                                    //lấy tên mặt hàng, nhà sản xuất theo quy cách
                                    SQLiteDataAdapter ad1 = new SQLiteDataAdapter("select tenmathang,nhasanxuat from manager  where quycach='" + s100.ToString() + "'", con);
                                    DataTable dt1 = new DataTable();
                                    ad1.Fill(dt1);
                                    s2 = dt1.Rows[0][0].ToString();
                                    s3 = dt1.Rows[0][1].ToString();
                                    //lấy mã thẻ theo quy cách
                                    SQLiteDataAdapter add1 = new SQLiteDataAdapter("select mathe from muontra where quycach = '" + s100.ToString() + "'", con);
                                    DataTable dttt1 = new DataTable();
                                    add1.Fill(dttt1);
                                    //kiểm tra trùng mã quy cách và mã thẻ
                                    for (int b = 0; b < dttt1.Rows.Count; b++)
                                    {
                                        string s10 = dttt1.Rows[b][0].ToString();
                                        if (s10 == textBox3.Text)
                                        {
                                            //lấy số lượng theo quy cách trước khi update
                                            SQLiteDataAdapter a1 = new SQLiteDataAdapter("select soluong from muontra where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'", con);
                                            DataTable d = new DataTable();
                                            a1.Fill(d);
                                            string s6 = d.Rows[0][0].ToString();
                                            con.Open();
                                            // update ngày trả và số lượng theo quy cách và mã thẻ
                                            SQLiteCommand cm1 = new SQLiteCommand("update muontra set ngaytra='" + DateTime.Now.ToString() + "',soluong=soluong-'" + textBox4.Text + "' where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'and (soluong-'" + textBox4.Text + "')>=0", con);
                                            cm1.ExecuteNonQuery();
                                            toolStripStatusLabel1.Text = "Đã Trả Kho " + textBox4.Text + "";
                                            toolStripStatusLabel1.ForeColor = Color.Green;
                                            // thêm vào bảng lịch sử mượn trả
                                            SQLiteCommand cm22 = new SQLiteCommand("insert into muontra1(tenmathang,nhasanxuat,quycach,soluong,mathe,nguoimuon,ngaytra) values('" + s2.ToString() + "','" + s3.ToString() + "','" + s1.ToString() + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + DateTime.Now.ToString() + "')", con);
                                            cm22.ExecuteNonQuery();
                                            //lấy số lượng sau khi đã upate xong
                                            SQLiteDataAdapter a11 = new SQLiteDataAdapter("select soluong from muontra where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'", con);
                                            DataTable d1 = new DataTable();
                                            a11.Fill(d1);
                                            string s7 = d1.Rows[0][0].ToString();
                                            //kiểm tra trước và sau khi update có thay đổi không
                                            if (s6 == s7)
                                            {
                                                toolStripStatusLabel1.Text = "Số Lượng Nhập Vào Lớn Hơn Đã Mượn";
                                                toolStripStatusLabel1.ForeColor = Color.Red;
                                                MessageBox.Show("Số Lượng Nhập Vào Lớn Hơn Số Lượng Đã Mượn!");
                                                con.Close();
                                                textBox1.Clear();
                                                textBox2.Clear();
                                                textBox3.Clear();
                                                textBox4.Clear();
                                                radioButton1.Checked = false;
                                                radioButton3.Checked = false;
                                                radioButton1.Enabled = false;
                                                radioButton3.Enabled = false;
                                                return;
                                            }
                                            if (s7 == "0")
                                            {
                                                SQLiteDataAdapter ad2 = new SQLiteDataAdapter("select tenmathang from muontra where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'", con);
                                                DataTable dtt2 = new DataTable();
                                                ad2.Fill(dtt2);
                                                string tenmathang = dtt2.Rows[0][0].ToString();
                                                //kiểm tra nếu số lượng bằng 0 thì xóa cột đó
                                                SQLiteCommand cm = new SQLiteCommand("delete from muontra where quycach='" + s1.ToString() + "'and mathe ='" + textBox3.Text + "'", con);
                                                cm.ExecuteNonQuery();
                                                MessageBox.Show("Đã Trả Hết " + tenmathang + "");
                                            }
                                            toolStripStatusLabel1.Text = "Đã Trả Kho " + s2.ToString() + "";
                                            toolStripStatusLabel1.ForeColor = Color.Green;
                                            con.Close();
                                            // reset dữ liệu nhập
                                            textBox1.Clear();
                                            textBox2.Clear();
                                            textBox3.Clear();
                                            textBox4.Clear();
                                            radioButton1.Checked = false;
                                            radioButton3.Checked = false;
                                            radioButton1.Enabled = false;
                                            radioButton3.Enabled = false;
                                            return;
                                        }
                                    }
                                    // không trùng mã thẻ đã mượn thẻ thông báo
                                    MessageBox.Show("Mã Thẻ Sai Hoặc Không Mượn Hàng");
                                    toolStripStatusLabel1.Text = "Mã Thẻ Sai Hoặc Không Mượn Hàng";
                                    toolStripStatusLabel1.ForeColor = Color.Red;
                                    con.Close();
                                    //reset
                                    textBox1.Clear();
                                    textBox2.Clear();
                                    textBox3.Clear();
                                    textBox4.Clear();
                                    radioButton1.Checked = false;
                                    radioButton3.Checked = false;
                                    radioButton1.Enabled = false;
                                    radioButton3.Enabled = false;
                                    return;
                                }

                                if (thongbao11 == DialogResult.No)
                                {
                                    con.Close();
                                    //reset
                                    textBox1.Clear();
                                    textBox2.Clear();
                                    textBox3.Clear();
                                    textBox4.Clear();
                                    radioButton1.Checked = false;
                                    radioButton3.Checked = false;
                                    radioButton1.Enabled = false;
                                    radioButton3.Enabled = false;
                                    return;
                                }
                            }
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //kiểm tra không trùng quy cách có sản phẩm 
                            s1 = dt.Rows[i][0].ToString();
                            m = s.Contains(s1);
                            if (m == false)
                            {
                                MessageBox.Show("Sản Phẩm Không Có Trong Kho Hàng Mượn !!");
                                toolStripStatusLabel1.Text = "Sản Phẩm Không Có Trong Kho Hàng Mượn !!";
                                toolStripStatusLabel1.ForeColor = Color.Red;
                                con.Close();
                                //
                                textBox1.Clear();
                                textBox2.Clear();
                                textBox3.Clear();
                                textBox4.Clear();
                                radioButton1.Checked = false;
                                radioButton3.Checked = false;
                                radioButton1.Enabled = false;
                                radioButton3.Enabled = false;
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    con.Close();
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox2.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Vui lòng Điền Đủ Thông Tin");
                return;
            }
            string s = textBox2.Text;
            s = s.Replace(" ", ""); // thay thế và xóa ký tự trống
            for (int i = 0; i < s.Length; i++)
            {
                s2 = s[i].ToString();
                try
                {
                    //kiểm tra xem có ký tự số trong chuỗi tên nhập vào hay không
                    int c = Convert.ToInt32(s2);
                    radioButton1.Enabled = false;
                    radioButton3.Enabled = false;
                    MessageBox.Show("Tên Không Chứa Ký Tự số");
                    return;
                }
                catch (Exception)
                {

                }
            }

            try
            {
                if (Convert.ToInt32(textBox4.Text) > 0)
                {
                    //kiểm tra số lượng có phải là số hay không
                    so = Convert.ToInt32(textBox4.Text);
                }
                else
                {
                    MessageBox.Show("Số Lượng Phải Lớn Hơn 0");
                    return;
                }
               
            }
            catch
            {
                MessageBox.Show("Số Lượng Phải Là Số");
                textBox4.Text = "";
                radioButton1.Enabled = false;
                radioButton3.Enabled = false;
            }
            if (textBox3.Text != "" && textBox2.Text != "" && textBox4.Text != "")
            {
                radioButton1.Enabled = true;
                radioButton3.Enabled = true;
            }
        }

        private void Quản_Lý_Mượn_Trả_Load(object sender, EventArgs e)
        {
            radioButton1.Enabled = false;
            radioButton3.Enabled = false;
            c.Start();
            try
            {
                if (!sp.IsOpen)
                {
                    sp.PortName = f3.comboBox1.Text;
                    sp.BaudRate = int.Parse(f3.comboBox2.Text);
                    sp.Open();
                    toolStripStatusLabel2.Text = "Kết Nối COM Thành Công !";
                    toolStripStatusLabel2.ForeColor = Color.Green;
                }
                else
                {
                    sp.Close();
                }
            }
            catch
            {
                toolStripStatusLabel2.Text = "Chưa Kết Nối COM !";
                toolStripStatusLabel2.ForeColor = Color.Red;
                return;
            }
        }

        private void Quản_Lý_Mượn_Trả_FormClosed(object sender, FormClosedEventArgs e)
        {
            c.Stop();
        }
    }
}
