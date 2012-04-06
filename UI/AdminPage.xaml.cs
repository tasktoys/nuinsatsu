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
            // データバインド
            installedPrinters.DataContext = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // 設定を反映
            Config config = Config.Load();
            config.PrinterName = (String)installedPrinters.SelectedItem;
            config.Save();
            // メニューに戻る
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Config config = Config.Load();

            SelectComboBoxItemString(installedPrinters, config.PrinterName);
            SelectComboBoxItem(kinectConnectionType,config.KinectType);
        }

        /// <summary>
        /// コンボボックスの指定のアイテムを選択状態にします。
        /// </summary>
        /// <param name="combo">選択を行うコンボボックス</param>
        /// <param name="targetContent">選択する名前</param>
        private void SelectComboBoxItem(ComboBox combo, String targetContent)
        {
            foreach (ComboBoxItem item in combo.Items)
            {
                if( item.Name == targetContent)
                {
                    item.IsSelected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// コンボボックスの指定のアイテムを選択状態にします。
        /// </summary>
        /// <param name="combo">選択を行うコンボボックス</param>
        /// <param name="targetContent">選択する名前</param>
        private void SelectComboBoxItemString(ComboBox combo, String targetContent)
        {
            // 設定済みの値を選択
            for (int i = 0; i < combo.Items.Count; ++i)
            {
                String content = (String)combo.Items[i];
                if (content == targetContent)
                {
                    combo.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
