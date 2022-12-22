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
    public partial class Setting : Form
    {
        Settings1 st = new Settings1();
        public Setting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            st.COM = comboBox1.Text;
            st.Baurate = comboBox2.Text;
            st.Save();
            this.Close();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            comboBox1.Text = st.COM;
            comboBox2.Text = st.Baurate;
        }
    }
}
