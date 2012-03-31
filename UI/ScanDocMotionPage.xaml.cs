using System;
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

namespace NUInsatsu.UI
{
    /// <summary>
    /// 登録時のモーションを取得するページです。
    /// </summary>
    public partial class ScanDocMotionPage : Page
    {
        ISkeletonSensor skeletonSensor;

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
                KinectClient client = KinectClientUtility.CreateKinectClientInstance();
                List<SkeletonTimeline> list = client.GetMotionList();
                NUInsatsu.Motion.Key docKeyByMotion = KinectClientUtility.GetKey(list);

                DocumentManager manager = DocumentManager.GetInstance();

                LocalFileIO io = new LocalFileIO();
                io.Put(docKeyByMotion, SharedData.ScanImageFile );

                MessageBox.Show("登録が完了しました。");
                TransMenuPage();
            }
            catch (COMException)
            {
                Console.WriteLine("[ScanDocMotionPage]スキャナーが接続されていません。");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TryEntry();
        }
    }
}
