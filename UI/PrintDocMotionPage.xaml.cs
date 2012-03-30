using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Research.Kinect.Nui;
using System.Threading;
using NUInsatsu.Net;
using NUInsatsu.Kinect;
using NUInsatsu.Motion;
using NUInsatsu.Document;

namespace NUInsatsu.UI
{
    /// <summary>
    /// 印刷時のモーション取得を行うページです。
    /// </summary>
    public partial class PrintDocMotionPage : Page
    {
        EventHandler<SkeletonFrameReadyEventArgs> skeletonFrameReadyHandler;
        NUInsatsu.Kinect.ISkeletonSensor skeletonSensor;

        public PrintDocMotionPage()
        {
            InitializeComponent();

            skeletonSensor = KinectInstanceManager.CreateSkeletonSensorInstance();
            skeletonFrameReadyHandler = new EventHandler<SkeletonFrameReadyEventArgs>(camera_SkeletonFrameReady);
            skeletonSensor.SkeletonFrameReady += skeletonFrameReadyHandler;

        }

        void camera_SkeletonFrameReady(object sender, Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            skeletonCanvas.DrawSkeletonFrame(e.SkeletonFrame);
        }

        /// <summary>
        /// メニュー画面に遷移します.
        /// </summary>
        private void transMenuPage()
        {
            // NavigationServiceと別スレッドでも画面遷移が行えるように、アクションを生成します.
            Action transAct = ()=>
                {
                       free();
                       NavigationService.Navigate(new MenuPage());
                };

            // アクションを起動させます
            Dispatcher.Invoke(transAct);
        }

        /// <summary>
        /// 顔認証を行う場合の印刷ページに移動します。
        /// </summary>
        private void transPrintFacePassPage()
        {
            Action transAct = () =>
                {
                    free();
                    NavigationService.Navigate(new PrintFacePassPage());
                };

            Dispatcher.Invoke(transAct);

        }

        /// <summary>
        /// 画面遷移によりこの場面から離れる場合、必ず呼び出してください.
        /// </summary>
        private void free()
        {
            skeletonSensor.Dispose();
        }

        /// <summary>
        /// このページがロードされたとき、印刷にトライします.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tryPrint();
        }

        /// <summary>
        /// 印刷処理を起動します。
        /// </summary>
        private void tryPrint()
        {
            Thread thread = new Thread(print);
            thread.Start();
        }

        /// <summary>
        /// 印刷を行います。
        /// </summary>
        private void print()
        {
            try
            {
                KinectClient client = KinectClientUtility.CreateKinectClientUtility();
                List<SkeletonTimeline> list = KinectClientUtility.GetMotionList(client);

                Key docKeyByMotion = KinectClientUtility.GetKey(list);

                DocumentManager manager = DocumentManager.GetInstance();
                Key docKey = manager.GetNearestDocumentKey(docKeyByMotion);

                if (manager.IsPassRequired(docKey))
                {
                    transPrintFacePassPage();
                }
                else
                {
                    transMenuPage();
                }
            }
            catch (DocumentNotFoundException)
            {
                showRetryDialog("ドキュメントが見つかりませんでした。");
            }
            catch (Exception e)
            {
                Console.WriteLine("[PrintDocMotionPage]予期しない例外が発生しました。 {0}", e.Message);
                String errorMessage = String.Format("予期しない例外が発生しました。\n ErrorMessage: {0}\n StackTrace:{1}", e.Message, e.StackTrace);
                MessageBox.Show(errorMessage);
            }
            
        }

        /// <summary>
        /// エラーが起こり、リトライを尋ねるダイアログを表示します。
        /// </summary>
        /// <param name="message">表示するメッセージ</param>
        private void showRetryDialog(String message)
        {
            MessageBoxResult result = MessageBox.Show(message, "失敗", MessageBoxButton.YesNo, MessageBoxImage.Information);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    // 印刷処理にトライする
                    tryPrint();
                    break;

                case MessageBoxResult.No:
                    // ホームに戻る
                    transMenuPage();
                    break;
            }
        }
    }
}
