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

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl4 : UserControl
    {
        Form3 form;
        int check;
        string[] list = new string[4];
        public UserControl4(int check)
        {
            this.form = form;
            InitializeComponent();
            list[0] = "UserControl3";
            list[1] = "UserControl6";
            list[2] = "UserControl7";
            list[3] = "UserControl10";
            this.check = check;
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
        }
        
        
        private void button2_Click(object sender, EventArgs e)
        {
            
            NavigationManager.NavigateTo("UserControl1");
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateTo(list[check - 3]);
            
        }

    }
}
