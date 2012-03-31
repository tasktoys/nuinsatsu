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
    /// メニューを表示する画面です。
    /// </summary>
    public partial class MenuPage : Page
    {
        private IVoiceRecognizer voiceRecognizer;

        public MenuPage()
        {
            InitializeComponent();

            voiceRecognizer = KinectInstanceManager.GetVoiceRecognizerInstance();

            voiceRecognizer.Recognized += new EventHandler<SaidWordArgs>(voiceRecognizer_Recognized);
        }

        void voiceRecognizer_Recognized(object sender, SaidWordArgs e)
        {
            if (e.Text == "SCAN")
            {
                TransScanPage();
            }
            else if(e.Text == "PRINT")
            {
                TransPrintDocMotionPage();
            }
        }

        /// <summary>
        /// スキャンページに遷移します。
        /// </summary>
        private void TransScanPage()
        {
            Free();
            NavigationService.Navigate(new ScanPage());
        }

        /// <summary>
        /// 印刷時にモーションを取得するページに遷移します。
        /// </summary>
        private void TransPrintDocMotionPage()
        {
            Free();
            NavigationService.Navigate(new PrintDocMotionPage());
        }

        /// <summary>
        /// このページのリソースを解放します。
        /// </summary>
        private void Free()
        {
            // 音声操作に関するイベントを削除します。
            voiceRecognizer.Recognized -= new EventHandler<SaidWordArgs>(voiceRecognizer_Recognized);
        }

        /// <summary>
        /// ドキュメントの登録画面に遷移します。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event</param>
        private void entryButton_Click(object sender, RoutedEventArgs e)
        {
            TransScanPage();
        }

        /// <summary>
        /// ドキュメントの印刷画面に遷移します。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event</param>
        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            TransPrintDocMotionPage();
        }

        private void adminButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage());
        }
    }
}
