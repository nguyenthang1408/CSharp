using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\kepkep.db;pooling=true;FailIfMissing=false");
        Setting st = new Setting();

        public Form2()
        {
            InitializeComponent();
        }
        void ConnectSQL()
        {
            con.Open();
            SQLiteDataAdapter adt = new SQLiteDataAdapter("select * from data", con);
            DataTable dt = new DataTable();
            dataGridView1.DataSource = dt;
            adt.Fill(dt);
            con.Close();
        }

        private void btn_show_Click(object sender, EventArgs e)
        {
            ConnectSQL();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want delete all?"
                                , "Delate Data"
                                , MessageBoxButtons.OKCancel
                                , MessageBoxIcon.Question
                                , MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Cancel)
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("delete from data", con);
                cmd.ExecuteNonQuery();
                con.Close();
                ConnectSQL();
            }
            else
                return;
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            int dataCount = dataGridView1.Rows.Count;
            if (dataCount > 1)
            {
                SaveFileDialog Myfile = new SaveFileDialog();
                Myfile.Filter = "ALL(*.CSV)|*CSV";
                Myfile.FileName = "Scan_" + DateTime.Now.ToString("yyyyMMdd");

                if (Myfile.ShowDialog() == DialogResult.OK)
                {
                    string PathStr = Myfile.FileName + ".CSV";
                    if (File.Exists(Myfile.FileName + ".CSV"))
                    {
                        MessageBox.Show("Ten file da ton tai!");
                        return;
                    }
                    if (!File.Exists(Myfile.FileName + ".CSV"))
                    {
                        StreamWriter CSVWriter = new StreamWriter(PathStr, true, Encoding.Default);
                        CSVWriter.WriteLine("Serial,StationName,EmpNo,Results,DateTime");
                        CSVWriter.Flush();
                        CSVWriter.Close();
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        StreamWriter CSVWriter = new StreamWriter(PathStr, true, Encoding.Default);
                        CSVWriter.WriteLine(dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() + "," + dataGridView1.Rows[i].Cells[1].Value.ToString().Trim() + "," + dataGridView1.Rows[i].Cells[2].Value.ToString().Trim() + "," + dataGridView1.Rows[i].Cells[3].Value.ToString().Trim() + "," + dataGridView1.Rows[i].Cells[4].Value.ToString().Trim());
                        CSVWriter.Flush();
                        CSVWriter.Close();
                    }
                    MessageBox.Show("Export Successfully!");
                }
            }
            else MessageBox.Show("No Data Found");
        }

        private void btn_showbydate_Click(object sender, EventArgs e)
        {
            con.Open();
            SQLiteDataAdapter cmd = new SQLiteDataAdapter("select * from data where DateTime like '%" + dateTimePicker.Text + "%'", con);
            DataTable dt = new DataTable();
            cmd.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
    }
}
