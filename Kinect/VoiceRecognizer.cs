using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace NUInsatsu.Kinect
{
    /// <summary>
    /// 音声を認識した際に使われるイベントハンドラです
    /// </summary>
    public class SaidWordArgs : EventArgs
    {
        public String Text { get; set; }
    }

	/// <summary>
	/// キネクトを利用した音声認識を行います。
    /// シングルトンクラスです。インスタンスの取得はGetInstanceを利用します。
	/// </summary>
	class VoiceRecognizer
	{
        private static VoiceRecognizer instance = null;

        public event EventHandler<SaidWordArgs> Recognized;
		public event EventHandler<SaidWordArgs> Hypothesized;
		public event EventHandler<SaidWordArgs> RecognitionRejected;

        private KinectAudioSource kinectSource;
        private readonly SpeechRecognitionEngine sre;
		private readonly RecognizerInfo ri;
		private const string RecognizerId = "SR_MS_ja-JP_TELE_11.0";
        private readonly Dictionary<String, double> voiceThresholdTable = new Dictionary<String, double>();

		private Choices words = new Choices();

        /// <summary>
        /// 認識する文字を追加します
        /// </summary>
        /// <param name="word">認識する文字</param>
        /// <param name="threshold">認識する制度</param>
		public void AddWord(String word,double threshold)
		{
			words.Add(word);
            voiceThresholdTable.Add(word, threshold);
		}

        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        /// <returns>インスタンス</returns>
        public static VoiceRecognizer GetInstance()
        {
            if( instance == null ) instance = new VoiceRecognizer();
            return instance;
        }

        /// <summary>
        /// クラスを構築します。
        /// </summary>
        private VoiceRecognizer()
        {
            ri = SpeechRecognitionEngine.InstalledRecognizers().Where(r => r.Id == RecognizerId).FirstOrDefault();
            if (ri == null)
                return;
                
            sre = new SpeechRecognitionEngine(ri.Id);

            sre.SpeechRecognized += sre_SpeechRecognized;
            sre.SpeechHypothesized += sre_SpeechHypothesized;
            sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sre_SpeechRecognitionRejected);
	    }

		public void Start()
		{
            if (sre != null)
            {
                var gb = new GrammarBuilder();
                gb.Append(words);
                gb.Culture = ri.Culture;

                var g = new Grammar(gb);
                sre.LoadGrammar(g);

                var t = new Thread(startThread);
                t.Start();
            }
		}

        private void startThread()
        {
            kinectSource = new KinectAudioSource();
            kinectSource.SystemMode = SystemMode.OptibeamArrayOnly;
            kinectSource.FeatureMode = true;
            kinectSource.AutomaticGainControl = false;
            kinectSource.MicArrayMode = MicArrayMode.MicArrayAdaptiveBeam;
            var kinectStream = kinectSource.Start();
            sre.SetInputToAudioStream(kinectStream, new SpeechAudioFormatInfo(
                                                  EncodingFormat.Pcm, 16000, 16, 1,
                                                  32000, 2, null));
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

		/// <summary>
		/// 音声認識を終了します。
		/// 以降利用しない場合は必ず呼び出してください
		/// </summary>
        public void Stop()
        {
            if (sre != null)
            {
                sre.RecognizeAsyncCancel();
                sre.RecognizeAsyncStop();
                kinectSource.Dispose();
            }
        }

		/// <summary>
		/// 登録したワードとマッチしない時に呼び出されます
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
			var said = new SaidWordArgs();
			said.Text = e.Result.Text;
			if (RecognitionRejected != null )
				RecognitionRejected(this, said);
        }

		/// <summary>
		/// 推測中のワードがある場合呼び出されます
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
			var said = new SaidWordArgs();
			said.Text = e.Result.Text;

			if (Hypothesized != null)
			{
				Hypothesized(this, said);
			}
        }

		/// <summary>
		/// 登録されたワードと一致した時に呼び出されます
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
			var said = new SaidWordArgs();
			said.Text = e.Result.Text;

            double threshold = voiceThresholdTable[said.Text];

			if (Recognized != null && e.Result.Confidence >= threshold)
			{
                System.Console.WriteLine("[VoiceRecognizer]{0} is Recognized.Confidence:{1}", said.Text, e.Result.Confidence);
				Recognized(this, said);
			}
        }
    }

}
