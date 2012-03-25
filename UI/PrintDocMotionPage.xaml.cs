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
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.UI
{
    /// <summary>
    /// 印刷時のモーション取得を行うページです。
    /// </summary>
    public partial class PrintDocMotionPage : Page
    {
        EventHandler<SkeletonFrameReadyEventArgs> skeletonFrameReadyHandler;
        NUInsatsu.Kinect.SkeletonSensor skeletonSensor;

        public PrintDocMotionPage()
        {
            InitializeComponent();

            skeletonSensor = NUInsatsu.Kinect.SkeletonSensor.GetInstance();
            skeletonFrameReadyHandler = new EventHandler<SkeletonFrameReadyEventArgs>(camera_SkeletonFrameReady);
            skeletonSensor.SkeletonFrameReady += skeletonFrameReadyHandler;
        }

        void camera_SkeletonFrameReady(object sender, Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            skeletonCanvas.DrawSkeletonFrame(e.SkeletonFrame);
            System.Console.WriteLine("[PrintDocMotionPage]Hoge");
        }

        private void kinectButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PrintFacePassPage());
            free();
        }

        void free()
        {
            skeletonSensor.Dispose();
        }
    }
}
