using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TimerEx
{
    public static class Variables
    {
        public static TimeSpan time;

        public static Thread thread = new Thread(f =>
        {
            while (!Close)
            {
                time = Config.Configs.TimeToEnd - DateTime.Now;
                if(time.TotalMilliseconds<=0) time = new TimeSpan(0);
                Thread.Sleep(10);
            }
        });

        public static bool Close = false; 

        public static string CompileString(string str)
        {
            string txt = str;

            int z = 0;
            
            Regex r;
            MatchCollection mc;
            string[] pattern = new[]
            {@"#\w+", @"<(.*?)>", "\\(if.*?\\((.*?)\\).*?then.*?\"(.*?)\".*?else.*?\"(.*?)\"\\)"};

            Do:
            if (z == 3) goto End;

            r = new Regex(pattern[z]);
            mc = r.Matches(txt);
            if(mc.Count!=0)
                foreach (Match match in mc)
                {
                    switch (z)
                    {
                        case 0:
                            txt = txt.Replace(match.Value, GetStringNumVariables(match.Value));
                            break;
                        case 1:
                            txt = txt.Replace(match.Value, GetStringStringVariables(match));
                            break;
                        case 2:
                            txt = txt.Replace(match.Value, GetStringFromMethod(match));
                            break;
                    }
                }
            z++;
            goto Do;

            End:
            return txt;
        }

        private static string GetStringFromMethod(Match str) //\(if.*?\((.*?)\).*?then.*?"(.*?)".*?else.*?"(.*?)"\)
        {
            return DoFunction(str.Groups[1].Value) ? str.Groups[2].Value : str.Groups[3].Value;
        }

        private static string GetStringStringVariables(Match str) // <(.*?)>
        {
            string[] s = str.Groups[1].Value.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length < 2) return "";
            switch (s[0].Replace(" ",""))
            {
                case "d":
                {
                    double z=0;
                    try
                    {
                        z = Convert.ToDouble(s[1].Replace(" ", "").Replace(",", "."));
                    }
                    catch {return "";}

                    if (z%10 == 1 && (z%100)/10 != 1)
                    {
                        return "день";
                    }
                    else if ((z%10 >= 2 && z%10 <= 4) && (z%100)/10 != 1)
                    {
                        return "дня";
                    }
                    else return "дней";
                }
                default:
                    return "";
            }
        }

        private static string GetStringNumVariables(string str) // #\w+
        {
            switch (str)
            {
                case "#D":
                    return time.Days.ToString();
                case "#H":
                    return time.Hours.ToString("00");
                case "#M":
                    return time.Minutes.ToString("00");
                case "#S":
                    return time.Seconds.ToString("00");
                case "#T":
                    return time.Milliseconds.ToString("000");
                default:
                    return "";
            }
        }

        private static bool DoFunction(string str)
        {
            if (str.Contains("=="))
            {
                try
                {
                string[] s = str.Split(new[] {"=="}, StringSplitOptions.RemoveEmptyEntries);
                return s[0].Replace(" ", "").ToLower() == s[1].Replace(" ", "").ToLower();
                }
                catch
                {
                    return false;
                }
            }
            else if (str.Contains("!="))
            {
                try
                {
                string[] s = str.Split(new[] { "!=" }, StringSplitOptions.RemoveEmptyEntries);
                return s[0].Replace(" ", "").ToLower() != s[1].Replace(" ", "").ToLower();
                }
                catch
                {
                    return false;
                }
            }
            else if (str.Contains("<="))
            {
                try
                {
                    string[] s = str.Split(new[] { "<=" }, StringSplitOptions.RemoveEmptyEntries);
                    return Convert.ToDouble(s[0].Replace(" ", "").ToLower().Replace(",", ".")) <= Convert.ToDouble(s[1].Replace(" ", "").ToLower().Replace(",", "."));
                }
                catch
                {
                    return false;
                }
            }
            else if (str.Contains(">="))
            {
                try
                {
                    string[] s = str.Split(new[] { ">=" }, StringSplitOptions.RemoveEmptyEntries);
                    return Convert.ToDouble(s[0].Replace(" ", "").ToLower().Replace(",", ".")) >= Convert.ToDouble(s[1].Replace(" ", "").ToLower().Replace(",", "."));
                }
                catch
                {
                    return false;
                }
            }
            else if (str.Contains("<"))
            {
                try
                {
                    string[] s = str.Split(new[] { "<" }, StringSplitOptions.RemoveEmptyEntries);
                    return Convert.ToDouble(s[0].Replace(" ", "").ToLower().Replace(",", ".")) < Convert.ToDouble(s[1].Replace(" ", "").ToLower().Replace(",", "."));
                }
                catch
                {
                    return false;
                }
            }
            else if (str.Contains(">"))
            {
                try
                {
                    string[] s = str.Split(new[] {">"}, StringSplitOptions.RemoveEmptyEntries);
                    return Convert.ToDouble(s[0].Replace(" ", "").ToLower().Replace(",", ".")) >
                           Convert.ToDouble(s[1].Replace(" ", "").ToLower().Replace(",", "."));
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }
    }
}
