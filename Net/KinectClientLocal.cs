﻿using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUInsatsu.Kinect;
using NUInsatsu.Motion;
using NUInsatsu.Navigate;

namespace NUInsatsu.Net
{
    /// <summary>
    /// Kinect制御アプリケーションの接続方法インターフェースKinectClientの実装です.
    /// NUInsatsuを起動しているマシンに直接キネクトを接続している場合に利用します.
    /// </summary>
    class KinectClientLocal : KinectClient
    {
        private bool isCanceled = false;

        /// <summary>
        /// キネクトと言うまで待機するイベント
        /// </summary>
        private readonly AutoResetEvent saidKinectEvent = new AutoResetEvent(false);

        bool KinectClient.Connect()
        {
            isCanceled = false;
            return true;
        }
        
        bool KinectClient.Close()
        {
            // キャンセルフラグをtrueに。
            isCanceled = true;

            // キネクトと発音するのを待機中なのを先に進める
            saidKinectEvent.Set();

            return true;
        }

        List<SkeletonTimeline> KinectClient.GetMotionList()
        {
            VoiceNavigation sound = VoiceNavigation.GetInstance();

            //「Kinect」といわれるまで待機
            sound.PlaySoundSync("WAIT_MOTION");

            // キネクトと発音されるまで待機します
            WaitSaidKinect();

            // もしキャンセルされたらダミーデータを返す
            if (isCanceled)
            {
                KinectClientStub stub = new KinectClientStub();
                return stub.MakeMotionList();
            }

            sound.PlaySoundSync("START_MOTION");

            Config config = Config.Load();

            //カウントダウン開始
            sound.CountDown(config.Countdown);
            //カウントダウン終了まで待機
            System.Threading.Thread.Sleep(config.Countdown * 1000);

            // モーションを取得し、XML文字列を生成します。
            String xml = MakeMotionXML(config.MotionTime);

            // XMLをパースし、構造化します。
            MotionResponseParser parser = new MotionResponseParser();
            List<SkeletonTimeline> list = parser.Parse(xml);

            return list;
        }

        FileInfo KinectClient.GetFaceImage()
        {
            throw new NotImplementedException("Not still implement KinectClientLocal.GetFaceImage");
        }

        void KinectClient.SendNavigation(String str)
        {
            throw new NotImplementedException("Not still implement KinectClientLocal.SendNavigation");
        }

        void KinectClient.SendTest()
        {
            throw new NotImplementedException("Not still implement KinectClientLocal.SendTest");
        }

        /// <summary>
        /// キネクトと発音されるまで待機します。
        /// </summary>
        private void WaitSaidKinect()
        {
            System.Console.WriteLine("[KinectClientLocal]wait voice recognized kinect");

            IVoiceRecognizer recognizer = KinectInstanceManager.GetVoiceRecognizerInstance();

            // キネクトと呼ばれると、waitoneから先に進めるようになるイベントをセット
            EventHandler<SaidWordArgs> voice_RecognizedHandler = new EventHandler<SaidWordArgs>(voice_Recognized);
            recognizer.Recognized += voice_RecognizedHandler;
            // キネクトと発音されるまで待機
            saidKinectEvent.WaitOne();
            // イベントを削除
            recognizer.Recognized -= voice_RecognizedHandler;
        }

        /// <summary>
        /// ボイスがRecognizedされた場合に呼び出されます.
        /// </summary>
        /// <remarks>
        /// 「キネクト」と発音すると、<see cref="WaitSaidKinect"/> のWaitOneされている部分から先に進みます。
        /// </remarks>
        /// <param name="sender">送信元オブジェクト</param>
        /// <param name="e">イベント情報</param>
        private void voice_Recognized(object sender, SaidWordArgs e)
        {   
            VoiceDictionary voicedic = new VoiceDictionary();
            String rectext = "";
            rectext = voicedic.RecognizeToCommand(e.Text);

            if (rectext == "kinect")
            {
                // 発音がキネクトだった場合、WaitOneから先に進むようイベントをセットします.
                saidKinectEvent.Set();
            }
        }

        /// <summary>
        /// キーとして利用してよいモーションかどうか検証します。
        /// </summary>
        /// <remarks>
        /// 利用してはいけないモーションとは、
        /// 識別人数が０、もしくは識別人数が途中で増減した場合です
        /// </remarks>
        /// <param name="motions">識別するモーション</param>
        /// <returns>キーとして利用してよい場合はtrue</returns>
        private bool IsValidMotionList(MotionList motions)
        {
            if (motions == null) return false;

            // 最初に認識している人数を調べる。0人だったらロストフレーム例外
            SkeletonDataList skeletonDataList = motions[0];
            int firstHeadCount = skeletonDataList.Count;
            if (firstHeadCount == 0) return false;

            // 最初に認識している人数と違い増減があったらロストフレーム例外
            foreach (SkeletonDataList sDataList in motions)
            {
                if (firstHeadCount != sDataList.Count) return false;
            }

            return true;
        }

        /// <summary>
        /// NMXPのMotionRequestに対するResponseのメッセージを生成します
        /// </summary>
        /// <param name="motiontime">モーション取得時間</param>
        /// <returns>生成されたメッセージ</returns>
        private String MakeMotionXML(int motiontime)
        {
            String sendMessage = null;

            // スケルトンを取得するためにインスタンスを生成します.
            using (ISkeletonSensor skeletonSensor = KinectInstanceManager.CreateSkeletonSensorInstance() )
            {
                // 指定した秒間のモーションを取得します
                MotionList motions = skeletonSensor.GetMotionForSeconds(motiontime);

                // モーションを利用できるかどうか検査します。
                if (IsValidMotionList(motions))
                {
                    Motion.KeyMaker keyMaker = new Motion.KeyMaker();
                    // モーションをXMLに変換します。
                    String str = keyMaker.Make(motions);
                    sendMessage = str;
                }
                else
                {
                    // 利用できないモーションデータの場合、NMXPのエラーを返します.
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
