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
    /// 表示するWindowです。これ自体は何も描画しません。
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowsNavigationUI = false;

            KinectInstanceManager.GetKinectInstance();
            IVoiceRecognizer recognizer = KinectInstanceManager.GetVoiceRecognizerInstance();

            recognizer.Start();
        }
    }
}
