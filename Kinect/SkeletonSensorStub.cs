using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Kinect
{
    class SkeletonSensorStub : ISkeletonSensor
    {
        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;

        public MotionList GetMotionForSeconds(int time)
        {
            return null;
        }

        public void Dispose() { }
    }
}
