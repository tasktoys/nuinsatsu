using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using System.Threading;

namespace NUInsatsu.Kinect
{
    interface ISkeletonSensor : IDisposable
    {
        event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;

        /// <summary>
        /// 指定時間モーションを取得する
        /// </summary>
        /// <param name="time">時間(秒)</param>
        /// <returns>モーション</returns>
        MotionList GetMotionForSeconds(int time);
    }
}
