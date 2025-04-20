using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Election_Commision_of_Pakistan
{
    public partial class Form7 : Form
    {
        int i;
        Home H = new Home();
        public Form7(int i, Home H)
        {
            this.H = H;
            InitializeComponent();
            this.i = i;
            NavigationManager.RegisterControl(this.panel1);
            NavigationManager.RegisterUserControl("UserControl11", () => new UserControl11(i));
            NavigationManager.RegisterUserControl("UserControl12", () => new UserControl12());
            
            NavigationManager.RegisterUserControl("UserControl14", () => new UserControl14());
            NavigationManager.RegisterUserControl("UserControl15", () => new UserControl15());
            NavigationManager.RegisterUserControl("UserControl19", () => new UserControl19());
            if (i == 3)
            {
                NavigationManager.NavigateTo("UserControl11", i);
            }
            else
            {
                NavigationManager.NavigateTo("UserControl11");
            }
        }

        private void Form7_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (i == 2)
            {
                Form3 form = new Form3(i, H);
                form.ShowDialog();
            }
            else
            {
                H.Show();
            }
        }

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }
    }
}
