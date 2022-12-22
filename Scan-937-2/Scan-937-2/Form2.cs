using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scan_937_2
{
    public partial class Form2 : Form
    {
        Settings1 st = new Settings1();
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            st.com = comboBox1.Text;
            st.baudrate = comboBox2.Text;
            st.Save();
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Text = st.com;
            comboBox2.Text = st.baudrate;
        }
    }
}
