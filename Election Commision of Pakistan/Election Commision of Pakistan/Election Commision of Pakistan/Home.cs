using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;


namespace Election_Commision_of_Pakistan
{
    public partial class Home : Form
    {
        private const int AnimationDuration = 5000; // Animation duration in milliseconds
        private DateTime animationStartTime;
        private bool isAnimating = false;
        public Home()
        {
            InitializeComponent();
            Font newFont = new Font("Mistral", 9, FontStyle.Regular);

            // Apply the new font to all controls on this form
            ControlHelper.SetFontForAllControls(this.Controls, newFont);
            timer1.Interval = 50; // Timer interval for smooth animation (adjust as needed)
            timer1.Tick += timer1_Tick;
            StartAnimation();
            initialize();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isAnimating)
            {
                TimeSpan elapsedTime = DateTime.Now - animationStartTime;
                double progress = elapsedTime.TotalMilliseconds / AnimationDuration;

                if (progress >= 1.0)
                {
                    // Animation complete
                    isAnimating = false;
                    panel1.Size = new Size(200, 200); // Set final size
                }
                else
                {
                    // Interpolate size based on animation progress
                    int targetWidth = 200; // Final width of the panel
                    int targetHeight = 200; // Final height of the panel
                    int currentWidth = (int)(progress * targetWidth);
                    int currentHeight = (int)(progress * targetHeight);
                    int newX = (ClientSize.Width - panel1.Width) / 2;
                    int newY = (ClientSize.Height - panel1.Height) / 2;

                    // Set the new location
                    panel1.Location = new Point(newX, newY);
                    panel1.Size = new Size(currentWidth, currentHeight);
                }
            }
        }       
        private void StartAnimation()
        {
            // Start the animation
            animationStartTime = DateTime.Now;
            isAnimating = true;
            panel1.Size = new Size(1, 1); // Start with a small size
     // Initial content
            timer1.Start(); // Start the timer
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button3.Enabled = true;
            Form3 form = new Form3(1, this);
            this.Hide();
            form.ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button2.Enabled = true;
            Form7 form = new Form7(2, this);
            this.Hide();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button4.Enabled = true;
            Form7 form = new Form7(3, this);
            this.Hide();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            button5.Enabled = true;
            Form1 form = new Form1(this);
            this.Hide();
            form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            Form8 form = new Form8(this);
            this.Hide();
            form.ShowDialog();
        }
        public class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                DoubleBuffered = true; // Enable double buffering
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
        public class OptimizedPanel : Panel
        {
            public OptimizedPanel()
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.UserPaint, true);
            }
        }
    }
}
