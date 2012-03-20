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
