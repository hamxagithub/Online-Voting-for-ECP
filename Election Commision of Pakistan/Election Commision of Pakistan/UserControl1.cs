
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
using static Election_Commision_of_Pakistan.UserControl4;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl1 : UserControl
    {
        OracleConnection con;
        int check;
        string[] list = new string[4];
        string[] list1 = new string[4];
        string[] list2 = new string[4];
        public UserControl1(int check)
        {
            this.check = check;
            list[0] = "ElectionOfficials";
            list1[0] = "OfficialId";
            list[1] = "Constituencies";
            list1[1] = "ConstituencyID";
            list[2] = "PoliticalParties";
            list1[2] = "PartyId";
            list[3] = "PollingStations";
            list1[3] = "PollingStationID";
            list2[0] = "UserControl2";
            list2[1] = "UserControl5";
            list2[2] = "UserControl8";
            list2[3] = "UserControl9";
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
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
            dataGridView1.DataSource = empDT;
            con.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Enabled = true;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            string sql = "Delete from "+ list[check-3] + " where " + list1[check - 3] + " = :id";
            string text = textBox7.Text;
            con.Open();
            OracleCommand getEmps = new OracleCommand(sql, con);
            if (text != "")
            {
                getEmps.Parameters.Add("id", OracleDbType.Int32).Value = int.Parse(text);

            }
            getEmps.ExecuteNonQuery();
            con.Close();
            DialogResult result = MessageBox.Show(list[check - 3] + " deleted successfully!");
            if (result == DialogResult.OK)
            {
                string sql1 = "Select * from " + list[check - 3] + " where " + list1[check - 3] +" = :id";
                updateGrid(sql1, text);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Enabled = true;
            radioButton1.Enabled = false;
            radioButton3.Enabled = false;
            string text = textBox7.Text;
            NavigationManager.RegisterUserControl("UserControl2", () => new UserControl2(text));
            NavigationManager.RegisterUserControl("UserControl5", () => new UserControl5(text));
            NavigationManager.RegisterUserControl("UserControl8", () => new UserControl8(text));
            NavigationManager.RegisterUserControl("UserControl9", () => new UserControl9(text));
            NavigationManager.NavigateTo(list2[check-3]);
           
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radioButton3.Enabled = true;
            radioButton2.Enabled = false;
            radioButton1.Enabled = false;
            string sql = "Select * from " + list[check - 3] + " where " + list1[check - 3] + " = :id";
            string text = textBox7.Text;
            updateGrid(sql, text);
        }
        private void textBox7_Validating(object sender, CancelEventArgs e)
        {
            int i;
            if (int.TryParse(textBox7.Text, out i))
            {
                string sql1 = "Select * from " + list[check - 3] + " where " + list1[check - 3] + " = :id";
                string text = textBox7.Text;
                updateGrid(sql1, text);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateBack();
        }
    }
}
