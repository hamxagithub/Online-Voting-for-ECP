using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Election_Commision_of_Pakistan
{
    public partial class Form3 : Form
    {
        Home H = new Home();
        Form4 form;
        Form5 form5;
        private int check= 0;
        private Point targetLocation;
        private Point initialLocation;
        private const int AnimationDuration = 500; // Animation duration in milliseconds
        private const int AnimationSteps = 50; // Number of animation steps
        private int currentStep = 0;
        public Form3(int i, Home H)
        {
            check = 0;
            this.H = H;
            this.form = new Form4(this);
            form5 = new Form5(this);
            InitializeComponent();
            if (i == 1)
            {
                button1.Enabled = false;       
            }
            else if(i == 2)
            {
                button1.Enabled=true;
                button6.Enabled=false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
            }
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            AnimatePanelToCenter();
            initialize();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);  
        }
        private void AnimatePanelToCenter(int i = 0)
        {
            int panelWidth = panel1.Width;
            int panelHeight = panel1    .Height;

            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            int centerX = (formWidth - panelWidth) / 2+120;
            int centerY = (formHeight - panelHeight) / 2;
            if(i != 0)
            {
                targetLocation = new Point(2*centerX-120, 2*centerY);
            }
            else
            {
                targetLocation = new Point(centerX, centerY);
                this.initialLocation = new Point(centerX-120, centerY);
            }

            
            currentStep = 0;

            timer1.Interval = AnimationDuration / AnimationSteps;
            timer1.Tick += AnimationTimer_Tick;
            timer1.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (currentStep < AnimationSteps)
            {
                // Calculate intermediate position for smooth animation
                int deltaX = (targetLocation.X - panel1.Location.X) / AnimationSteps;
                int deltaY = (targetLocation.Y - panel1.Location.Y) / AnimationSteps;

                panel1.Location = new Point(panel1.Location.X + deltaX, panel1.Location.Y + deltaY);
                panel1.Invalidate();
                currentStep++;
            }
            else
            {
                // Animation complete, stop the timer
                timer1.Stop();
                timer1.Dispose();
                switch(check)
                {
                    case 1:
                        panel1.Location = initialLocation;
                        this.Hide();
                        form.ShowDialog();
                        check = 0;
                        break;
                    case 2:
                        panel1.Location = initialLocation;
                        this.Hide();
                        form5.ShowDialog();
                        check = 0;
                        break;
                    case 3:
                        panel1.Location = initialLocation;
                        Form6 form6 = new Form6(check, this);
                        this.Hide();
                        form6.ShowDialog();
                        check = 0;
                        break;
                    case 4:
                        panel1.Location = initialLocation;
                        Form6 form7 = new Form6(check, this);
                        this.Hide();
                        form7.ShowDialog();
                        check = 0;
                        break;
                    case 5:
                        panel1.Location = initialLocation;
                        Form6 form8 = new Form6(check, this);
                        this.Hide();
                        form8.ShowDialog();
                        check = 0;
                        break;
                    case 6:
                        panel1.Location = initialLocation;
                        Form6 form9 = new Form6(check, this);
                        this.Hide();
                        form9.ShowDialog();
                        check = 0;
                        break;

                }

            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
            dc.DrawImage(Background, ClientRectangle);
            base.OnPaint(e);
        }
        Bitmap Background, Backgroundtemp;

        private void initialize()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            Backgroundtemp = new Bitmap(Properties.Resources.WhatsApp_Image_2024_05_13_at_22_14_16_061725d2);
            Background = new Bitmap(Backgroundtemp, Backgroundtemp.Width, Backgroundtemp.Height);
        }
        public class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                DoubleBuffered = true; // Enable double buffering
            }
        }
        public class OptimizedPanel : Panel
        {
            public OptimizedPanel()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.UserPaint, true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button5.Enabled = true;
            check = 2;
            AnimatePanelToCenter(check);
            initialize();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            check = 3;
            button4.Enabled = true;
            AnimatePanelToCenter(check);
            initialize();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
            check = 4;
            AnimatePanelToCenter(check);
            initialize();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            check = 5;
            AnimatePanelToCenter(check);
            initialize();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            check = 6;
            AnimatePanelToCenter(check);
            initialize();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            H.Show();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            check = 1;
            AnimatePanelToCenter(check);
            initialize();
        }
    }
}
