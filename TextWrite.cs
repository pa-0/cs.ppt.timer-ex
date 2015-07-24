using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimerEx
{
    public static class TextWrite
    {
        public static Thread thread = new Thread(f =>
        {
            while (Config.Configs.TextActive)
            {
                StreamWriter sw = new StreamWriter(Config.Configs.TextPath);
                sw.Write(Variables.CompileString(Config.Configs.Text));
                sw.Close();

                Thread.Sleep(Config.Configs.TextSleep);
            }
        });
    }
}
