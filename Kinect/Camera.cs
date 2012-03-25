using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Kinect
{


    /// <summary>
    /// Kinectのカメラに関する機能を提供します。
    /// </summary>
    class Camera
    {
        static Camera instance = null;

        Runtime nui;

        private PlanarImage cameraimage;
        private static BitmapSource picture;

        /// <summary>
        /// Cameraインスタンスを取得します。
        /// </summary>
        /// <returns></returns>
        public static Camera GetInstance()
        {
            if ( instance == null ) instance = new Camera();
            return instance;
        }

        /// <summary>
        /// 現在カメラが認識しているイメージを取得します
        /// </summary>
        public BitmapSource Picture
        {
            get { return picture; }
        }

        public PlanarImage CameraImage
        {
            get { return cameraimage; }
        }

        /// <summary>
        /// Cameraクラスを構築します
        /// </summary>
        private Camera()
        {
            nui = NUInsatsu.Kinect.KinectManager.GetKinect();

			nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_ColorFrameReady);
        }

		/// <summary>
		/// カメラが準備完了になったときに呼び出されます
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void nui_ColorFrameReady(object sender, ImageFrameReadyEventArgs e)
		{
			//PlanarImage KImage;

			// 32-bit per pixel, RGBA image
			cameraimage = e.ImageFrame.Image;

            picture = BitmapSource.Create(
            cameraimage.Width, cameraimage.Height, 96, 96, PixelFormats.Bgr32, null, cameraimage.Bits, cameraimage.Width * cameraimage.BytesPerPixel);
		}

		/// <summary>
		/// カメラの映像を画像として保存します
		/// </summary>
		public string capture()
		{
            BitmapFrame bmpFrame = BitmapFrame.Create(picture);

			FileStream stream = new FileStream(@"capture.jpeg", FileMode.Create);

			JpegBitmapEncoder jpenc = new JpegBitmapEncoder();

			jpenc.Frames.Add(bmpFrame);
			jpenc.Save(stream);

			stream.Close();

			FileStream fstr = File.Open("capture.jpeg", FileMode.Open, FileAccess.Read);
			byte[] bytes = new byte[fstr.Length];
			string str;
			fstr.Read(bytes, 0, bytes.Length);
			str = System.Convert.ToBase64String(bytes);
			fstr.Close();

			return str;
		}
    }
}
