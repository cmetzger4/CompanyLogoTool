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
        SqlCommand commandA;
        SqlCommand commandB;
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
            string custNum = "test";
            string insertImageSQL = "INSERT INTO rsci_Logos(image) " +
                                    "OUTPUT INSERTED.ID "            +
                                    "VALUES (@image)";

            string insertCustSQL = "INSERT INTO rsci_CompanyLogos(kcustnum, logo_id)" +
                                   "VALUES (@kcustnum, @logoId)";

            try
            {
                byte[] image = null;
                FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                image = br.ReadBytes((int)fs.Length);
                
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                commandA = new SqlCommand(insertImageSQL, conn);
                commandA.Parameters.Add(new SqlParameter("@image", image));
                Int32 recordId = (Int32)commandA.ExecuteScalar();

                commandB = new SqlCommand(insertCustSQL, conn);
                commandB.Parameters.Add(new SqlParameter("@kcustnum", custNum));
                commandB.Parameters.Add(new SqlParameter("@logoId", recordId));
                commandB.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Record Saved: " + recordId.ToString());
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
