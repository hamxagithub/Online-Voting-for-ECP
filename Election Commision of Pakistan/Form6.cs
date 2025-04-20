using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Election_Commision_of_Pakistan
{
    
    public partial class Form6 : Form
    {
        Form3 F;
        public Form6(int check, Form3 F)
        {
            this.F = F;
            InitializeComponent();
            NavigationManager.RegisterControl(this.panel1);
            NavigationManager.RegisterUserControl("UserControl1", () => new UserControl1(check));
            NavigationManager.RegisterUserControl("UserControl3", () => new UserControl3());
            NavigationManager.RegisterUserControl("UserControl4", () => new UserControl4(check));
            NavigationManager.RegisterUserControl("UserControl6", () => new UserControl6());
            NavigationManager.RegisterUserControl("UserControl7", () => new UserControl7()); 
            NavigationManager.RegisterUserControl("UserControl10", () => new UserControl10());
            NavigationManager.NavigateTo("UserControl4");

        }
        public Panel Access()
        {
            return panel1;
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        {
            F.Show();
        }
    }
    public static class NavigationManager
    {
        private static Dictionary<string, Func<UserControl>> _userControlFactories = new Dictionary<string, Func<UserControl>>();
        private static Stack<UserControl> _navigationStack = new Stack<UserControl>();
        private static Control control1;
        private static int I;
        // Method to register a user control factory with a unique key
        public static void RegisterUserControl(string key, Func<UserControl> userControlFactory)
        {
            if (!_userControlFactories.ContainsKey(key))
            {
                _userControlFactories.Add(key, userControlFactory);
            }
        }
        public static void RegisterControl(Control control)
        {
            control1 = control;
        }
        // Method to navigate to a registered user control by key

        public static void NavigateTo(string key, int i =0)
        {
            I = i;
            if (_userControlFactories.ContainsKey(key))
            {
                UserControl userControl = _userControlFactories[key]();
                ShowUserControl(userControl, control1);
                _navigationStack.Push(userControl); // Push the user control onto the navigation stack
            }
            else
            {
                throw new ArgumentException($"User control with key '{key}' is not registered.");
            }
        }
        public static int Access()
        {
            return I;
        }
        // Method to navigate back to the previous user control
        public static void NavigateBack()
        {
            if (_navigationStack.Count > 1)
            {
                _navigationStack.Pop(); // Remove the current user control from the stack
                UserControl previousControl = _navigationStack.Peek();
                ShowUserControl(previousControl, control1);
            }
        }

        // Helper method to display a user control in a container
        private static void ShowUserControl(UserControl userControl, Control container)
        {
            container.Controls.Clear();
            container.Controls.Add(userControl);
            userControl.Dock = DockStyle.Fill;
        }
    }
}
