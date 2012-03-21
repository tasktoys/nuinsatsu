using System;
using System.Collections;
using System.Collections.Generic;

namespace NUInsatsu
{
	class VoiceDictionary
	{
		//コマンド→ナビの辞書
		Dictionary<String,String> navidic = new Dictionary<String,String>();
		//認識→コマンドの辞書
        Dictionary<String, String> commdic = new Dictionary<String, String>();
		
		public VoiceDictionary()
		{
			AddNaviDic();
			AddCommDic();
		}

		//読み上げ用辞書への追加
		private void AddNaviDic() {
			navidic.Add("WELCOME", "ニュイマジオへようこそ");
			navidic.Add("SELECT_SCAN_OR_PRINT","スキャンまたはプリントを選択してください");

            navidic.Add("PRINTING","印刷中です");
            navidic.Add("END_ENTRY", "登録が完了しました");

            navidic.Add("SCAN_START", "紙を設置し、スキャンを選択してください");
			navidic.Add("SCAN_COMPLETE","スキャンが完了しました");

			navidic.Add("BACK","メニュー画面へ移動します");
			navidic.Add("CONFIRM_FACEPASS","顔認証を追加しますか？");
			navidic.Add("ENTRY_OR_PRINT", "印刷または登録を選択してください");

            navidic.Add("WAIT_FACE", "顔情報取得の準備が完了しました。");
            navidic.Add("START_FACE", "顔情報の取得を開始します");
            navidic.Add("END_FACE", "顔情報の取得が完了しました");

            navidic.Add("START_MOTION", "モーションの取得を開始します");
			navidic.Add("WAIT_MOTION","モーション取得の準備が完了しました。");
            navidic.Add("END_MOTION", "モーションの取得が完了しました");
		}

		//コマンド用辞書への追加
		private void AddCommDic()
		{
			commdic.Add("おーけー","OK");
			commdic.Add("次へ","NEXT");
			commdic.Add("スキャン","SCAN");
			commdic.Add("いんさつ","PRINT");
			commdic.Add("キネクト","kinect");
            commdic.Add("とうろく", "ENTRY");
            commdic.Add("もどる", "BACK");
            commdic.Add("いえす", "YES");
            commdic.Add("のー", "NO");
            commdic.Add("ばるす", "BALSE");
		}

		//認識用辞書への追加
		public void AddDic(VoiceRecognizer voiceRec)
		{
            foreach (String key in commdic.Keys)
            {
                voiceRec.AddWord(key);
            }
		}

		public String discToNavi(string disc)
		{
			try
			{
				return navidic[disc].ToString();
			}
			catch (NullReferenceException e)
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
