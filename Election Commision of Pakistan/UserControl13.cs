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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Oracle.ManagedDataAccess.Client;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl13 : UserControl
    {
        OracleConnection con;
        DateTime[] date = new DateTime[3];
        string text;
        public UserControl13(string text)
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
        private void PopulateTimeSlots()
        {
            comboBox1.Items.Clear();
            DateTime selectedDate = date[0].Date;
            DateTime startTime = selectedDate.AddHours(8); // Start time (e.g., 8:00 AM)
            DateTime endTime = selectedDate.AddHours(20);  // End time (e.g., 8:00 PM)
            TimeSpan interval = TimeSpan.FromMinutes(30);   // Time interval (e.g., 30 minutes)

            string timeFormat = "h:mm tt"; // Define the expected time format

            while (startTime <= endTime)
            {
                string formattedTime = startTime.ToString(timeFormat); // Format time as string

                comboBox1.Items.Add(formattedTime); // Add formatted time to ComboBox

                startTime = startTime.Add(interval); // Increment by interval
            }

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0; // Select first time slot by default
            }
        }

        private void comboBoxTimeSlots_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTime = comboBox1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedTime))
            {
                // Attempt to parse the selected time string into a TimeSpan
                if (DateTime.TryParseExact(selectedTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
                {
                    // Combine the date part from Date[0] with the time part from the parsed DateTime
                    DateTime selectedDateTime = date[0].Date + parsedDateTime.TimeOfDay;

                    // Store the selected DateTime into Date[2]
                    date[2] = selectedDateTime;

                    // Update the text box with the selected time
                    textBox1.Text = selectedTime;

                    // Hide the ComboBox after selecting a time
                    comboBox1.Visible = false;
                }
                else
                {
                    // Handle parsing error (selectedTime format does not match "h:mm tt")
                    MessageBox.Show("Invalid time format. Please select a valid time.");
                }
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            string[] para = new string[4];
            string[] attri = new string[4];
            try
            {
                attri[0] = "ElectionType";
                para[0] = textBox12.Text;
                attri[1] = "ElectionDate";
                para[1] = textBox11.Text;
                attri[2] = "NominationDeadline";
                para[2] = textBox8.Text;
                attri[3] = "PollingTime";
                para[3] = textBox1.Text;
                for (int i = 0; i < 4; i++)
                {
                    if (para[i] != "" && !ContainsDigit(para[i]))
                    {
                        //para[i] = "'" + para[i] + "'";
                        string sql1 = "UPDATE Election SET " + attri[i] + " = :name where ElectionId = :id";

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
                            MessageBox.Show("Election is rescheduled");
                        }
                        con.Close();
                    }
                    else if (ContainsDigit(para[i]))
                    {
                        string sql2 = "UPDATE Election SET " + attri[i] + " = :name where ElectionId = :id";

                        con.Open();
                        OracleCommand getEmps = new OracleCommand(sql2, con);
                        int  mo = 0;
                        if (int.TryParse(text, out mo)&& para[i] != "")
                        {
                            getEmps.Parameters.Add("name", OracleDbType.Date).Value = DateTime.Now;
                            getEmps.Parameters.Add("id", OracleDbType.Int32).Value = mo;
                        }

                        int row = getEmps.ExecuteNonQuery();
                        if (row > 0)
                        {
                            MessageBox.Show("Election is rescheduled");
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

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            date[0] = dateTimePicker1.Value;
            textBox11.Text = date[0].ToString("dd-MM-yyyy");
            textBox11.ForeColor = Color.Black;
            PopulateTimeSlots();
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)
        {
            date[1] = dateTimePicker3.Value;
            textBox11.ForeColor = Color.Black;
            textBox8.Text = date[1].ToString("dd-MM-yyyy");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateBack();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
            comboBox1.Visible = true;
        }

        private void textBox12_Click(object sender, EventArgs e)
        {
            textBox12.Text = "";
            textBox12.ForeColor = Color.Black;
        }
    }
}
