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
        
        SqlConnection conn = new SqlConnection("data source="+ Credentials.dataSource + ";initial catalog=" + Credentials.database + ";user id=" + Credentials.userName + ";password="+ Credentials.password);
        SqlCommand command;
        string imagePath = "";
        
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

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Select Image";

                // Only Allow JPG and PNG file types
                dlg.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dlg.FileName.ToString();
                    pictureBoxLogo.ImageLocation = imagePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
