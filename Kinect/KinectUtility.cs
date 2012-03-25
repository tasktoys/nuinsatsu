using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Kinect
{
    class KinectUtility
    {
        /// <summary>
        /// スケルトンを利用するためのインスタンスを生成します.
        /// キネクトが接続されていない場合、スタブインスタンスを生成します.
        /// </summary>
        /// <returns>生成されたインスタンス</returns>
        public static ISkeletonSensor CreateSkeletonSensorInstance()
        {
            return new SkeletonSensorImpl();
        }
    }
}
