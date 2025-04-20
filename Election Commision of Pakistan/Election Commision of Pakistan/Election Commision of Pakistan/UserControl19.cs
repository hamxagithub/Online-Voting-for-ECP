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
    public partial class UserControl19 : UserControl
    {
        OracleConnection con;
        public UserControl19()
        {
            InitializeComponent();
            NavigationManager.RegisterUserControl("UserControl13", () => new UserControl13(textBox7.Text));
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
        private void button5_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl13");
        }

        private void textBox7_Validating(object sender, CancelEventArgs e)
        {
            int no;
            if (int.TryParse(textBox7.Text, out no))
            {
                string sql = "Select * from Election where ElectionID = :id";
                string text = textBox7.Text;
                updateGrid(sql, text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateBack();
        }
    }
}
