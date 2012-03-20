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
    /// メニューを表示する画面です。
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ドキュメントの登録画面に遷移します。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event</param>
        private void entryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ScanPage());
        }

        /// <summary>
        /// ドキュメントの印刷画面に遷移します。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">routed event</param>
        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PrintDocMotionPage());
        }
    }
}
