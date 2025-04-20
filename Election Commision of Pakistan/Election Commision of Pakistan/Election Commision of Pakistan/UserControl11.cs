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

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl11 : UserControl
    {
        OracleConnection con;

        public UserControl11(int i)
        {
            InitializeComponent();
            if(i == 2)
            {
                button1.Enabled = false;
            }
            if(NavigationManager.Access() == 3)
            {
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false; 
                button4.Enabled = false; 
                button5.Enabled = false;
            }
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql1 = "Begin EScheduler; Commit; end;";
                con.Open();
                OracleCommand comm = new OracleCommand(sql1, con);
                int row = comm.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("Election has been Scheduled");
                }
                con.Close();
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Oracle Execption: "+ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl19");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl14");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl15");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl12");
        }
    }
}
