using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Kinect
{
    class KinectClientUtility
    {
        public static KinectClient CreateKinectClientUtility()
        {
            KinectClient client;

            client = new KinectClientLocal();

            return client;
        }
    }
}
