﻿using System;
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
        String imagePath = "";
        String imageName = "";
        byte[] image = null;
        
        public mainForm()
        {
            InitializeComponent();
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

                    imageName = Path.GetFileName(imagePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SqlCommand commandA;
            SqlCommand commandB;
            SqlCommand commandC;
            SqlCommand commandD;

            Int32 recordId = -1;
            
            string insertImageSQL = "INSERT INTO rsci_Logos(imageName, image) " +
                                    "OUTPUT INSERTED.ID "            +
                                    "VALUES (@imageName, @image)";

            string insertCustSQL = "INSERT INTO rsci_CompanyLogos(kcustnum, logo_id)" +
                                   "VALUES (@kcustnum, @logoId)";

            string deleteExistingCompanyLogosRecordsSQL = "DELETE FROM rsci_CompanyLogos " +
                                                       "WHERE kcustnum = @kcustnum";

            string getExistingImagesSQL = "SELECT id, image " +
                                           "FROM rsci_Logos " +
                                           "WHERE imageName = @imageName";

            try
            {
                
               // FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
               // BinaryReader br = new BinaryReader(fs);
                SqlDataReader reader;

               // image = br.ReadBytes((int)fs.Length);
                
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                commandD = new SqlCommand(getExistingImagesSQL, conn);
                commandD.Parameters.Add(new SqlParameter("@imageName", imageName));
                reader = commandD.ExecuteReader();
                //reader.Read();

                if (reader.HasRows)
                {
                    while(reader.Read())
                        if (reader[1].ToString().Equals(image.ToString()))
                        {
                            recordId = Int32.Parse(reader[0].ToString());
                        }
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);

                    image = br.ReadBytes((int)fs.Length);

                    commandA = new SqlCommand(insertImageSQL, conn);
                    commandA.Parameters.Add(new SqlParameter("@image", image));
                    commandA.Parameters.Add(new SqlParameter("@imageName", imageName));
                    recordId = (Int32)commandA.ExecuteScalar();
                }

                commandC = new SqlCommand(deleteExistingCompanyLogosRecordsSQL, conn);
                commandC.Parameters.Add(new SqlParameter("@kcustnum", textBoxCustomerNumber.Text));
                commandC.ExecuteNonQuery();
                
                commandB = new SqlCommand(insertCustSQL, conn);
                commandB.Parameters.Add(new SqlParameter("@kcustnum", textBoxCustomerNumber.Text));
                commandB.Parameters.Add(new SqlParameter("@logoId", recordId));
                commandB.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show("Record Saved");

                textBoxCustomerNumber.Text = null;
                textBoxCustomerName.Text = null;
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

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            

            SqlCommand command;
            SqlDataReader reader;

            try
            {
                
                string selectSQL = "SELECT custname, imageName, image " +
                                   "FROM custmast t1 " +
                                   "LEFT JOIN rsci_CompanyLogos t2 ON t2.kcustnum = t1.kcustnum " +
                                   "LEFT JOIN rsci_Logos t3 ON t3.id = t2.logo_id " +
                                   "WHERE t1.kcustnum = '" + textBoxCustomerNumber.Text + "' ";

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                command = new SqlCommand(selectSQL, conn);
                reader = command.ExecuteReader();
                reader.Read();

                if (reader.HasRows)
                {
                    textBoxCustomerName.Text = reader[0].ToString();

                    try
                    {
                        imageName = reader[1].ToString();
                        image = (byte[])(reader[2]);
                    }
                    catch(InvalidCastException ex)
                    {
                        if(typeof(DBNull) == reader[1].GetType())
                        {
                            image = null;
                        }
                        else
                        {
                            if (conn.State != ConnectionState.Closed)
                            {
                                conn.Close();
                            }

                            MessageBox.Show(ex.Message);
                        }
                    }
                    
                    if (image == null)
                    {
                        pictureBoxLogo.Image = null;
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream(image);
                        pictureBoxLogo.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    textBoxCustomerName.Text = "";
                    pictureBoxLogo.Image = null;
                    MessageBox.Show("No Record Found for this Customer Number");
                }

                conn.Close();
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
