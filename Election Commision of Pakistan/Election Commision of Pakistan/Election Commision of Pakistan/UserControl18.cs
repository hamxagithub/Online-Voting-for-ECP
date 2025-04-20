using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl18 : UserControl
    {
        OracleConnection con;
        string text;
        public UserControl18(string text)
        {
            this.text = text;
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            con.Open();
            string sql = "BEGIN Eresult(" + text + "); End;";
            OracleCommand getEmps = new OracleCommand(sql, con);
            int row = getEmps.ExecuteNonQuery();
            con.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "Select * from Form45 where pollingstationid = :id";
            updateGrid(sql, text);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateBack();
        }
    }
}
