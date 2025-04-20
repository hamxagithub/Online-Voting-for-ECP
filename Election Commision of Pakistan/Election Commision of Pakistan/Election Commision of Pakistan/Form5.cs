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
    public partial class Form5 : Form
    {
        OracleConnection con;
        Form3 F;
        public Form5(Form3 F)
        {
            this.F = F;
            InitializeComponent();
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
            panel3.BackColor = Color.FromArgb(100, 0, 0, 0);
            groupBox1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
            panel3.Visible = false;
            groupBox1.Visible = false;

        }
        private void updateGrid(string sql, string text = "")
        {
            con.Open();
            OracleCommand getEmps = new OracleCommand(sql, con);
            if (text != "")
            {
                getEmps.Parameters.Add("id", OracleDbType.Int32).Value = int.Parse(text);

            }
            OracleDataReader empDR = getEmps.ExecuteReader();
            DataTable empDT = new DataTable();
            empDT.Load(empDR);
            dataGridView2.DataSource = empDT;
            con.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Enabled = true;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            string sql = "Delete from Voters where VoterID = :id";
            string text = textBox7.Text;
            con.Open();
            OracleCommand getEmps = new OracleCommand(sql, con);
            if (text != "")
            {
                getEmps.Parameters.Add("id", OracleDbType.Int32).Value = int.Parse(text);

            }
            getEmps.ExecuteNonQuery();
            con.Close();
            DialogResult result = MessageBox.Show("Voter deleted successfully!");
            if (result == DialogResult.OK)
            {
                string sql1 = "Select * from Voters where Voterid = :id";
                updateGrid(sql1, text);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Enabled = true;
            radioButton1.Enabled = false;
            radioButton3.Enabled = false;
            panel1.Visible = true;
            panel3.Visible = false;
            panel2.Visible = false;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radioButton3.Enabled = true;
            radioButton2.Enabled = false;
            radioButton1.Enabled = false;
            string sql = "Select * from Voters where Voterid = :id";
            string text = textBox7.Text;
            updateGrid(sql, text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel2.Visible = false;
            panel1.Visible = false;
            groupBox1.Visible = false;
        }

        private void panel3_VisibleChanged(object sender, EventArgs e)
        {
            if (sender is Panel panel && panel.Visible)
            {
                // Calculate new position to center the panel
                int newX = (ClientSize.Width - panel.Width) / 2;
                int newY = (ClientSize.Height - panel.Height) / 2;

                // Set the new location
                panel.Location = new Point(newX, newY);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel2.Visible = false;
            panel1.Visible = false;
            groupBox1.Visible = true;

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
        private void button7_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = false;
            panel1.Visible = false;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            string sql = "INSERT INTO Voters (Voterid, Name, Fname, CNIC, Address, PollingStationID) " +
              "VALUES (:id, :name, :Lname, :CNIC, :Address, :Pid)";

            try
            {
                using (OracleCommand getEmps = new OracleCommand(sql, con))
                {
                    // Parse textbox values to expected data types
                    int VoterId = 0, PollingId = 0;
                    if (int.TryParse(textBox13.Text, out VoterId) &&
                        int.TryParse(textBox8.Text, out PollingId))
                    {
                        con.Open();
                        getEmps.Parameters.Add("id", OracleDbType.Int32).Value = VoterId;
                        getEmps.Parameters.Add("name", OracleDbType.Varchar2).Value = textBox12.Text;
                        getEmps.Parameters.Add("Lname", OracleDbType.Varchar2).Value = textBox14.Text;
                        getEmps.Parameters.Add("CNIC", OracleDbType.Varchar2).Value = textBox1.Text;
                        getEmps.Parameters.Add("Address", OracleDbType.Varchar2).Value = textBox11.Text;
                        getEmps.Parameters.Add("Pid", OracleDbType.Int32).Value = PollingId;

                        int rowsInserted = getEmps.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Voter inserted successfully!");
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

        private void textBox7_Validating(object sender, CancelEventArgs e)
        {
            int i;
            if (int.TryParse(textBox7.Text, out i))
            {
                string sql1 = "Select * from Voters where Voterid = :id";
                string text = textBox7.Text;
                updateGrid(sql1, text);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string[] para = new string[5];
            string[] attri = new string[5];
            try
            {
                attri[0] = "Name";
                para[0] = textBox2.Text;
                attri[1] = "CNIC";
                para[1] = textBox3.Text;
                attri[2] = "Fname";
                para[2] = textBox4.Text;
                attri[3] = "Address";
                para[3] = textBox5.Text;
                attri[4] = "PollingstationID";
                para[4] = textBox6.Text;
                string text = textBox7.Text;
                for (int i = 0; i < 5; i++)
                {
                    if (para[i] != "" && !ContainsDigit(para[i]) || para[i] == "'CNIC'")
                    {
                        //para[i] = "'" + para[i] + "'";
                        string sql1 = "UPDATE Voters SET " + attri[i] + " = :name where VoterId = :id";

                        con.Open();
                        OracleCommand getEmps = new OracleCommand(sql1, con);
                        getEmps.Parameters.Add("name", OracleDbType.Varchar2).Value = para[i];
                        int no = 0;
                        if (int.TryParse(text, out no))
                        {
                            getEmps.Parameters.Add("id", OracleDbType.Int32).Value = no;
                        }
                        int row = getEmps.ExecuteNonQuery();
                        if (row > 0)
                        {
                            MessageBox.Show("Voters is Updated");
                        }
                        con.Close();
                    }
                    else if (ContainsDigit(para[i]))
                    {
                        string sql2 = "Update Voters set " + attri[i] + " = :name  where VoterId = :id;";

                        con.Open();
                        OracleCommand getEmps = new OracleCommand(sql2, con);
                        int no = 0, mo = 0;
                        if (int.TryParse(para[i], out no) && int.TryParse(text, out mo))
                        {
                            getEmps.Parameters.Add("name", OracleDbType.Int32).Value = no;
                            getEmps.Parameters.Add("id", OracleDbType.Int32).Value = mo;
                        }

                        int row = getEmps.ExecuteNonQuery();
                        if(row > 0) 
                        {
                            MessageBox.Show("Voters is Updated");
                        }
                        con.Close();
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle Exception: " + ex.Message);
            }
            string sql = "Select * from Voters where Voterid = :id";
            string text1 = textBox7.Text;
            updateGrid(sql, text1);

        }

        

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = false;
            panel3.Visible = false;
            groupBox1.Visible = true;
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            F.Show();
        }
    }

}
