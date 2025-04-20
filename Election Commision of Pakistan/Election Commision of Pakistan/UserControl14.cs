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
    public partial class UserControl14 : UserControl
    {
        OracleConnection con;
        public UserControl14()
        {
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
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

        private void button9_Click(object sender, EventArgs e)
        {
            string text = textBox7.Text;
            string sql = "Select * from Election where ElectionID = :id";
            updateGrid(sql, text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            con.Open();
            int no = 0;
            try
            {
                if (int.TryParse(textBox7.Text, out no))
                {
                    string sql = "Delete from Election where ElectionID = :id";
                    OracleCommand comm = new OracleCommand(sql, con);
                    comm.Parameters.Add("id", OracleDbType.Int32).Value = no;
                    int row = comm.ExecuteNonQuery();
                    if (row > 0)
                    {
                        MessageBox.Show("Election Cancled Succesfully");
                    }
                    else
                    {
                        MessageBox.Show("Election Cancelation is Unsuccessful");
                    }
                }
                else
                {
                    MessageBox.Show("Enter the correct ID");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Oracle exception :" +  ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateBack();
        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
            textBox7.ForeColor = Color.Black;
        }
    }
}
