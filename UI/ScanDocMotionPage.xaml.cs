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

namespace NUInsatsu.UI
{
    /// <summary>
    /// 登録時のモーションを取得するページです。
    /// </summary>
    public partial class ScanDocMotionPage : Page
    {
        ISkeletonSensor camera;

        public ScanDocMotionPage()
        {
            InitializeComponent();
            camera = KinectInstanceManager.CreateSkeletonSensorInstance();
            camera.SkeletonFrameReady += new EventHandler<Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs>(camera_SkeletonFrameReady);
        }

        void camera_SkeletonFrameReady(object sender, Microsoft.Research.Kinect.Nui.SkeletonFrameReadyEventArgs e)
        {
            skeletonCanvas.DrawSkeletonFrame(e.SkeletonFrame);
            
        }

        private void kinectButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show("パスワードを入力しますか？", "確認", button, MessageBoxImage.Asterisk);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    NavigationService.Navigate(new ScanFacePassPage());
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("登録が完了しました", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new MenuPage());
                    break;
            }


            camera.Dispose();
            
        }
    }
}
