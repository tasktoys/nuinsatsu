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
    /// 設定ページです。
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            //データバインド
            installedPrinters.DataContext = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            //設定を反映
            Config config = Config.Load();
            config.PrinterName = (String)installedPrinters.SelectedItem;
            config.Save();
            //メニューに戻る
            NavigationService.GoBack();
        }
    }
}
