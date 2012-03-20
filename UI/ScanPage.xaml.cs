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
    /// スキャンを行うページです。
    /// </summary>
    public partial class ScanPage : Page
    {
        public ScanPage()
        {
            InitializeComponent();
        }

        private void scanButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxImage image = MessageBoxImage.Information;
            MessageBox.Show("スキャン中です", "Please wait", MessageBoxButton.OK, image);
            NavigationService.Navigate(new ScanDocMotionPage());
        }
    }
}
