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
using NUInsatsu.Document;
using System.Runtime.InteropServices;
using System.IO;

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
            //MessageBoxImage image = MessageBoxImage.Information;
            //MessageBox.Show("スキャン中です", "Please wait", MessageBoxButton.OK, image);

            DocumentManager manager = DocumentManager.GetInstance();
            try
            {
                SharedData.ScanImageFile = manager.Scan();
                NavigationService.Navigate(new ScanDocMotionPage());
            }
            catch (COMException)
            {
                Console.Error.WriteLine("[ScanPage]スキャナーが接続されていません。");
                MessageBox.Show("スキャナーが接続されていません。");
            }
        }
    }
}
