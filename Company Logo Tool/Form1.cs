using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] image = null;
                FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                image = br.ReadBytes((int)fs.Length);
                string insertSQL = "INSERT INTO rsci_Logos(image) "+
                                   "VALUES (@image)";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                command = new SqlCommand(insertSQL, conn);
                command.Parameters.Add(new SqlParameter("@image", image));
                command.ExecuteReader();
                conn.Close();
                MessageBox.Show("Record Saved");
                pictureBoxLogo.Image = null;
            }
            catch (Exception ex)
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }

                MessageBox.Show(ex.Message);
            }
        }

    }
}
