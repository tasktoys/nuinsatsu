using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Kinect
{
    class KinectInstanceManager
    {
        private static Runtime nui = null;
        private static IVoiceRecognizer voiceRecognizerInstance = null;

        public static Runtime GetKinectInstance()
        {
            if( nui == null)
            {
                if (Runtime.Kinects.Count == 0)
                {
                    System.Windows.MessageBox.Show("キネクトが接続されていません。");
                    return null;
                }
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

        /// <summary>
        /// スケルトンを利用するためのインスタンスを生成します.
        /// キネクトが接続されていない場合、スタブインスタンスを生成します.
        /// </summary>
        /// <returns>生成されたインスタンス</returns>
        public static ISkeletonSensor CreateSkeletonSensorInstance()
        {
            if (nui == null)
            {
                Console.WriteLine("[KinectInstanceManager]create stub");
                return new SkeletonSensorStub();
            }
            else
            {
                return new SkeletonSensorImpl();
            }
        }

        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        /// <returns>インスタンス</returns>
        public static IVoiceRecognizer GetVoiceRecognizerInstance()
        {
            if (nui == null)
            {
                if (voiceRecognizerInstance == null)
                {
                    voiceRecognizerInstance = new VoiceRecognizerStub();

                }
                return voiceRecognizerInstance;
            }
            else
            {
                if (voiceRecognizerInstance == null)
                {
                    voiceRecognizerInstance = new VoiceRecognizer();
                    VoiceDictionary dict = new VoiceDictionary();
                    dict.AddDic(voiceRecognizerInstance);
                }

                return voiceRecognizerInstance;
            }
        }

        public static void UninitializeKinect()
        {
            nui.Uninitialize();
        }
    }
}
