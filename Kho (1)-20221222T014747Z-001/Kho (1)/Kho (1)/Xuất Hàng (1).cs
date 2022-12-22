using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kho
{
    public partial class Xuất_Hàng : Form
    {
        public Xuất_Hàng()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int a = Convert.ToInt32(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Số Lượng Phải là số");
            }
            this.Close();
        }

        private void Xuất_Hàng_Load(object sender, EventArgs e)
        {

        }
    }
}
