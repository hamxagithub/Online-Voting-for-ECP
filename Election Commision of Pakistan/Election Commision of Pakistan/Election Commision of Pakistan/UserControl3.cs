using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl3 : UserControl
    {
        OracleConnection con;
        public UserControl3()
        {
            InitializeComponent();
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
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
            string sql = "INSERT INTO ElectionOfficials (Officialid, Name, Designation, ContactNumber) " +
              "VALUES (:id, :name, :Designation, :Contact)";

            try
            {
                using (OracleCommand getEmps = new OracleCommand(sql, con))
                {
                    // Parse textbox values to expected data types
                    int OfficialId;
                    if (int.TryParse(textBox13.Text, out OfficialId))
                    {
                        con.Open();
                        getEmps.Parameters.Add("id", OracleDbType.Int32).Value = OfficialId;
                        getEmps.Parameters.Add("name", OracleDbType.Varchar2).Value = textBox12.Text;
                        getEmps.Parameters.Add("Designation", OracleDbType.Varchar2).Value = textBox11.Text;
                        getEmps.Parameters.Add("Contact", OracleDbType.Varchar2).Value = textBox8.Text;
                        int rowsInserted = getEmps.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Official inserted successfully!");
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
    }
}
