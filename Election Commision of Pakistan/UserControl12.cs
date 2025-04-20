using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Election_Commision_of_Pakistan
{
    public partial class UserControl12 : UserControl
    {
        OracleConnection con;

        string[] date = new string[3];
        DateTime[] Date = new DateTime[3];
        private enum SelectionState
        {
            FirstTextbox,
            SecondTextbox,
            None
        }

        private SelectionState currentSelectionState = SelectionState.FirstTextbox;
        public UserControl12()
        {
            
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            con = new OracleConnection(conStr);
            panel3.BackColor = Color.FromArgb(100, 0, 0, 0);
            //monthCalendar1.BackColor = Color.FromArgb(100, 0, 0, 0);
            //comboBox1.BackColor = Color.FromArgb(100, 0, 0, 0);        
        }

        private void PopulateTimeSlots()
        {
            comboBox1.Items.Clear();
            DateTime selectedDate = Date[0].Date;
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
                    DateTime selectedDateTime = Date[0].Date + parsedDateTime.TimeOfDay;

                    // Store the selected DateTime into Date[2]
                    Date[2] = selectedDateTime;
                    textBox1.ForeColor = Color.Black;
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
            string sql = "INSERT INTO Election(Electionid, ElectionType, ElectionDate, NominationDeadline, PollingTime) " +
              "VALUES (:id, :Etype, :date1, :date2, :date3)";

            try
            {
                using (OracleCommand getEmps = new OracleCommand(sql, con))
                {
                    int ID;
                    if (int.TryParse(textBox13.Text, out ID) &&
                        textBox11.Text != "" && textBox8.Text != ""&& textBox1.Text != "")
                    {
                        con.Open();
                        getEmps.Parameters.Add("id", OracleDbType.Int32).Value = ID;
                        getEmps.Parameters.Add("Etype", OracleDbType.Varchar2).Value = textBox12.Text;
                        getEmps.Parameters.Add("date1", OracleDbType.Date).Value = Date[0];
                        getEmps.Parameters.Add("date2", OracleDbType.Date).Value = Date[1];
                        getEmps.Parameters.Add("date3", OracleDbType.Date).Value = Date[2];
                        int rowsInserted = getEmps.ExecuteNonQuery();
                        con.Close();
                        if (rowsInserted > 0)
                        {
                            MessageBox.Show("Election Scheduled successfully!");
                            
                        }
                        else
                        {
                            MessageBox.Show("No rows were inserted.");
                        }
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid input for ID or empty fields");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle Exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NavigationManager.NavigateBack();
        }

        private void textBox11_Click(object sender, EventArgs e)
        {
            currentSelectionState = SelectionState.FirstTextbox;
            monthCalendar1.Visible = true;
            textBox11.Text = "";
            textBox11.ForeColor = Color.Black;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = e.Start;

            // Check the current selection state
            if (currentSelectionState == SelectionState.FirstTextbox)
            {
                Date[0] = selectedDate;
                date[0] = selectedDate.ToString("dd-MM-yyyy");
                textBox11.Text = date[0];
                currentSelectionState = SelectionState.SecondTextbox; // Move to the second textbox
                monthCalendar1.Visible = false;
                PopulateTimeSlots();
            }
            else if (currentSelectionState == SelectionState.SecondTextbox)
            {
                Date[1] = selectedDate;
                textBox8.ForeColor = Color.Black;
                textBox8.Text = selectedDate.ToString("dd-MM-yyyy");
                currentSelectionState = SelectionState.None; // Reset selection state
                monthCalendar1.Visible = false; // Hide the MonthCalendar
            }

        }

        private void textBox8_Click(object sender, EventArgs e)
        {
            if (currentSelectionState == SelectionState.SecondTextbox)
            {
                monthCalendar1.Visible = true; // Show the MonthCalendar
            }
            else
            {
                MessageBox.Show("Please select a date for the first textbox first.");
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Visible = true;
        }

        private void textBox13_Click(object sender, EventArgs e)
        {
            textBox13.Text = "";
            textBox13.ForeColor = Color.Black;
        }

        private void textBox12_Click(object sender, EventArgs e)
        {
            textBox12.Text = "";
            textBox12.ForeColor = Color.Black;
        }
    }
}
