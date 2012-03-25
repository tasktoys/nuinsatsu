﻿using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUInsatsu.Kinect;

namespace NUInsatsu.Net
{
    /// <summary>
    /// Kinect制御アプリケーションの接続方法インターフェースKinectClientの実装です.
    /// NUInsatsuを起動しているマシンに直接キネクトを接続している場合です.
    /// </summary>
    class KinectClientLocal : KinectClient
    {
        /// <summary>
        /// キネクトと言うまで待機するイベント
        /// </summary>
        private readonly AutoResetEvent SaidKinectEvent = new AutoResetEvent(false);

        public bool Connect()
        {
            return true;
        }

        public bool Close()
        {
            return true;
        }

        public List<List<Dictionary<String, Double>>> GetMotionList()
        {
            //キネクトと発音されるまで待機
            waitSaidKinect();

            Config config = Config.Load();
            String xml = MakeMotionXML(config.MotionTime);

            Dictionary<String, Double> d = new Dictionary<string, double>();
            List<Dictionary<String, Double>> l = new List<Dictionary<string, double>>();
            l.Add(d);
            List<List<Dictionary<String, Double>>> l2 = new List<List<Dictionary<string, double>>>();
            l2.Add(l);

            return l2;
        }

        public void SendNavigation(String str)
        {
        }

        /// <summary>
        /// キネクトと発音されるまで待機します。
        /// </summary>
        private void waitSaidKinect()
        {
            System.Console.WriteLine("[KinectClientLocal]wait voice recognized kinect");

            VoiceRecognizer recognizer = VoiceRecognizer.GetInstance();

            //キネクトと呼ばれると、waitoneから先に進めるようになるイベントをセット
            EventHandler<SaidWordArgs> voice_RecognizedHandler = new EventHandler<SaidWordArgs>(voice_Recognized);
            recognizer.Recognized += voice_RecognizedHandler;
            //キネクトと発音されるまで待機
            SaidKinectEvent.WaitOne();
            //イベントを削除
            recognizer.Recognized -= voice_RecognizedHandler;
        }

        /// <summary>
        /// ボイスがRecognizedされた場合に呼び出されます.
        /// 「キネクト」と発音すると、GetMotionListのwaitSaidKinectでwaitされている部分から先に進みます。
        /// </summary>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="e">イベント情報</param>
        private void voice_Recognized(object sender, SaidWordArgs e)
        {   
            VoiceDictionary voicedic = new VoiceDictionary();
            String rectext = "";
            rectext = voicedic.RecognizeToCommand(e.Text);
            if (rectext == "kinect")
            {
                SaidKinectEvent.Set();
            }
        }

        /// <summary>
        /// キーとして利用してよいモーションかどうか検証します
        /// 利用してはいけないモーションとは、
        /// 識別人数が０、もしくは識別人数が途中で増減した場合です
        /// </summary>
        /// <param name="motions">識別するモーション</param>
        /// <returns>キーとして利用してよい場合はtrue</returns>
        private bool IsValidMotionList(MotionList motions)
        {
            //最初に認識している人数を調べる。0人だったらロストフレーム例外
            SkeletonDataList skeletonDataList = motions[0];
            int firstHeadCount = skeletonDataList.Count;
            if (firstHeadCount == 0) return false;

            //最初に認識している人数と違い増減があったらロストフレーム例外
            foreach (SkeletonDataList sDataList in motions)
            {
                if (firstHeadCount != sDataList.Count) return false;
            }

            return true;
        }

        /// <summary>
        /// /// NMXPのMotionRequestに対するResponseのメッセージを生成します
        /// </summary>
        /// <param name="motiontime">モーション取得時間</param>
        /// <returns>生成されたメッセージ</returns>
        private String MakeMotionXML(int motiontime)
        {
            String sendMessage = null;

            using (ISkeletonSensor skeletonSensor = KinectUtility.CreateSkeletonSensorInstance() )
            {
                // モーションを取得する
                MotionList motions = skeletonSensor.GetMotionForSeconds(motiontime);

                if (IsValidMotionList(motions))
                {
                    //キーを数える
                    Motion.KeyMaker keyMaker = new Motion.KeyMaker();
                    String str = keyMaker.Make(motions);
                    sendMessage = str;
                }
                else
                {
                    sendMessage = "<?xml version=\"1.0\"?><nmxp version=\"2.0\"><error>LostFrameException</error></nmxp>";
                }

                using (StreamWriter writer = new StreamWriter("motion_response.txt"))
                {
                    writer.Write(sendMessage);
                }
            }

            return sendMessage;
        }
    }
}