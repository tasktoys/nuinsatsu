using System;
using System.IO;
using System.Text;

namespace KinectServer
{
    /// <summary>
    /// コンフィグの情報を保持します
    /// </summary>
    struct ConfigData
    {
        public int ServerPort { get; set; }
        public int NuimagioPort { get; set; }
        public String NuimagioIP { get; set; }
        public double VoiceOK { get;  set; }
        public double VoiceNO { get;  set; }
        public double VoiceYES { get;  set; }
        public double VoiceNext { get;  set; }
        public double VoiceKinect { get;  set; }
        public double VoiceEntry { get;  set; }
        public double VoiceBack { get;  set; }
        public double VoiceScan { get;  set; }
        public double VoicePrint { get;  set; }
        public double VoiceBalse { get;  set; }
    }

    class Config
    {



        /// <summary>
        /// サーバの設定を読み込みます
        /// </summary>
        public static ConfigData LoadConfig()
        {
            // port番号設定
            // config.txtのserver_port= の後ろがport番号になる
            StreamReader stream = new StreamReader("config.txt", Encoding.GetEncoding("Shift_JIS"));
            string[] splits;
            string[] sep = { "=" };
            string line = "";

            ConfigData config = new ConfigData();

            while ((line = stream.ReadLine()) != null)
            {
                splits = line.Split(sep, StringSplitOptions.None);
                if (splits[0].CompareTo("server_port") == 0)
                {
                    config.ServerPort = int.Parse(splits[1]);
                }
                if (splits[0].CompareTo("nuimagio_port") == 0)
                {
                    config.NuimagioPort = int.Parse(splits[1]);
                }
                if (splits[0].CompareTo("nuimagio_ip") == 0)
                {
                    config.NuimagioIP = splits[1].ToString();
                }
                else if (splits[0].CompareTo("voice_ok") == 0)
                {
                    config.VoiceOK = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_no") == 0)
                {
                    config.VoiceNO = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_yes") == 0)
                {
                    config.VoiceYES = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_next") == 0)
                {
                    config.VoiceNext = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_kinect") == 0)
                {
                    config.VoiceKinect = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_entry") == 0)
                {
                    config.VoiceEntry = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_entry") == 0)
                {
                    config.VoiceEntry = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_back") == 0)
                {
                    config.VoiceBack = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_scan") == 0)
                {
                    config.VoiceScan = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_print") == 0)
                {
                    config.VoicePrint = double.Parse(splits[1]);
                }
                else if (splits[0].CompareTo("voice_balse") == 0)
                {
                    config.VoiceBalse = double.Parse(splits[1]);
                }
            }

            stream.Close();

            Console.WriteLine("[Config]server_port:" + config.ServerPort.ToString());
            Console.WriteLine("[nuimagio_port]nuimagio_port" + config.NuimagioPort.ToString());
            Console.WriteLine("[nuimagio_ip]nuimagio_ip" + config.NuimagioIP.ToString());

            return config;
        }

    }
}
