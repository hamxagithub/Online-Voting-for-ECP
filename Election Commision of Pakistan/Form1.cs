using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Election_Commision_of_Pakistan
{
    public partial class Form1 : Form
    {
        OracleConnection con;
        Home H = new Home();
        //private void updateGrid()
        //{
        //    con.Open();
        //    OracleCommand getEmps = con.CreateCommand();
        //    getEmps.CommandText = "SELECT * FROM Form45";
        //    getEmps.CommandType = CommandType.Text;
        //    OracleDataReader empDR = getEmps.ExecuteReader();
        //    DataTable empDT = new DataTable();
        //    empDT.Load(empDR);
        //    dataGridView1.DataSource = empDT;
        //    con.Close();
        //}
        public Form1(Home H)
        {
            this.H = H;
            InitializeComponent();
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            string source = "C:\\Users\\ADMIN\\source\\repos\\Election Commision of Pakistan\\Election Commision of Pakistan\\1904674-accept-approved-check-checked-confirm-done-tick_122524.ico";
            string source1 = "C:\\Users\\ADMIN\\source\\repos\\Election Commision of Pakistan\\Election Commision of Pakistan\\exclamationmarkinsideacircle_121970.ico";
            string source2 = "C:\\Users\\ADMIN\\source\\repos\\Election Commision of Pakistan\\Election Commision of Pakistan\\cross_wrong_close_delete_icon_191608.ico";
            IconResizer resizer = new IconResizer();
            errorProvider1.Icon = resizer.Resize(source, 18);
            errorProvider2.Icon = resizer.Resize(source1, 18);
            errorProvider3.Icon = resizer.Resize(source2, 18);
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
        }
        private void textBox1Validating(object sender, System.ComponentModel.CancelEventArgs e)
        { 
            try
            {
                int number = int.Parse(textBox1.Text);
                // Use 'number' here, which is the converted integer
                errorProvider1.SetError(textBox1, "Correct");
                errorProvider2.SetError(textBox1, "");
                errorProvider3.SetError(textBox1, "");
            }
            catch (FormatException)
            {
                errorProvider1.SetError(textBox1, "");
                errorProvider2.SetError(textBox1, "Please write the correct ID");
                errorProvider3.SetError(textBox1, "");
            }
            if (textBox1.Text == "")
            {
                errorProvider1.SetError(textBox1, "");
                errorProvider2.SetError(textBox1, "");
                errorProvider3.SetError(textBox1, "Please Enter Name");
            }
        }
        public class IconResizer
        {
            public Icon Resize(string sourceFilePath, int targetSize)
            {
                try
                {
                    // Load the original icon from the source file
                    using (Icon originalIcon = new Icon(sourceFilePath))
                    {
                        // Create a new bitmap with the desired size
                        using (Bitmap resizedBitmap = new Bitmap(targetSize, targetSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                        {
                            using (Graphics g = Graphics.FromImage(resizedBitmap))
                            {
                                // Clear the bitmap with a transparent background
                                g.Clear(Color.Transparent);
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
       
                                // Draw the original icon onto the resized bitmap
                                g.DrawIcon(originalIcon, new Rectangle(0, 0, targetSize, targetSize));
                            }
                            IntPtr hicon = resizedBitmap.GetHicon();

                            // Create an Icon object from the icon handle
                            Icon icon = Icon.FromHandle(hicon);
                          
                            return icon;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error resizing and saving icon: " + ex.Message);
                    return null ;
                }
            }


        }
        private void button3_Click(object sender, EventArgs e)
        {
            con.Open();
            OracleCommand SelectVoter = con.CreateCommand();
            SelectVoter.CommandText = "Select * from Voters where VoterID = " + textBox1.Text.ToString();
            SelectVoter.CommandType = CommandType.Text;
            OracleDataReader reader = SelectVoter.ExecuteReader();
            int ID = 0;
            int VID = Convert.ToInt32(textBox1.Text);
            if (reader.Read())
            {
                ID = Convert.ToInt32(reader["PollingStationID"]);
                this.Hide();
                if (radioButton1.Checked == true)
                {
                    Form2 form7 = new Form2(this, VID, ID, true);
                    form7.ShowDialog();
                    panel1.Visible = true;
                    panel2.Visible = false;
                }
                else if(radioButton2.Checked==true)
                {
                    Form2 form2 = new Form2(this, VID, ID, false);
                    form2.ShowDialog();
                    panel1.Visible = true;
                    panel2.Visible = false;
                }
                else
                {
                    MessageBox.Show("Select the option", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            con.Close();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            H.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }
    }
    public static class ControlHelper
    {
        public static void SetFontForAllControls(Control.ControlCollection controls, Font font)
        {
            foreach (Control control in controls)
            {
              
                control.Font = font;
                if(control is Panel || control is GroupBox)
                {
                    control.BackColor = Color.FromArgb(100, 0, 0, 0);
                }
                if(control is Label|| control is RadioButton)
                {
                    control.BackColor = Color.Transparent;
                }
                if(control is System.Windows.Forms.Button)
                {
                    control.BackColor = Color.DarkSeaGreen;
                    control.EnabledChanged += (sender, e) =>
                    {
                        if (!control.Enabled)
                        {
                            control.BackColor = Color.Gray;
                        }
                        else
                        {
                            control.BackColor = Color.DarkSeaGreen;
                        }
                    };
                }
                if(control is System.Windows.Forms.TextBox)
                {
                    control.Click += (sender, e) =>
                    {
                        control.Text = ""; // Clear the text of the TextBox
                        control.ForeColor = Color.Black;    
                    };
                }
                // Recursively apply font to child controls
                if (control.HasChildren)
                {
                    SetFontForAllControls(control.Controls, font);
                }
            }
        }
    }

}
