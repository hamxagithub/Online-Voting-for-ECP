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
using static System.Net.Mime.MediaTypeNames;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl20 : UserControl
    {
        OracleConnection con;
        string text;
        public UserControl20(string text)
        {
            this.text = text;   
            InitializeComponent();
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            con.Open();
            string sql = "BEGIN Eresult1(" + text + "); End;";
            OracleCommand getEmps = new OracleCommand(sql, con);
            int row = getEmps.ExecuteNonQuery();
            con.Close();
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
        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "Select * from Form47 where constituencyid = :id";
            updateGrid(sql, text);
        }
    }
}
