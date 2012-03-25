using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu
{
    class SkeletonDataComparer : IComparer<SkeletonData>
    {
        public int Compare(SkeletonData lhs, SkeletonData rhs)
        {
            return (int)(lhs.Position.X - rhs.Position.X);
        }
    }

    /// <summary>
    /// Kinectのカメラに関する機能を提供します。
    /// </summary>
    class Camera
    {
        // We want to control how depth data gets converted into false-color data
        // for more intuitive visualization, so we keep 32-bit color frame buffer versions of
        // these, to be updated whenever we receive and process a 16-bit frame.
        const int RED_IDX = 2;
        const int GREEN_IDX = 1;
        const int BLUE_IDX = 0;
        byte[] depthFrame32 = new byte[320 * 240 * 4];
        static Camera instance = null;

        Runtime nui;

        private PlanarImage cameraimage;
        private static BitmapSource picture;
        //現在のスケルトンフレームを格納
        private static SkeletonFrame currentSkeletonFrame = null;

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
        /// 現在カメラが認識しているフレームを取得します
        /// </summary>
        public SkeletonFrame CurrentSkeletonFrame
        {
            get {return currentSkeletonFrame; }
            private set { currentSkeletonFrame = value; }
        }

        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;

        /// <summary>
        /// Cameraクラスを構築します
        /// </summary>
        private Camera()
        {
            nui = NUInsatsu.Kinect.KinectManager.GetKinect();

			nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nui_ColorFrameReady);
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
        }

        /// <summary>
        /// フレームが認識され取得できる状態になったときに呼び出されます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            //スケルトンフレームを保存
            CurrentSkeletonFrame = e.SkeletonFrame;

            if (SkeletonFrameReady == null){return; }
            SkeletonFrameReady(this, e);
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
        /// 指定時間モーションを取得する
        /// </summary>
        /// <param name="time">時間</param>
        /// <returns>モーション</returns>
        public MotionList GetMotionForSeconds(int time)
        {
            System.Console.WriteLine("[Camera]time:{0}!", time);

            Camera kinect = Camera.GetInstance();

            MotionList motionList = new MotionList();

            for (int i = 0; i < time; i++)
            {
                //現在のスケルトンを得る
                SkeletonDataList datas = Skeletons2List(kinect.CurrentSkeletonFrame.Skeletons);
                //左にいる人がコレクションの最初の方の要素になるように、x軸でソートする
                datas.Sort( new SkeletonDataComparer() );
                motionList.Add(datas);
                Thread.Sleep(100);
            }

            return motionList;
        }

        /// <summary>
        /// SkeletonDataの配列をSkeletonDataのリストに変換します。
        /// SkeletonDataの配列は要素数が６で固定ですが、
        /// SkeletonDataListは識別している人数分の要素数になります。
        /// </summary>
        /// <param name="datas">SkeletonDataの配列を</param>
        /// <returns></returns>
        private SkeletonDataList Skeletons2List(SkeletonData[] datas)
        {
            SkeletonDataList list = new SkeletonDataList();
            foreach (SkeletonData data in datas)
            {
                if (data.TrackingState == SkeletonTrackingState.Tracked) { list.Add(data); };
            }
            return list;
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
