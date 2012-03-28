using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;

namespace NUInsatsu.Net
{
    /// <summary>
    /// キネクトクライアントを簡単に利用するためのユーティリティです.
    /// </summary>
    class KinectClientUtility
    {
        /// <summary>
        /// 設定値から適切なKinectClientインターフェースの実装を選択し、そのインスタンスを生成します.
        /// </summary>
        /// <returns>KinectClientインターフェースを実装したクラスのインスタンス</returns>
        public static KinectClient CreateKinectClientUtility()
        {
            KinectClient client;

            client = new KinectClientLocal();

            return client;
        }

        /// <summary>
        /// サーバと接続し座標データを取得します.
        /// </summary>
        /// <param name="client">サーバと接続するためのクライアント</param>
        /// <returns>取得した座標データ</returns>
        public static List<SkeletonTimeline> GetMotionList(KinectClient client)
        {
            client.Connect();

            List<SkeletonTimeline> motionList = client.GetMotionList();
            if (motionList == null)
            {
                throw new ConnectFailedException();
            }

            client.Close();

            return motionList;
        }
    }
}
