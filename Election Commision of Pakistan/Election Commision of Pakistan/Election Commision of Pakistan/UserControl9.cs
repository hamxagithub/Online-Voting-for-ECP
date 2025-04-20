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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl9 : UserControl
    {
        string text;
        OracleConnection con;
        public UserControl9(string text)
        {
            InitializeComponent();
            this.text = text;
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
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
            string[] para = new string[6];
            string[] attri = new string[6];
            try
            {
                attri[0] = "PollingStationName";
                para[0] = textBox9.Text;
                attri[1] = "Location";
                para[1] = textBox7.Text;
                attri[2] = "ConstituencyID";
                para[2] = textBox8.Text;
                attri[3] = "Presiding";
                para[3] = textBox5.Text;
                attri[4] = "AsistPresiding";
                para[4] = textBox3.Text;
                attri[5] = "PollingOfficer";
                para[5] = textBox1.Text;

                for (int i = 0; i < 6; i++)
                {
                    if (para[i] != "" && !ContainsDigit(para[i]))
                    {
                        //para[i] = "'" + para[i] + "'";
                        string sql1 = "UPDATE PollingStations SET " + attri[i] + " = :name where PollingStationId = :id";

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
                            MessageBox.Show("Polling Station is Updated");
                        }
                        con.Close();
                    }
                    else if (ContainsDigit(para[i]))
                    {
                        string sql2 = "Update PollingStations set " + attri[i] + " = :name  where PollingStationId = :id;";

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
                            MessageBox.Show("Polling Station is Updated");
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
