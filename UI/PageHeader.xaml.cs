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
    /// NUInsatsuのUIに利用する共通のヘッダーです。
    /// </summary>
    public partial class PageHeader : UserControl
    {
        public PageHeader()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ヘッダーのテキストを指定します。
        /// </summary>
        public String Title
        {
            get
            {
                return (String)titleLabel.Content;
            }
            set
            {
                titleLabel.Content = value;
            }
        }
    }
}
