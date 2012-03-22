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
    /// 印刷時の顔認証を行うページです。
    /// </summary>
    public partial class PrintFacePassPage : Page
    {
        public PrintFacePassPage()
        {
            InitializeComponent();
        }


        private void kinectButton_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Printing.PrintDocument pd =
                new System.Drawing.Printing.PrintDocument();
            Config config = Config.Load();
            pd.PrinterSettings.PrinterName = config.PrinterName;

            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
            pd.Print();

            MessageBox.Show("印刷中です", "確認", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.Navigate(new MenuPage());
        }

        /// <summary>
        /// 印刷を行います。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile("hikaru.jpg");
            //画像を描画する
            e.Graphics.DrawImage(img, e.MarginBounds);
            //次のページがないことを通知する
            e.HasMorePages = false;
            //後始末をする
            img.Dispose();
        }
    }
}
