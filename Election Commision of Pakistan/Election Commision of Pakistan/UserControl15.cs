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
    public partial class UserControl15 : UserControl
    {
        OracleConnection con;

        public UserControl15()
        {
            InitializeComponent();
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
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
            string sql;
            string text = textBox7.Text;
            if ((textBox1.Text == "" || textBox1.Text == "Enter the ID") && (text != "" || text != "Enter the ID"))
            {
                sql = "Select E.* from Election E inner join ElectionSchedule O on E.ElectionID = O.ElectionID inner join Constituencies C on C.constituencyID = O.ConstituencyID where C.ConstituencyID = :id";
                updateGrid(sql, text);

            }
            else if (text == "" || text == "Enter the ID")
            {
                sql = "Select E.* from Election E inner join ElectionSchedule O on E.ElectionID = O.ElectionID inner join pollingstations P on P.ConstituencyID = O.ConstituencyID where pollingstationId = :id";
                updateGrid(sql, textBox1.Text);
            }
        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
            textBox7.ForeColor = Color.Black;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateBack();
        }
    }
}
