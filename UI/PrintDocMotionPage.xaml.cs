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

        private void kinectButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PrintFacePassPage());
            free();
        }

        private void free()
        {
            skeletonSensor.Dispose();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tryPrint();
        }

        private void tryPrint()
        {
            Thread thread = new Thread(print);
            thread.Start();
        }

        private void print()
        {
            try
            {
                KinectClient client = KinectClientUtility.CreateKinectClientUtility();
                List<SkeletonTimeline> list = KinectClientUtility.GetMotionList(client);

                Key docKeyByMotion = KinectClientUtility.GetKey(list);

                DocumentManager manager = DocumentManager.GetInstance();
                Key docKey = manager.GetNearestDocumentKey(docKeyByMotion);
            }
            catch (DocumentNotFoundException)
            {
                MessageBox.Show("ドキュメントが見つかりませんでした。");
            }
            catch (Exception e)
            {
                Console.WriteLine("[PrintDocMotionPage]予期しない例外が発生しました。 {0}", e.Message);
                String errorMessage = String.Format("予期しない例外が発生しました。\n ErrorMessage: {0}\n StackTrace:{1}", e.Message, e.StackTrace);
                MessageBox.Show(errorMessage);
            }
            
        }
    }
}
