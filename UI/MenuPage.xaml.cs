using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using NUInsatsu.Kinect;
using NUInsatsu.Navigate;

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

            VoiceNavigation navigation = new VoiceNavigation();
            navigation.PlaySoundASync("ENTRY_OR_PRINT");
        }

        void voiceRecognizer_Recognized(object sender, SaidWordArgs e)
        {
            if (e.Text == "とうろく")
            {
                TransScanPage();
            }
            else if(e.Text == "いんさつ")
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
        /// 管理ページに遷移します。
        /// </summary>
        private void TransAdminPage()
        {
            Free();
            NavigationService.Navigate(new AdminPage());
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
        /// ボタンを押したら呼び出され、ドキュメントの登録画面に遷移します。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event</param>
        private void entryButton_Click(object sender, RoutedEventArgs e)
        {
            TransScanPage();
        }

        /// <summary>
        /// ボタンを押したら呼び出され、ドキュメントの印刷画面に遷移します。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event</param>
        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            TransPrintDocMotionPage();
        }

        /// <summary>
        /// ボタンを押したら呼び出され、管理画面に遷移します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adminButton_Click(object sender, RoutedEventArgs e)
        {
            TransAdminPage();
        }
    }
}
