﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace NUInsatsu.Kinect
{

	class VoiceDictionary
	{
		// コマンド→ナビの辞書
		Dictionary<String, String> navidic = new Dictionary<String, String>();
		// 認識→コマンドの辞書
        Dictionary<String, String> commdic = new Dictionary<String, String>();
		
		public VoiceDictionary()
		{
			AddNaviDic();
			AddCommDic();
		}


        /// <summary>
        /// 読み上げ用辞書への追加
        /// </summary>
		private void AddNaviDic() {
			navidic.Add("WELCOME", "ニュイマジオへようこそ");
            navidic.Add("ENTRY_OR_PRINT", "印刷または登録を選択してください");

            navidic.Add("PRINTING", "印刷中です");
            navidic.Add("END_ENTRY", "登録が完了しました");

            navidic.Add("SCAN_START", "紙を設置し、スキャンを選択してください");
			navidic.Add("SCAN_COMPLETE", "スキャンが完了しました");

			navidic.Add("CONFIRM_FACEPASS", "顔認証を追加しますか？");

            navidic.Add("WAIT_FACE", "顔情報取得の準備が完了しました。");
            navidic.Add("START_FACE", "顔情報の取得を開始します");
            navidic.Add("END_FACE", "顔情報の取得が完了しました");

            navidic.Add("START_MOTION", "モーションの取得を開始します");
			navidic.Add("WAIT_MOTION", "モーション取得の準備が完了しました。");
            navidic.Add("END_MOTION", "モーションの取得が完了しました");
		}

		/// <summary>
        /// コマンド用辞書への追加
		/// </summary>
		private void AddCommDic()
		{
			commdic.Add("おーけー", "OK");
			commdic.Add("次へ", "NEXT");
			commdic.Add("スキャン", "SCAN");
			commdic.Add("いんさつ", "PRINT");
			commdic.Add("キネクト", "kinect");
            commdic.Add("とうろく", "ENTRY");
            commdic.Add("もどる", "BACK");
            commdic.Add("いえす", "YES");
            commdic.Add("のー", "NO");
            commdic.Add("ばるす", "BALSE");
		}

        /// <summary>
        /// VoiceRecognizerに認識する音声を追加します.
        /// </summary>
        /// <param name="voiceRec">音声を追加するVoiceRecognizer</param>
		public void AddDic(IVoiceRecognizer voiceRec)
		{
            NUInsatsu.Config config = NUInsatsu.Config.Load();

            Dictionary<String, Double> voiceThresholdTable = new Dictionary<string, double>();

            voiceThresholdTable.Add("おーけー", config.VoiceOK);
            voiceThresholdTable.Add("次へ", config.VoiceNext);
            voiceThresholdTable.Add("スキャン", config.VoiceScan);
            voiceThresholdTable.Add("いんさつ", config.VoicePrint);
            voiceThresholdTable.Add("キネクト", config.VoiceKinect);
            voiceThresholdTable.Add("とうろく", config.VoiceEntry);
            voiceThresholdTable.Add("もどる", config.VoiceBack);
            voiceThresholdTable.Add("いえす", config.VoiceYES);
            voiceThresholdTable.Add("のー", config.VoiceNO);
            voiceThresholdTable.Add("ばるす", config.VoiceBalse);

            foreach (String key in voiceThresholdTable.Keys)
            {
                voiceRec.AddWord(key, voiceThresholdTable[key]);
            }
		}


		public String DiscToNavi(string disc)
		{
			try
			{
				return navidic[disc].ToString();
			}
			catch (NullReferenceException)
			{
				System.Console.Out.WriteLine("[VoiceDictionary]navigation not found");
				System.Console.Out.WriteLine("message : " + disc);
				return "";
			}
		}

		public String RecognizeToCommand(string rec)
		{
			return commdic[rec].ToString();
		}
	}
}
