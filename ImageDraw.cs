using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimerEx
{
    public static class ImageDraw
    {
        public static Thread thread = new Thread(f =>
        {
            while (Config.Configs.ImageActive)
            {
                Image i = new Bitmap(Config.Configs.ImageWidth, Config.Configs.ImageHeight);
                Graphics g = Graphics.FromImage(i);

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //TODO: ВАЖНО!!!
                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.Clear(Config.Configs.ImageBackColor);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                g.DrawString(Variables.CompileString(Config.Configs.Text), Config.Configs.ImageFont, new SolidBrush(Config.Configs.ImageFontColor), new RectangleF(new PointF(0, 0), i.Size), sf);

                i.Save(Config.Configs.ImagePath,ImageFormat.Png);

                Thread.Sleep(Config.Configs.ImageSleep);
            }
        });
    }
}
