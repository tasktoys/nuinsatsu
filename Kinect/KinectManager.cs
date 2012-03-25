using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Kinect
{
    class KinectManager
    {
        private static Runtime nui = null;

        public static Runtime GetKinect()
        {
            if( nui == null)
            {
                nui = Runtime.Kinects[0];

                try
                {
                    nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
                }
                catch (InvalidOperationException)
                {
                    System.Windows.MessageBox.Show("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                    return null;
                }
                try
                {
                    nui.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
				    nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
			    }
                catch (InvalidOperationException)
                {
                    System.Windows.MessageBox.Show("Failed to open stream. Please make sure to specify a supported image type and resolution.");
                    return null;
                }
            }

            return nui;
        }

        public static void Uninitialize()
        {
            nui.Uninitialize();
        }
    }
}
