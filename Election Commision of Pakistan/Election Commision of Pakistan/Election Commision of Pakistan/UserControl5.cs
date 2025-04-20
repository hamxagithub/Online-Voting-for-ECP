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
    public partial class UserControl5 : UserControl
    {
        string text;
        OracleConnection con;
        public UserControl5(string text)
        {
            InitializeComponent();
            this.text = text;
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
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
            NavigationManager.NavigateBack();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string[] para = new string[4];
            string[] attri = new string[4];
            try
            {
                attri[0] = "ConstituencyName";
                para[0] = textBox2.Text;
                attri[1] = "Province";
                para[1] = textBox4.Text;
                attri[2] = "District";
                para[2] = textBox6.Text;
                attri[3] = "ReturningOfficer";
                para[3] = textBox1.Text;

                for (int i = 0; i < 4; i++)
                {
                    if (para[i] != "" && !ContainsDigit(para[i]))
                    {
                        //para[i] = "'" + para[i] + "'";
                        string sql1 = "UPDATE Constituencies SET " + attri[i] + " = :name where ConstituencyId = :id";

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
                            MessageBox.Show("Constituency is Updated");
                        }
                        con.Close();
                    }
                    else if (ContainsDigit(para[i]))
                    {
                        string sql2 = "Update Constituencies set " + attri[i] + " = :name  where ConstituencyId = :id;";

                        con.Open();
                        OracleCommand getEmps = new OracleCommand(sql2, con);
                        int no = 0, mo = 0;
                        if (int.TryParse(para[i], out no) && int.TryParse(text, out mo))
                        {
                            getEmps.Parameters.Add("name", OracleDbType.Int32).Value = no;
                            getEmps.Parameters.Add("id", OracleDbType.Int32).Value = mo;
                        }

                        int row = getEmps.ExecuteNonQuery();
                        if (row > 0)
                        {
                            MessageBox.Show("Constituency is Updated");
                        }
                        con.Close();
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle Exception: " + ex.Message);
            }

        }
    }
}
