using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using NUInsatsu.Document;
using System.Runtime.InteropServices;
using NUInsatsu.Navigate;
using NUInsatsu.Kinect;

namespace NUInsatsu.UI
{
    /// <summary>
    /// スキャンを行うページです。
    /// </summary>
    public partial class ScanPage : Page
    {
        private readonly IVoiceRecognizer voiceRecognizer;

        public ScanPage()
        {
            InitializeComponent();

            voiceRecognizer = KinectInstanceManager.GetVoiceRecognizerInstance();
            voiceRecognizer.Recognized += new EventHandler<SaidWordArgs>(voiceRecognizer_Recognized);

            VoiceNavigation navigation = new VoiceNavigation();
            navigation.PlaySoundASync("SCAN_START");
        }

        void voiceRecognizer_Recognized(object sender, SaidWordArgs e)
        {
            if (e.Text == "スキャン")
            {
                DoScan();
            }
            else if (e.Text == "もどる")
            {
                TransMenuPage();
            }
        }

        /// <summary>
        /// リソースの解放を行います。
        /// </summary>
        private void Free()
        {
            voiceRecognizer.Recognized -= new EventHandler<SaidWordArgs>(voiceRecognizer_Recognized);
        }

        /// <summary>
        /// ScanDocMotionPageに遷移します。
        /// </summary>
        private void TransScanDocMotionPage()
        {
            Free();
            NavigationService.Navigate(new ScanDocMotionPage());
        }

        /// <summary>
        /// メニューに遷移します。
        /// </summary>
        private void TransMenuPage()
        {
            Free();
            NavigationService.Navigate(new MenuPage());
        }

        private void scanButton_Click(object sender, RoutedEventArgs e)
        {
            DoScan();
        }

        private void DoScan()
        {
            DocumentManager manager = DocumentManager.GetInstance();
            try
            {
                SharedData.ScanImageFile = manager.Scan();
                TransScanDocMotionPage();
            }
            catch (COMException)
            {
                Console.Error.WriteLine("[ScanPage]スキャナーが接続されていません。");
                MessageBox.Show("スキャナーが接続されていません。");
            }
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            TransMenuPage();
        }
    }
}
