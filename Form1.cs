using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimerEx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Config.Load();
            SetParams();

            Variables.thread.Start();

            DrawTimeChart(new Point(-1,0));
        }

        private void TimerOpCh(object sender, CancelEventArgs e)
        {
            checkBox2.Checked = true;
        }

        void SetParams()
        {
            SelectedTime = Config.Configs.TimeToEnd.Hour;
            dateTimePicker1.Value = Config.Configs.TimeToEnd;

            richTextBox1.Text = Config.Configs.Text;

            checkBox1.Checked = Config.Configs.FormActive;
            checkBox1_CheckedChanged(null, null);
            numericUpDown1.Value = Config.Configs.FormWidth;
            numericUpDown2.Value = Config.Configs.FormHeight;
            checkBox2.Checked = Config.Configs.FormVisible;
            numericUpDown3.Value = Config.Configs.FormSleep;
            label1.Text = Config.Configs.FormFont.FontFamily.Name;
            label1.Font = new Font(label1.Text, 8.25f);
            button3.BackColor = Config.Configs.FormFontColor;
            label3.Text = "R: " + Config.Configs.FormFontColor.R + "G: " + Config.Configs.FormFontColor.G + "B: " + Config.Configs.FormFontColor.B;
            button4.BackColor = Config.Configs.FormBackColor;
            label4.Text = "R: " + Config.Configs.FormBackColor.R + "G: " + Config.Configs.FormBackColor.G + "B: " + Config.Configs.FormBackColor.B;

            checkBox4.Checked = Config.Configs.ImageActive = false;
            checkBox4_CheckedChanged(null, null);
            numericUpDown5.Value = Config.Configs.ImageWidth;
            numericUpDown6.Value = Config.Configs.ImageHeight;
            numericUpDown4.Value = Config.Configs.ImageSleep;

            label7.Text = Config.Configs.ImageFont.FontFamily.Name;
            label7.Font = new Font(label7.Text,8.25f);

            button9.BackColor = Config.Configs.ImageFontColor;
            label9.Text = "R: " + Config.Configs.ImageFontColor.R + "G: " + Config.Configs.ImageFontColor.G + "B: " + Config.Configs.ImageFontColor.B;

            button8.BackColor = Config.Configs.ImageBackColor;
            label8.Text = "A: " + Config.Configs.ImageBackColor.A + "R: " + Config.Configs.ImageBackColor.R + "G: " + Config.Configs.ImageBackColor.G + "B: " + Config.Configs.ImageBackColor.B;

            checkBox3.Checked = Config.Configs.TextActive;
            checkBox3_CheckedChanged(null, null);
            numericUpDown7.Value = Config.Configs.TextSleep;
            textBox1.Text = Config.Configs.TextPath;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            Config.Configs.Text = richTextBox1.Text;
        }
        
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Config.Configs.TimeToEnd = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day,SelectedTime,0,0);
        }

        private int SelectedTime = 0;

        public void DrawTimeChart(Point click)
        {
            Image img = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            float hu = pictureBox1.Width/4, wu = pictureBox1.Height/6;

            if (click.X!=-1)
            SelectedTime = ((click.Y / (int)wu) + (6 * (click.X / (int)hu)));

            Graphics g = Graphics.FromImage(img);
            g.Clear(Color.Transparent);
            
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //TODO: ВАЖНО!!!
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Font fnt = new Font("Consolas",15);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            for (int i = 0; i < 24; i++)
            {
                Rectangle r = new Rectangle((i / 6) * (int)hu, (i % 6) * (int)wu, (int)hu, (int)wu);

                if (SelectedTime == i) g.FillRectangle(Brushes.Orange, r);
                
                g.DrawRectangle(Pens.Black,r);
                
                g.DrawString(i.ToString("00")+":00",fnt,Brushes.Black,r,sf);
            }

            pictureBox1.Image = img;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            DrawTimeChart(new Point(e.X,e.Y));
            dateTimePicker1_ValueChanged(null, null);
        }
        
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Config.Configs.FormVisible = checkBox2.Checked;
            t.Visible = Config.Configs.FormVisible;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = Config.Configs.FormFont;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                Config.Configs.FormFont = fd.Font;
                label1.Text = fd.Font.FontFamily.Name;
                label1.Font = new Font(label1.Text, 8.25f); ;
            }
        }

        Timer t = new Timer();

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Config.Configs.FormActive = checkBox1.Checked;
            panel1.Enabled = checkBox1.Checked;
            if (checkBox1.Checked)
            {
                t = new Timer();
                t.Closing += TimerOpCh;
                t.Show();
                t.Run();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            Config.Configs.FormSleep = (int)numericUpDown3.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Config.Configs.FormWidth = (int)numericUpDown1.Value;
            Config.Configs.FormHeight = (int)numericUpDown2.Value;

            t.FormSize = new Size(Config.Configs.FormWidth, Config.Configs.FormHeight);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = Config.Configs.FormFontColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Config.Configs.FormFontColor = cd.Color;
                button3.BackColor = cd.Color;
                label3.Text = "R: " + cd.Color.R + "G: " + cd.Color.G + "B: " + cd.Color.B;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = Config.Configs.FormBackColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Config.Configs.FormBackColor = cd.Color;
                button4.BackColor = cd.Color;
                label4.Text = "R: " + cd.Color.R + "G: " + cd.Color.G + "B: " + cd.Color.B;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Config.Configs.FormBackColor =
                Color.FromArgb(
                    Config.Configs.FormFontColor.R == 0
                        ? Config.Configs.FormFontColor.R + 1
                        : Config.Configs.FormFontColor.R - 1, Config.Configs.FormFontColor.G,
                    Config.Configs.FormFontColor.B);
            button4.BackColor = Config.Configs.FormBackColor;
            label4.Text = "R: " + Config.Configs.FormBackColor.R + "G: " + Config.Configs.FormBackColor.G + "B: " +
                          Config.Configs.FormBackColor.B;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "png (*.png) | *.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Config.Configs.ImagePath = sfd.FileName;
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            Config.Configs.ImageSleep = (int)numericUpDown4.Value;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Config.Configs.ImageBackColor = Color.Transparent;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            Config.Configs.ImageWidth = (int)numericUpDown5.Value;
            Config.Configs.ImageHeight = (int)numericUpDown6.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = Config.Configs.ImageFont;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                Config.Configs.ImageFont = fd.Font;
                label7.Text = fd.Font.FontFamily.Name;
                label7.Font = new Font(label7.Text, 8.25f); ;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = Config.Configs.ImageFontColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Config.Configs.ImageFontColor = cd.Color;
                button9.BackColor = cd.Color;
                label9.Text = "R: " + cd.Color.R + "G: " + cd.Color.G + "B: " + cd.Color.B;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = Config.Configs.ImageBackColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Config.Configs.ImageBackColor = cd.Color;
                button8.BackColor = cd.Color;
                label8.Text = "A: " + cd.Color.A + "R: " + cd.Color.R + "G: " + cd.Color.G + "B: " + cd.Color.B;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Config.Configs.ImageActive = checkBox4.Checked;
            panel2.Enabled = Config.Configs.ImageActive;
            if(Config.Configs.ImageActive) ImageDraw.thread.Start();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text file (*.txt) | *.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Config.Configs.TextPath = sfd.FileName;
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            Config.Configs.TextSleep = (int)numericUpDown7.Value;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Config.Configs.TextActive = checkBox3.Checked;
            panel3.Enabled = Config.Configs.TextActive;
            if (Config.Configs.TextActive) TextWrite.thread.Start();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowInTaskbar = false;
            Hide();
            notifyIcon1.Visible = true;
            Config.Save();
            e.Cancel = true;
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Variables.Close = true;
            Config.Save();
            Environment.Exit(0);
        }
    }
}
