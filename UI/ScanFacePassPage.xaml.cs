using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NUInsatsu.UI
{
    /// <summary>
    /// 登録時の顔認識を行うページです。
    /// </summary>
    public partial class ScanFacePassPage : Page
    {
        public ScanFacePassPage()
        {
            InitializeComponent();
        }

        private void kinectButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("登録が完了しました", "確認", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new MenuPage());
        }
    }
}
