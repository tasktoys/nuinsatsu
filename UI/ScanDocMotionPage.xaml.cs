﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NUInsatsu.Kinect;
using System.Threading;
using NUInsatsu.Net;
using NUInsatsu.Motion;
using NUInsatsu.Document;
using System.IO;
using System.Runtime.InteropServices;
using NUInsatsu.Navigate;

namespace NUInsatsu.UI
{
    /// <summary>
    /// 登録時のモーションを取得するページです。
    /// </summary>
    public partial class ScanDocMotionPage : Page
    {
        ISkeletonSensor skeletonSensor;
        KinectClient client = null;
        bool isFree = false;

        public ScanDocMotionPage()
        {
            InitializeComponent();
            skeletonSensor = KinectInstanceManager.CreateSkeletonSensorInstance();
            skeletonSensor.SkeletonFrameReady += new EventHandler<Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs>(camera_SkeletonFrameReady);
        }

        void camera_SkeletonFrameReady(object sender, Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            skeletonCanvas.DrawSkeletonFrame(e.SkeletonFrame);
        }

        private void TransScanPage()
        {
            Action act = () =>
            {
                Free();
                NavigationService.Navigate(new ScanPage());
            };
            Dispatcher.Invoke(act);
        }

        /// <summary>
        /// メニューに画面遷移します。
        /// </summary>
        private void TransMenuPage()
        {
            Action act = () =>
            {
                Free();
                NavigationService.Navigate(new MenuPage());
            };
            Dispatcher.Invoke(act);
        }

        /// <summary>
        /// このページが持つリソースを解放します。
        /// </summary>
        private void Free()
        {
            skeletonSensor.Dispose();
            client.Close();
            isFree = true;
        }


        /// <summary>
        /// ドキュメントの登録にトライします。
        /// </summary>
        private void TryEntry()
        {
            Thread thread = new Thread(Entry);
            thread.Start();
        }

        /// <summary>
        /// ドキュメントの登録を行います。
        /// </summary>
        private void Entry()
        {
            try
            {
                client = KinectClientUtility.CreateKinectClientInstance();
                List<SkeletonTimeline> list = client.GetMotionList();

                // キネクトと発音される前に
                if (isFree)
                {
                    return;
                }

                NUInsatsu.Motion.Key docKeyByMotion = KinectClientUtility.GetKey(list);

                DocumentFileIO io = DocumentManager.GetInstance().GetIOInstance();
                try
                {
                    io.GetNearestDocument(docKeyByMotion);
                    ShowRetryDialog("モーションが重複しています。\nモーションを登録しなおしますか？");
                    return;
                }
                catch (DocumentNotFoundException)
                {
                    // ドキュメントが見つからない場合・・・登録できる
                }

                io.Put(docKeyByMotion, SharedData.ScanImageFile);

                MessageBox.Show("登録が完了しました。");
                TransMenuPage();
            }
            catch (NotInstalledSpeechLibraryException)
            {
                MessageBox.Show("音声エンジン（Speech Platform Runtime）か、音声データ「はるか」がインストールされていません。");
                TransMenuPage();
            }
            catch (NMXPErrorMessageException)
            {
                ShowRetryDialog("フレームの認識数が増減しました。\nリトライしますか？");
            }
            catch (COMException)
            {
                Console.WriteLine("[ScanDocMotionPage]スキャナーが接続されていません。");
            }
        }

        /// <summary>
        /// エラーが起こり、リトライを尋ねるダイアログを表示します。
        /// </summary>
        /// <param name="message">表示するメッセージテキスト</param>
        private void ShowRetryDialog(String message)
        {
            MessageBoxResult result = MessageBox.Show(message, "失敗", MessageBoxButton.YesNo, MessageBoxImage.Information);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    // 印刷処理にトライする
                    TryEntry();
                    break;

                case MessageBoxResult.No:
                    // ホームに戻る
                    TransMenuPage();
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TryEntry();
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            TransMenuPage();
        }
    }
}
