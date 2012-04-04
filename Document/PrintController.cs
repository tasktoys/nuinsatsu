using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NUInsatsu.Document
{
    /// <summary>
    /// 印刷処理を制御するクラスです。
    /// </summary>
    class PrintController
    {
        FileInfo fileInfo;

        /// <summary>
        /// 指定したファイルの印刷処理を行います。
        /// </summary>
        /// <param name="fileInfo">印刷する対象のファイル情報</param>
        /// <returns>印刷が成功したらtrue,失敗したらfalse</returns>
        public bool Print(FileInfo fileInfo)
        {
            try
            {
                this.fileInfo = fileInfo;

                System.Drawing.Printing.PrintDocument pd =
                    new System.Drawing.Printing.PrintDocument();
                Config config = Config.Load();
                pd.PrinterSettings.PrinterName = config.PrinterName;

                pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
                pd.Print();
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
            catch (System.Drawing.Printing.InvalidPrinterException e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }

            return true;
        }

        /// <summary>
        /// 印刷を行います。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(fileInfo.FullName);
            //画像を描画する
            e.Graphics.DrawImage(img, e.MarginBounds);
            //次のページがないことを通知する
            e.HasMorePages = false;
            //後始末をする
            img.Dispose();
        }
    }


}
