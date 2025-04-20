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
    public partial class UserControl17 : UserControl
    {
        OracleConnection con;
        public UserControl17()
        {

            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            NavigationManager.RegisterUserControl("UserControl18", () => new UserControl18(textBox1.Text));
            NavigationManager.RegisterUserControl("UserControl20", () => new UserControl20(textBox2.Text));
            NavigationManager.RegisterUserControl("UserControl21", () => new UserControl21());
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.BackColor = Color.Gray;
            panel2.Visible = true;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button2.BackColor = Color.Gray;
            panel3.Visible = true;

        }
        private void button4_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl18");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl20");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo("UserControl21");
        }
    }
}
