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
    public partial class UserControl8 : UserControl
    {
        OracleConnection con;
        string text;
        public UserControl8(string text)
        {
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            this.text = text;
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
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
            string[] para = new string[3];
            string[] attri = new string[3];
            try
            {
                attri[0] = "PartyName";
                para[0] = textBox2.Text;
                attri[1] = "Leader";
                para[1] = textBox4.Text;
                attri[2] = "Headquarters";
                para[2] = textBox6.Text;
                

                for (int i = 0; i < 3; i++)
                {
                    if (para[i] != "" && !ContainsDigit(para[i]))
                    {
                        //para[i] = "'" + para[i] + "'";
                        string sql1 = "UPDATE PoliticalParties SET " + attri[i] + " = :name where PartyId = :id";

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
                            MessageBox.Show("Political Party is Updated");
                        }
                        con.Close();
                    }
                    else if (ContainsDigit(para[i]))
                    {
                        MessageBox.Show("Wrong Input. Input contain number");
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
