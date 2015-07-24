using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TimerEx
{
    [DataContract]
    public class Config
    {
        public static Config Configs = new Config();

        public static void Load()
        {
            if (File.Exists("config"))
            {
                StreamReader sr = new StreamReader("config");
                DataContractSerializer serializer = new DataContractSerializer(typeof(Config));
                Configs = (Config)serializer.ReadObject(sr.BaseStream);
                sr.Close();
            }
            else SetDefault();
        }

        public static void Save()
        {
            StreamWriter sw = new StreamWriter("config");
            DataContractSerializer serializer = new DataContractSerializer(typeof(Config));
            serializer.WriteObject(sw.BaseStream, Configs);
            sw.Close();
        }

        public static void SetDefault()
        {
            Configs.Text = "До Нового Года осталось\r\n(if (#D==0) then \" \" else \"#D <d:#D>\") #H:#M:#S:#T";
            Configs.TimeToEnd = new DateTime(DateTime.Now.Year, 12, 31, 0, 0, 0);

            Configs.FormActive = false;
            Configs.FormWidth = 800;
            Configs.FormHeight = 600;
            Configs.FormVisible = false;
            Configs.FormSleep = 150;

            Configs.FormFont = new Font("Consolas", 16);
            Configs.FormFontColor = Color.Black;
            Configs.FormBackColor = Color.White;

            Configs.ImageActive = false;
            Configs.ImageWidth = 800;
            Configs.ImageHeight = 600;
            Configs.ImageSleep = 1100;
            Configs.ImagePath = "default.png";

            Configs.ImageFont = new Font("Consolas", 16);
            Configs.ImageFontColor = Color.Black;
            Configs.ImageBackColor = Color.White;

            Configs.TextActive = false;
            Configs.TextSleep = 1100;
            Configs.TextPath = "default.txt";
        }



        #region field
        
        public DateTime TimeToEnd
        {
            set
            {
                Year = value.Year;
                Month = value.Month;
                Day = value.Day;
                Hour = value.Hour;
            }
            get
            {
                return new DateTime(Year, Month, Day, Hour, 0, 0);
            }
        }

        [DataMember]
        private int Year;
        [DataMember]
        private int Month;
        [DataMember]
        private int Day;
        [DataMember]
        private int Hour;
        //
        [DataMember]
        public string Text;
        //
        [DataMember]
        public bool FormActive;
        [DataMember]
        public int FormWidth;
        [DataMember]
        public int FormHeight;
        [DataMember]
        public bool FormVisible;
        [DataMember]
        public int FormSleep;
        
        public Font FormFont
        {
            set
            {
                formFontFamily = value.FontFamily.Name;
                formFontSize = value.Size;
            }
            get
            {
                return new Font(formFontFamily, formFontSize);
            }
        }
        [DataMember]
        private string formFontFamily;
        [DataMember]
        private float formFontSize;
        
        public Color FormFontColor
        {
            set { formFontColor = ColorTranslator.ToHtml(value); }
            get { return ColorTranslator.FromHtml(formFontColor); }
        }
        [DataMember]
        private string formFontColor;
        
        public Color FormBackColor
        {
            set { formBackColor = ColorTranslator.ToHtml(value); }
            get { return ColorTranslator.FromHtml(formBackColor); }
        }
        [DataMember]
        private string formBackColor;
        //
        [DataMember]
        public bool ImageActive;
        [DataMember]
        public int ImageWidth;
        [DataMember]
        public int ImageHeight;
        [DataMember]
        public int ImageSleep;
        [DataMember]
        public string ImagePath;
        
        public Font ImageFont
        {
            set
            {
                imageFontFamily = value.FontFamily.Name;
                imageFontSize = value.Size;
            }
            get
            {
                return new Font(imageFontFamily, imageFontSize);
            }
        }
        [DataMember]
        private string imageFontFamily;
        [DataMember]
        private float imageFontSize;
        
        public Color ImageFontColor
        {
            set { imageFontColor = ColorTranslator.ToHtml(value); }
            get { return ColorTranslator.FromHtml(imageFontColor); }
        }
        [DataMember]
        private string imageFontColor;
        
        public Color ImageBackColor
        {
            set { imageBackColor = ColorTranslator.ToHtml(value); }
            get { return ColorTranslator.FromHtml(imageBackColor); }
        }
        [DataMember]
        private string imageBackColor;
        //
        [DataMember]
        public bool TextActive;
        [DataMember]
        public string TextPath;
        [DataMember]
        public int TextSleep;
        #endregion
    }
}
