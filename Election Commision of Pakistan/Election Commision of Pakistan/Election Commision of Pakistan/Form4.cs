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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Election_Commision_of_Pakistan
{
    public partial class Form4 : Form
    {
        OracleConnection con;
        Form3 F;
        public Form4(Form3 F)
        {
            this.F = F;
            InitializeComponent();
            groupBox1.Visible = true; 
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con= new OracleConnection(conStr);
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
            panel3.BackColor = Color.FromArgb(100, 0, 0, 0);
            groupBox1.BackColor = Color.FromArgb(100, 0, 0, 0);
            con.Open();
            string sql = "Select ElectionType from Election";
            OracleCommand cmd = new OracleCommand(sql, con);
            OracleDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("ElectionType"));
                comboBox1.Items.Add(name);
                comboBox2.Items.Add(name);
            }
            con.Close();
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0; // Select first time slot by default
            }
            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }
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
            string sql = "Delete from Candidates where candidateid = :id";
            string text = textBox7.Text;
            con.Open();
            OracleCommand getEmps = new OracleCommand(sql, con);
            if (text != "")
            {
                getEmps.Parameters.Add("id", OracleDbType.Int32).Value = int.Parse(text);

            }
            getEmps.ExecuteNonQuery();
            con.Close();
            DialogResult result = MessageBox.Show("Candidate deleted successfully!");
            if(result == DialogResult.OK)
            {
                string sql1 = "Select * from Candidates where candidateid = :id";
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
            string sql = "Select * from Candidates where candidateid = :id";
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
            string sql = "INSERT INTO Candidates (Candidateid, Name, PartyID, ElectionType, ConstituencyID) VALUES (:id, :name, :pid, :Ename, :Cid)";
            try
            {


                using (OracleCommand getEmps = new OracleCommand(sql, con))
                {
                    // Parse textbox values to expected data types
                    int candidateId = 0, partyId = 0, constituencyId = 0;
                    if (int.TryParse(textBox13.Text, out candidateId) &&
                        int.TryParse(textBox11.Text, out partyId) &&
                        int.TryParse(textBox8.Text, out constituencyId))
                    {
                        con.Open();
                        getEmps.Parameters.Add("id", OracleDbType.Int32).Value = candidateId;
                        getEmps.Parameters.Add("name", OracleDbType.Varchar2).Value = textBox12.Text;
                        getEmps.Parameters.Add("pid", OracleDbType.Int32).Value = partyId;
                        getEmps.Parameters.Add("Ename", OracleDbType.Varchar2).Value = textBox9.Text;
                        getEmps.Parameters.Add("Cid", OracleDbType.Int32).Value = constituencyId;

                        int rowsInserted = getEmps.ExecuteNonQuery();
                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Candidate inserted successfully!");
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
                string sql1 = "Select * from Candidates where candidateid = :id";
                string text = textBox7.Text;
                updateGrid(sql1, text);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string[] para = new string[4];
            string[] attri = new string[4];
            attri[0] = "Name";
            para[0] = textBox2.Text;
            attri[1] = "PartyID";
            para[1] = textBox3.Text;
            attri[2] = "ElectionType";
            para[2] = textBox5.Text;
            attri[3] = "ConstituencyID";
            para[3] = textBox6.Text;
            string text = textBox7.Text;
            for (int i = 0; i < 4; i++)
            {
                if (para[i] != "" && !ContainsDigit(para[i]))
                {
                    //para[i] = "'" + para[i] + "'";
                    string sql1 = "UPDATE CANDIDATES SET " + attri[i] + " = :name where CandidateId = :id";

                    con.Open();
                    OracleCommand getEmps = new OracleCommand(sql1, con);
                    getEmps.Parameters.Add("name", OracleDbType.Varchar2).Value = para[i];
                    int no = 0;
                    if (int.TryParse(text, out no))
                    {
                        getEmps.Parameters.Add("id", OracleDbType.Int32).Value = no;
                    }
                    getEmps.ExecuteNonQuery();
                    con.Close();
                }
                else if (ContainsDigit(para[i]))
                {
                    string sql2 = "Update Candidates set " + attri[i] + " = :name  where CandidateId = :id;";

                    con.Open();
                    OracleCommand getEmps = new OracleCommand(sql2, con);
                    int no = 0, mo = 0;
                    if (int.TryParse(para[i], out no) && int.TryParse(text, out mo))
                    {
                        getEmps.Parameters.Add("name", OracleDbType.Int32).Value = no;
                        getEmps.Parameters.Add("id", OracleDbType.Int32).Value = mo;
                    }

                    getEmps.ExecuteNonQuery();
                    con.Close();
                }
            }
            MessageBox.Show("Candidate Updated");
            string sql = "Select * from Candidates where candidateid = :id";
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

        private void button11_Click(object sender, EventArgs e)
        {
            
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            F.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem?.ToString();
            textBox9.Text = selected;

        }
    }
}
