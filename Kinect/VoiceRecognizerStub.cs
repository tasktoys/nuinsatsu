using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Kinect
{
    class VoiceRecognizerStub : IVoiceRecognizer
    {
        /// <summary>
        /// 登録した音声が認識された時に呼び出されるイベントです.
        /// </summary>
        public event EventHandler<SaidWordArgs> Recognized;
        /// <summary>
        /// 音声を推論する場合に呼び出されるイベントです.
        /// </summary>
        public event EventHandler<SaidWordArgs> Hypothesized;
        /// <summary>
        /// 登録されていない音声が認識された時に呼び出されるイベントです.
        /// </summary>
        public event EventHandler<SaidWordArgs> RecognitionRejected;

        public VoiceRecognizerStub()
        {
            // 一回は使ってやらないと警告がうるさいので使ってあげる
            SaidWordArgs e = new SaidWordArgs();
            if (Recognized != null)
            {
                Recognized(this, e);
            }
            if (Hypothesized != null)
            {
                Hypothesized(this, e);
            }
            if (RecognitionRejected != null)
            {
                RecognitionRejected(this, e);
            }
        }

        /// <summary>
        /// 認識する文字を追加します
        /// </summary>
        /// <param name="word">認識する文字</param>
        /// <param name="threshold">認識する制度</param>
        public void AddWord(String word, double threshold) { }

        /// <summary>
        /// 音声認識を開始します.
        /// </summary>
        public void Start() { }

        /// <summary>
        /// 音声認識を中断します.
        /// </summary>
        public void Stop() { }
    }
}
