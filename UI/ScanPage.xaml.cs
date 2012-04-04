using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using NUInsatsu.Document;
using System.Runtime.InteropServices;
using NUInsatsu.Navigate;

namespace NUInsatsu.UI
{
    /// <summary>
    /// スキャンを行うページです。
    /// </summary>
    public partial class ScanPage : Page
    {

        public ScanPage()
        {
            InitializeComponent();

            VoiceNavigation navigation = new VoiceNavigation();
            navigation.PlaySoundASync("SCAN_START");
        }

        /// <summary>
        /// リソースの解放を行います。
        /// </summary>
        private void Free()
        {
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
