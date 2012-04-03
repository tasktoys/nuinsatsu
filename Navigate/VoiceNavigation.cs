using SpeechLib;
using NUInsatsu.Kinect;

namespace NUInsatsu.Navigate
{
    /// <summary>
    /// 音声ファイル再生クラス
    /// </summary>
    class VoiceNavigation
    {
		private SpeechVoiceSpeakFlags flg;
		private SpeechVoiceSpeakFlags cdflg;
		private SpVoice tts;
		private System.Threading.Timer cdtimer;
		private int counter;

		public VoiceNavigation()
		{
            try
            {
                tts = new SpVoice();      // 音声合成のオブジェクト
                ISpeechObjectTokens voiceInfo;    // 音声の情報
                voiceInfo = tts.GetVoices("", "");
                string haruka = "Microsoft Server Speech Text to Speech Voice (ja-JP, Haruka)";
                int i = 0;
                while (true)
                {
                    tts.Voice = voiceInfo.Item(i);// 音声の設定
                    string name = tts.Voice.GetAttribute("NAME");
                    if (name.CompareTo(haruka) == 0)
                    {
                        break;
                    }
                    i++;
                }
                System.Console.Out.WriteLine();

                tts.Volume = int.Parse("100");                // 音量の設定
                tts.Rate = int.Parse("-1");                    // 速度の設定
                flg = SpeechVoiceSpeakFlags.SVSFIsXML /*| SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak*/;
                cdflg = SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFlagsAsync /*| SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak*/;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                throw new NotInstalledSpeechLibraryException();
            }
		}

		public void PlaySoundSync(string text)
		{
			Kinect.VoiceDictionary voiceDictionary = new Kinect.VoiceDictionary();
			string navi = voiceDictionary.DiscToNavi(text);
            PlaySync(navi);
		}

        public void PlaySoundASync(string text)
        {
            Kinect.VoiceDictionary voiceDictionary = new Kinect.VoiceDictionary();
            string navi = voiceDictionary.DiscToNavi(text);
            PlayASync(navi);
        }

		private void PlayASync(string text)
		{
			string s = text.Replace("<", "&lt;");
			s = "<pitch absmiddle=\"" + "4" + "\">" + s + "</pitch>";
			tts.Speak(s, cdflg);          // 読み上げ
		}

		private void PlaySync(string text)
		{
			string s = text.Replace("<", "&lt;");
			s = "<pitch absmiddle=\"" + "4" + "\">" + s + "</pitch>";
			tts.Speak(s, flg);          // 読み上げ
		}

		public void PlayLength(int length)
		{
			PlaySoundSync("LENGTH1");
			PlayASync(length.ToString());
			PlaySoundSync("LENGTH2");
		}

		public void PlayNum(int num)
		{
			PlayASync(num.ToString());
		}

		public void CountDown(int count)
		{
			counter = count;
			cdtimer = new System.Threading.Timer(
			new System.Threading.TimerCallback(CountDownTimer),
			null, 0, 1000);
		}

		private void CountDownTimer(object o)
		{
			if (counter > 0)
			{
				PlayASync(counter.ToString());
				counter--;
			}
			else
			{
				cdtimer.Dispose();
			}
		}
    }
}
