using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimerEx
{
    public partial class Timer : Form
    {
        public Timer()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            Visible = Config.Configs.FormVisible;
            FormSize = new Size(Config.Configs.FormWidth, Config.Configs.FormHeight);
        }

        public bool Visible
        {
            set
            {
                if (value) Opacity = 0;
                else Opacity = 100;
            }
        }

        public Size FormSize
        {
            set { Size = value; }
        }

        public void Run()
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void Timer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Config.Configs.FormActive)
            {
                e.Cancel = true;
                Opacity = 0;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (Config.Configs.FormActive)
            {
                Image i = new Bitmap(Width,Height);
                Graphics g = Graphics.FromImage(i);

                g.Clear(Config.Configs.FormBackColor);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                g.DrawString( Variables.CompileString(Config.Configs.Text), Config.Configs.FormFont, new SolidBrush(Config.Configs.FormFontColor), new RectangleF(new PointF(0, 0), Size), sf);
                BackgroundImage = i;

                Thread.Sleep(Config.Configs.FormSleep);
            }
            Close();
        }
    }
}
