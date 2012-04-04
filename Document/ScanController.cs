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
        // WIA.FormatIDを参照
        const String wiaFormatPNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
        const String wiaFormatJPG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
        const String scanTmpFileName = "\\scan_tmp.jpg";

        /// <summary>
        /// スキャンを行います。
        /// </summary>
        /// <returns>スキャンしたイメージファイルの情報</returns>
        public FileInfo Scan()
        {
            CommonDialog commonDialog = new CommonDialog();

            Device scannerDevice = commonDialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);

            if (scannerDevice != null)
            {
                Item scannerItem = scannerDevice.Items[1];

                AdjustScannerSettings(scannerItem, 300, 0, 0, 2500, 3500, 0, 0);
                //AdjustScannerSettings(scannerItem, 300, 0, 0, 1010, 620, 0, 0);

                object scanResult = commonDialog.ShowTransfer(scannerItem, wiaFormatJPG, false);
                if (scanResult != null)
                {
                    String p = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

                    String filePass = p + scanTmpFileName;
                    System.IO.File.Delete(filePass);

                    ImageFile image = (ImageFile)scanResult;

                    SaveImage(image, filePass, wiaFormatJPG);

                    return new FileInfo(filePass);
                }
            }

            return null;
        }

        /// <summary>
        /// スキャナーの設定を行います。
        /// </summary>
        /// <param name="scannnerItem">設定を行うスキャナーのインスタンス</param>
        /// <param name="scanResolutionDPI">DPI</param>
        /// <param name="scanStartLeftPixel">開始位置の左側のピクセル</param>
        /// <param name="scanStartTopPixel">開始位置の上側のピクセル</param>
        /// <param name="scanWidthPixels">スキャン横幅</param>
        /// <param name="scanHeightPixels">スキャン縦縦</param>
        /// <param name="brightnessPercents">明度</param>
        /// <param name="contrastPercents">コントラスト</param>
        private void AdjustScannerSettings(IItem scannnerItem, int scanResolutionDPI, int scanStartLeftPixel, int scanStartTopPixel,
    int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents)
        {
            const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
            const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
            const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
            const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
            const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
            const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
            const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
            const string WIA_SCAN_CONTRAST_PERCENTS = "6155";
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
            SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_SIZE_PIXELS, scanWidthPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_SIZE_PIXELS, scanHeightPixels);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
            SetWIAProperty(scannnerItem.Properties, WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
        }

        /// <summary>
        /// デバイスアイテムのプロパティの設定を行います
        /// </summary>
        /// <param name="properties">設定を行う対称のデバイスアイテムプロパティ</param>
        /// <param name="propName">設定対称のプロパティ文字列</param>
        /// <param name="propValue">設定値</param>
        private void SetWIAProperty(IProperties properties, object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);
            prop.set_Value(ref propValue);
        }

        /// <summary>
        /// イメージを保存します。
        /// </summary>
        /// <param name="image">保存するイメージ</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="imageFormatID">フォーマットID</param>
        private void SaveImage(ImageFile image, string fileName, String imageFormatID)
        {
            ImageProcess imgProcess = new ImageProcess();
            object convertFilter = "Convert";
            string convertFilterID = imgProcess.FilterInfos.get_Item(ref convertFilter).FilterID;
            imgProcess.Filters.Add(convertFilterID, 0);
            SetWIAProperty(imgProcess.Filters[imgProcess.Filters.Count].Properties, "FormatID", imageFormatID);
            image = imgProcess.Apply(image);
            image.SaveFile(fileName);
        }
    }
}
