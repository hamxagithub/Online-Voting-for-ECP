using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl7 : UserControl
    {
        OracleConnection con;
        MyList<Image> image = new MyList<Image>();
        MyList<int> ID = new MyList<int>(); 
        int count = 0;
        int count1 = 0;
        int no= 0;
        public UserControl7()
        {
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            con.Open();
            string sql = "Select image, imageid from Symbols where imageid not in(Select symbol from politicalparties)";
            OracleCommand emp = new OracleCommand(sql, con);
            OracleDataReader reader = emp.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("imageid"));
                OracleBlob oracleBlob = reader.GetOracleBlob(reader.GetOrdinal("image"));
                byte[] imageData = new byte[oracleBlob.Length];
                oracleBlob.Read(imageData, 0, (int)oracleBlob.Length);

                // Convert byte array to Image
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image I = Image.FromStream(ms);

                    // Display image in PictureBox or perform further processing
                    image.Add(I);
                    ID.Add(id);
                    count++;
                }

            }
            con.Close();
            if (image.Number(0) > 0)
            {
                Image temp = image.Ahead();
                pictureBox1.Image = temp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                no = ID.Ahead();
                count--;
                count1++;
            }
            if (count == 0)
            {
                button2.Enabled = false;
                button2.BackColor = Color.Gray;
            }
            if (image.Number(0) == 0)
            {
                button3.Enabled = false;
                button3.BackColor = Color.Gray;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl4");
        }
        public bool ContainsDigit(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            string sql = "INSERT INTO PoliticalParties(Partyid, PartyName, Leader,Headquarters, Symbol) " +
              "VALUES (:id, :Pname, :leader, :HQ, :symbol)";

            try
            {
                using (OracleCommand getEmps = new OracleCommand(sql, con))
                {
                    // Parse textbox values to expected data types
                    int ID;
                    if (int.TryParse(textBox13.Text, out ID))
                    {
                        con.Open();
                        getEmps.Parameters.Add("id", OracleDbType.Int32).Value = ID;
                        getEmps.Parameters.Add("Pname", OracleDbType.Varchar2).Value = textBox12.Text;
                        getEmps.Parameters.Add("leader", OracleDbType.Varchar2).Value = textBox11.Text;
                        getEmps.Parameters.Add("HQ", OracleDbType.Varchar2).Value = textBox8.Text;
                        getEmps.Parameters.Add("Symbol", OracleDbType.Int32).Value =no;
                        int rowsInserted = getEmps.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Political Party inserted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No rows were inserted.");
                        }
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid input for ID, Party ID, or Constituency ID.");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle Exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (image.Number(0) > 0)
            { 
                Image temp = image.Ahead();
                pictureBox1.Image = temp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                no = ID.Ahead();
                count--;
                count1++;

            }
            if (count == 0)
            {
                button2.Enabled = false;
                button2.BackColor = Color.Gray;
            }
            if (count1 > 0)
            {
                button3.Enabled = true;
                button3.BackColor = Color.White;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (image.Number(1) > 0)
            {
                
                Image temp = image.Back();
                pictureBox1.Image = temp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                no = ID.Back();
                count1--;
                count++;
            }
            if (count1 == 0)
            {
                button3.Enabled = false;
                button3.BackColor = Color.Gray;
            }
            if (count > 0)
            {
                button2.Enabled = true;
                button2.BackColor = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button8.Enabled = true;
        }
    }
}
