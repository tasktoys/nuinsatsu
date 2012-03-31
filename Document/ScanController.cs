using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using WIA;
using System.IO;

namespace NUInsatsu.Document
{
    /// <summary>
    /// スキャンを行うクラスです。
    /// </summary>
    class ScanController
    {
        public const String ScanTmpFileName = @".\scan_tmp.bmp";

        /// <summary>
        /// スキャンを行います。
        /// </summary>
        /// <exception cref="COMException">スキャナーが繋がっていない場合</exception>
        public FileInfo Scan()
        {

            WIA.CommonDialog dlg = new WIA.CommonDialog();
            ImageFile Image = dlg.ShowAcquireImage(WiaDeviceType.ScannerDeviceType,
                         WiaImageIntent.ColorIntent, WiaImageBias.MaximizeQuality,
                         "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}", true, true, true);
            string p = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            System.IO.File.Delete(p + ScanTmpFileName);
            Image.SaveFile(p + ScanTmpFileName);

            return new FileInfo(ScanTmpFileName);
        }
    }
}
