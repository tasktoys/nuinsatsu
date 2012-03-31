using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NUInsatsu.UI
{
    /// <summary>
    /// ページで共有して利用するデータです。
    /// </summary>
    class SharedData
    {
        /// <summary>
        /// スキャンしたイメージのファイル情報を持ちます。
        /// </summary>
        public static FileInfo ScanImageFile { get; set; }
    }
}
