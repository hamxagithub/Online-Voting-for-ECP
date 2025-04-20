using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Election_Commision_of_Pakistan
{
    
    public partial class Form2 : Form
    {
        public class V
        {
            int VID;
            int CID;
            string Etype;
            string IID = "";
            int PID = 0;
            int Integrity;
            public void EAdd(string E)
            {
                Etype = E;
            }
            public void VAdd(int i)
            {
                VID = i;
            }
            public void CAdd(int i)
            {
                CID = i;
            }
            public void PAdd(int i)
            {
                PID = i;
            }
            public void IAdd(string i)
            {
                IID = i;
            }
            public void IIAdd(int i = 0)
            {
                if (IID.Length != 10)
                {
                    Integrity = 0;
                }
                if (i != 0)
                {
                    Integrity = i;
                }
            }
            public int VOut()
            {
                return VID;
            }
            public string EOut()
            {
                return Etype;
            }
            public int COut()
            {
                return CID;
            }
            public int POut()
            {
                return PID;
            }
            public string IOut()
            {
                return IID;
            }
            public int IIOut()
            {
                return Integrity;
            }
        }
        MyList<Image> I = new MyList<Image>();
        MyList<string> N = new MyList<string>();
        MyList<int> Cid = new MyList<int>();
        MyList<string> E = new MyList<string>();
        OracleConnection conn;
        Form1 previous;
        V Vote = new V();
        int count = 0, count1 = 0;
        int Id;
        bool check = false;
        public Form2(Form1 prev, int VID, int ID, bool i)
        {
            check = i;
            previous = prev;
            Id = ID;
            Vote.PAdd(ID);
            Vote.VAdd(VID);
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            string conStr = @"DATA SOURCE=localhost:1521/xe;USER ID=MYECP;Password=12345";
            conn = new OracleConnection(conStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (check)
            {
                panel4.Visible = true;
                panel4.BringToFront(); // Bring panel to the front within its parent container

                panel1.Visible = false;
                panel3.Visible = false;
            }
            else
            {
                panel1.Visible = false;
                panel4.Visible = false;
                panel3.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (I.Number(0) > 0)
            {
                Vote.EAdd(E.Ahead());
                Vote.CAdd(Cid.Ahead()); 
                Image temp = I.Ahead();
                pictureBox1.Image = temp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                string temp1 = N.Ahead();
                label1.Text = temp1;
                count--;
                count1++;

            }
            if (count == 0)
            {
                button2.Enabled = false;
                button2.BackColor = Color.Gray;
            }
            if (count1 > 0)
            {
                button3.Enabled = true;
                button3.BackColor = Color.White;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel3.Visible = false;
            panel1.Visible = true;
            try
            {
                conn.Open();

                // Retrieve the BLOB image data from the database
                string sql = "Select L.name, L.CandidateID, L.ElectionType, S.image from pollingstations P " +
                    "inner join Constituencies C on P.constituencyid = C.constituencyid " +
                    "inner join Candidates L " +
                    "on L.constituencyid = C.constituencyid " +
                    "inner join politicalparties N " +
                    "on N.partyid = L.partyid " +
                    "inner join Symbols S " +
                    "on N.symbol = S.imageid " +
                    "where P.pollingstationid = :id";

                Console.WriteLine("Generated SQL Query: " + sql);
                OracleCommand command = new OracleCommand(sql, conn);
                command.Parameters.Add("id", OracleDbType.Int32).Value = Id; // Provide the image ID
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(reader.GetOrdinal("name"));
                    int tid = reader.GetInt32(reader.GetOrdinal("candidateid"));
                    string Etype = reader.GetString(reader.GetOrdinal("electiontype"));
                    OracleBlob oracleBlob = reader.GetOracleBlob(reader.GetOrdinal("image"));
                    byte[] imageData = new byte[oracleBlob.Length];
                    oracleBlob.Read(imageData, 0, (int)oracleBlob.Length);

                    // Convert byte array to Image
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(ms);

                        // Display image in PictureBox or perform further processing
                        I.Add(image);
                        N.Add(name);
                        Cid.Add(tid);
                        E.Add(Etype);
                    }
                    count++;
                }

                reader.Close();
                conn.Close();
                if (I.Number(0) > 0)
                {
                    Vote.EAdd(E.Ahead());
                    Vote.CAdd(Cid.Ahead());
                    string temp1 = N.Ahead();
                    label1.Text = temp1;
                    Image temp = I.Ahead();
                    pictureBox1.Image = temp;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    count--;
                    count1++;
                }
                if (count == 0)
                {
                    button2.Enabled = false;
                    button2.BackColor = Color.Gray;
                }
                if (I.Number(0) == 0)
                {
                    button3.Enabled = false;
                    button3.BackColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2.Checked = false;
            radioButton1.Checked = true;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            Vote.IAdd(textBox1.Text.ToString());
            if (Id == 0)
            {
                Vote.IIAdd();
            }
            else
            {
                if (radioButton1.Checked == true)
                {
                    Vote.IIAdd(1);
                }
                else if (radioButton2.Checked == true)
                {
                    Vote.IIAdd(0);
                }
            }
            conn.Open();
            string sql = "INSERT INTO OnlineVotes (VoterID,Votedate,IP_Address, Voteintegrity, ElectionType, CandidateID) " +
                             "VALUES (:VID, :DateValue,:IID, :Voteintegrity,:Etype, :CID)";
            OracleCommand command = new OracleCommand(sql, conn);
            command.Parameters.Add("VID", OracleDbType.Int32).Value = Vote.VOut();
            
            command.Parameters.Add("DateValue", OracleDbType.Date).Value = DateTime.Now;
            command.Parameters.Add("IID", OracleDbType.Varchar2).Value = Vote.IOut();
            command.Parameters.Add("IIID", OracleDbType.Int32).Value = Vote.IIOut();
            command.Parameters.Add("Etype", OracleDbType.Varchar2).Value = Vote.EOut();
            command.Parameters.Add("CID", OracleDbType.Int32).Value = Vote.COut();
            command.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("The Vote has been Casted", "Message!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

            // Show the previous form (Form1)
            if (previous != null && !previous.IsDisposed)
            {
                previous.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Id == 0)
            {
                Vote.IIAdd();
            }
            else
            {
                if (radioButton1.Checked == true)
                {
                    Vote.IIAdd(1);
                }
                else if (radioButton2.Checked == true)
                {
                    Vote.IIAdd(0);
                }
            }
            conn.Open();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            string sql = "INSERT INTO Votes (VoterID,PollingStationID, Votedate, Voteintegrity,ElectionType, CandidateID) " +
                             "VALUES (:VID, :PID, :VoteDate, :Voteintegrity, :Etype, :Cid)";

            OracleCommand command = new OracleCommand(sql, conn);
            command.Parameters.Add("VID", OracleDbType.Int32).Value = Vote.VOut();
            
            
            command.Parameters.Add("PID", OracleDbType.Int32).Value = Vote.POut();
            command.Parameters.Add("Votedate", OracleDbType.Date).Value = DateTime.Now;
            command.Parameters.Add("Voteintegrity", OracleDbType.Int32).Value = Vote.IIOut();
            command.Parameters.Add("Etype", OracleDbType.Varchar2).Value = Vote.EOut();
            command.Parameters.Add("CID", OracleDbType.Int32).Value = Vote.COut();
            command.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("The Vote has been Casted", "Message!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

            // Show the previous form (Form1)
            if (previous != null && !previous.IsDisposed)
            {
                previous.Show();
            }
        }

        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            if (sender is Panel panel && panel.Visible)
            {
                // Calculate new position to center the panel
                int newX = (ClientSize.Width - panel.Width) / 2;
                int newY = (ClientSize.Height - panel.Height) / 2;

                // Set the new location
                panel.Location = new Point(newX, newY);
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (I.Number(1) > 0)
            {
                Vote.EAdd( E.Back());
                Vote.CAdd(Cid.Back());
                Image temp = I.Back();
                pictureBox1.Image = temp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                string temp1 = N.Back();
                label1.Text = temp1;
                count1--;
                count++;
            }
            if (count1 == 0)
            {
                button3.Enabled = false;
                button3.BackColor = Color.Gray;
            }
            if (count > 0)
            {
                button2.Enabled = true;
                button2.BackColor = Color.White;
            }
        }
    }
    public class MyList<T>
    {
        Stack<T> Next;
        Stack<T> Prev;
        T value;
        public MyList()
        {
            Next = new Stack<T>();
            Prev = new Stack<T>();
        }
        public void Add(T V)
        {
            Next.Push(V);
        }
        public T Ahead()
        {
            value = Next.Pop();
            Prev.Push(value);
            return value;
        }
        public T Back()
        {
            value = Prev.Pop();
            Next.Push(value);
            return value;
        }
        public int Number(int i)
        {
            if (i == 0)
            {
                return Next.Count;
            }
            else if (i == 1)
            {
                return Prev.Count;
            }
            return 0;
        }
    }
}
