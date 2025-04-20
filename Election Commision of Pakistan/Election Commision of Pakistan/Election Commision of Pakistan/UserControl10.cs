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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl10 : UserControl
    {
        OracleConnection con;
        public UserControl10()
        {
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
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
            string sql = "INSERT INTO PollingStations(PollingStationid, ConstituencyID, PollingStationName, Location, Presiding, Asistpresiding, PollingOfficer) " +
              "VALUES (:id, :Cid, :Pname, :location, :Presiding, :APO, :PO)";

            try
            {
                using (OracleCommand getEmps = new OracleCommand(sql, con))
                {
                    // Parse textbox values to expected data types
                    int PSId, Cid,PD, APO,PO;
                    if (int.TryParse(textBox2.Text, out PSId) &&
                        int.TryParse(textBox8.Text, out Cid) &&
                        int.TryParse(textBox5.Text, out PD) &&
                        int.TryParse(textBox3.Text, out APO) &&
                        int.TryParse(textBox1.Text, out PO))
                    {
                        con.Open();
                        getEmps.Parameters.Add("id", OracleDbType.Int32).Value = PSId;
                        getEmps.Parameters.Add("Cid", OracleDbType.Int32).Value = Cid;
                        getEmps.Parameters.Add("Pname", OracleDbType.Varchar2).Value = textBox9.Text;
                        getEmps.Parameters.Add("location", OracleDbType.Varchar2).Value = textBox7.Text;
                        getEmps.Parameters.Add("Presiding", OracleDbType.Int32).Value = PD;
                        getEmps.Parameters.Add("APO", OracleDbType.Int32).Value = APO;
                        getEmps.Parameters.Add("PO", OracleDbType.Int32).Value = PO;
                        int rowsInserted = getEmps.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Polling Station inserted successfully!");
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

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.ForeColor = Color.Black;
        }

        private void textBox9_Click(object sender, EventArgs e)
        {
            textBox9.Text = "";
            textBox9.ForeColor = Color.Black;
        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
            textBox7.ForeColor = Color.Black;
        }

        private void textBox8_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
            textBox8.ForeColor = Color.Black;
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox5.ForeColor = Color.Black;
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox3.ForeColor = Color.Black;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }
    }

}
