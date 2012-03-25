using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Kinect
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
        List<List<Dictionary<String, Double>>> GetMotionList();

        /// <summary>
        /// ナビゲーションメッセージを送信します.
        /// </summary>
        /// <param name="navigation"></param>
        void SendNavigation(String navigation);
    }
}
