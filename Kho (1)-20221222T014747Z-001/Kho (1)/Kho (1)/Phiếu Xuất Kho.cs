using System;
using Aspose.Words;//Thêm thư viện này (file Dll\Aspose.Word.dll)
using Aspose.Words.Tables;
using System.Windows.Forms;
using ThuVienWinform.Report.AsposeWordExtension;
using System.Drawing;
using System.Threading;
using System.IO;


namespace Kho
{
    public partial class Phiếu_Xuất_Kho : Form
    {
        int a1 = 0;
        string a = "Bắc Giang";
        int hangHienTai = 1;
        int dem = 1;
        int tong = 0;
        string tenTep;
        Document baoCao;
        public Phiếu_Xuất_Kho()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                label9.Text = "Loading...";
                label9.ForeColor = Color.Green;
                var homNay = DateTime.Now;
                //Bước 1: Nạp file mẫu
                string filename = "Baocao.doc";
                string dt = DateTime.Now.ToShortTimeString();
                string tenTep = @"C:\Users\Administrator\Desktop" + "\\" + filename;
                Document baoCao = new Document(tenTep);
                //Bước 2: Điền các thông tin cố định
                baoCao.MailMerge.Execute(new[] { "Ngay_Thang_Nam_BC" }, new[] { string.Format("" + a.ToString() + ", ngày {0} tháng {1} năm {2}", homNay.Day, homNay.Month, homNay.Year) });
                baoCao.MailMerge.Execute(new[] { "Ho_Ten" }, new[] { textBox1.Text });
                baoCao.MailMerge.Execute(new[] { "Bo_Phan" }, new[] { textBox3.Text });
                baoCao.MailMerge.Execute(new[] { "Ly_Do" }, new[] { textBox4.Text });
                baoCao.MailMerge.Execute(new[] { "Xuat" }, new[] { textBox5.Text });
                baoCao.MailMerge.Execute(new[] { "Thoi_Gian" }, new[] { dateTimePicker1.Value.ToShortDateString()});

                //Bước 3: Điền thông tin lên bảng

                //baoCao.MailMerge.Execute(new[] { "Tong_Tien" }, new[] { ((Convert.ToInt32(textBox8.Text)) * (Convert.ToInt32(textBox7.Text))).ToString() });
                //Bước 4: Lưu và mở file
                baoCao.Save(@"C:\Users\Administrator\Desktop" + "\\" + filename);
                label9.Text = "Xuất File Đã Xong";
                label9.ForeColor = Color.Green;
                int date = DateTime.Now.Minute + DateTime.Now.Second;
                File.Move(@"C:\Users\Administrator\Desktop" + "\\" + filename, @"C:\Users\Administrator\Desktop" + "\\" + date + ".doc");
                baoCao.SaveAndOpenFile(date + ".doc");
                // sử dụng hàm tạo của lớp FileStream
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Điền đầy đủ thông tin!");
                return;
            }
            try
            {
                int a3 = Convert.ToInt32(textBox7.Text);
                int a4 = Convert.ToInt32(textBox8.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Giá Và Số Lượng phải là Số");
                return;
            }
            Thread listen1 = new Thread(() =>
            {
                label9.Text = "Loading...";
                label9.ForeColor = Color.Green;
                dem++;

                if (dem == 2)
                {
                    baoCao = new Document("Template\\Mau_Bao_Cao1.doc");
                }
                else
                {
                    tenTep = @"C:\Users\Administrator\Desktop" + "\\" + "BaoCao.doc";
                    baoCao = new Document(tenTep);
                }
                Table bangThongTin = baoCao.GetChild(NodeType.Table, 0, true) as Table;//Lấy bảng thứ 2 trong file mẫu
                try
                {
                    bangThongTin.PutValue(hangHienTai, 0, (dem - 1).ToString());
                    bangThongTin.PutValue(hangHienTai, 1, textBox2.Text);
                    bangThongTin.PutValue(hangHienTai, 2, textBox8.Text);
                    bangThongTin.PutValue(hangHienTai, 3, textBox7.Text);
                    bangThongTin.PutValue(hangHienTai, 4, ((Convert.ToInt32(textBox8.Text)) * (Convert.ToInt32(textBox7.Text))).ToString());
                    a1 = ((Convert.ToInt32(textBox8.Text)) * (Convert.ToInt32(textBox7.Text)));
                    tong = a1 + tong;
                    if (hangHienTai == 6)
                    {
                        MessageBox.Show("hết ô trong bảng thông tin");
                        return;
                    }
                    //bangThongTin.InsertRows(1, hangHienTai+1, dem);
                    if (hangHienTai >= 2)
                    {
                        //bangThongTin.InsertRows(1, hangHienTai + 1, dem);
                        bangThongTin.PutValue(hangHienTai+1, 4, "Tổng Tiền: "+tong.ToString());
                        tenTep = @"C:\Users\Administrator\Desktop" + "\\" + "BaoCao.doc";
                        // Lưu Tệp
                        baoCao.Save(tenTep);
                        //Process.Start(tenTep);
                        textBox2.Clear();
                        textBox8.Clear();
                        textBox7.Clear();
                        label9.Text = "Lưu Đã Xong";
                        label9.ForeColor = Color.Green;
                        hangHienTai++;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                tenTep = @"C:\Users\Administrator\Desktop" + "\\" + "BaoCao.doc";
                // Lưu Tệp
                baoCao.Save(tenTep);
                //Process.Start(tenTep);
                textBox2.Clear();
                textBox8.Clear();
                textBox7.Clear();
                label9.Text = "Lưu Đã Xong";
                label9.ForeColor = Color.Green;
                hangHienTai++;
                textBox1.Enabled = true;
                textBox5.Enabled = true;
                dateTimePicker1.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                button1.Show();
            }); listen1.Start();
        }

        private void Phiếu_Xuất_Kho_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox5.Enabled = false;
            dateTimePicker1.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button1.Hide();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
