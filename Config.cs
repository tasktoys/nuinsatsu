using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace NUInsatsu
{
    /// <summary>
    /// コンフィグの情報を読み込み・書き込みします。
    /// </summary>
    public class Config
    {
        const String fileName = "config.xml";

        public String MotionAlgorithm { get; set; }
        public int ServerPort { get; set; }
        public int NuimagioPort { get; set; }
        public String NuimagioIP { get; set; }
        public double VoiceOK { get; set; }
        public double VoiceNO { get; set; }
        public double VoiceYES { get; set; }
        public double VoiceNext { get; set; }
        public double VoiceKinect { get; set; }
        public double VoiceEntry { get; set; }
        public double VoiceBack { get; set; }
        public double VoiceScan { get; set; }
        public double VoicePrint { get; set; }
        public double VoiceBalse { get; set; }
        public bool DummyFace { get; set; }
        public String PrinterName { get; set; }
        public int MotionTime { get; set; }

        /// <summary>
        /// コンフィグのデフォルト値を設定し、クラスを構築します。
        /// 設定値の読み込みはLoadメソッドを利用してください。
        /// </summary>
        private Config()
        {
            ServerPort = 50001;
            MotionAlgorithm = "5";
            NuimagioIP = "127.0.0.1";
            NuimagioPort = 50002;
            VoiceOK = 0.5;
            VoiceYES = 0.5;
            VoiceNO = 0.8;
            VoiceNext = 0.9;
            VoiceKinect = 0.5;
            VoiceEntry = 0.9;
            VoiceBack = 0.9;
            VoiceScan = 0.8;
            VoicePrint = 0.8;
            VoiceBalse = 0.99;
            DummyFace = true;
            PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters[0];
            MotionTime = 3;
        }

        /// <summary>
        /// 設定ファイルを読み込みます。設定ファイルが無い場合は、デフォルト値が設定されています。
        /// </summary>
        /// <returns>設定を読み込んだインスタンス</returns>
        public static Config Load()
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Config));
                    return (Config)serializer.Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                return new Config();
            }
        }

        /// <summary>
        /// 設定を保存します。
        /// </summary>
        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));

            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create);

            serializer.Serialize(fs, this);
            fs.Close();
        }
    }


}
