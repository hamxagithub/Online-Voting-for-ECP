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
    public partial class Form8 : Form
    {
        Home H = new Home();
        public Form8(Home H)
        {
            this.H = H;
            InitializeComponent();
            NavigationManager.RegisterControl(this.panel1);
            NavigationManager.RegisterUserControl("UserControl17", () => new UserControl17());
            NavigationManager.NavigateTo("UserControl17");

        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            H.Show();
        }

        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }
    }
}
