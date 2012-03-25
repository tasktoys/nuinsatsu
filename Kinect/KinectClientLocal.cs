using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Kinect
{
    /// <summary>
    /// Kinect制御アプリケーションの接続方法インターフェースKinectClientの実装です.
    /// NUInsatsuを起動しているマシンに直接キネクトを接続している場合です.
    /// </summary>
    class KinectClientLocal : KinectClient
    {
        public bool Connect()
        {
            return true;
        }

        public bool Close()
        {
            return true;
        }

        public List<List<Dictionary<String, Double>>> GetMotionList()
        {
        }

        public void SendNavigation(String str)
        {
        }
    }
}
