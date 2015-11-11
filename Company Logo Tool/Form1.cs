using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Company_Logo_Tool
{
    public partial class mainForm : Form
    {
        
        SqlConnection conn = new SqlConnection("data source=RSCI-EBS;initial catalog=TEST;user id=" + Credentials.userName + ";password="+ Credentials.password);
        SqlCommand command;
        string imgLoc = "";
        
        public mainForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
