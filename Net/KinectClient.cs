using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;
using System.IO;

namespace NUInsatsu.Net
{
    /// <summary>
    /// Kinect制御アプリケーションに接続するためのクラス構造を定めたアプリケーションです
    /// </summary>
    interface KinectClient
    {
        /// <summary>
        /// Kinect 制御アプリケーションに接続します.
        /// </summary>
        /// <returns>成功した場合true,失敗した場合false</returns>
        bool Connect();

        /// <summary>
        /// Kinect 制御アプリケーションから切断します.
        /// </summary>
        /// <returns>成功した場合true,失敗した場合false</returns>
        bool Close();

        /// <summary>
        /// Kinectへの入力を開始し、座標データを取得します.
        /// </summary>
        /// <returns>座標データ</returns>
        List<SkeletonTimeline> GetMotionList();

        /// <summary>
        /// 顔認証用の画像を取得します。
        /// </summary>
        /// <returns>顔画像のテンポラリファイル</returns>
        FileInfo GetFaceImage();

        /// <summary>
        /// テストメッセージを送信します。
        /// </summary>
        void SendTest();

        /// <summary>
        /// ナビゲーションメッセージを送信します.
        /// </summary>
        /// <param name="navigation"></param>
        void SendNavigation(String navigation);
    }
}
