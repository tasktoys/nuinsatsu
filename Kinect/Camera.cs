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
		
        Object pictureLock = new Object();

        Runtime nui;


        private PlanarImage cameraimage;
        private static BitmapSource picture;
        //現在のスケルトンフレームを格納
        private static SkeletonFrame currentSkeletonFrame = null;

        //読み書きのスレッド同期を行う
        private ReaderWriterLock rwLock = new ReaderWriterLock();

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
            nui = Runtime.Kinects[0];

            try
            {
                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking | RuntimeOptions.UseColor);
            }
            catch (InvalidOperationException)
            {
                System.Windows.MessageBox.Show("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                return;
            }
            try
            {
                nui.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
				nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
			}
            catch (InvalidOperationException)
            {
                System.Windows.MessageBox.Show("Failed to open stream. Please make sure to specify a supported image type and resolution.");
                return;
            }

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
        /// 関節座標を画面に表示する座標に変換します
        /// </summary>
        /// <param name="joint">関節ID</param>
        /// <param name="screenWidth">画面の横幅</param>
        /// <param name="screenWeight">画面の縦幅</param>
        /// <returns>変換された画面上の座標</returns>
        public Point GetDisplayPosition(Joint joint, double screenWidth, double screenHeight)
        {
            float depthX, depthY;
            nui.SkeletonEngine.SkeletonToDepthImage(joint.Position, out depthX, out depthY);

            depthX = Math.Max(0, Math.Min(depthX * 320, 320));  //convert to 320, 240 space
            depthY = Math.Max(0, Math.Min(depthY * 240, 240));  //convert to 320, 240 space
            int colorX, colorY;
            ImageViewArea iv = new ImageViewArea();
            // only ImageResolution.Resolution640x480 is supported at this point
            nui.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(ImageResolution.Resolution640x480, iv, (int)depthX, (int)depthY, (short)0, out colorX, out colorY);

            // map back to skeleton.Width & skeleton.Height
            return new Point((int)(screenWidth * colorX / 640.0), (int)(screenHeight * colorY / 480));
        }

		/// <summary>
		/// カメラの映像を画像として保存します
		/// </summary>
		public string capture()
		{
            BitmapFrame bmpFrame = BitmapFrame.Create(picture);

			FileStream stream = new FileStream(@"capture.jpeg", FileMode.Create);
			//PngBitmapEncoder pbenc = new PngBitmapEncoder();
			JpegBitmapEncoder jpenc = new JpegBitmapEncoder();

			//pbenc.Frames.Add(bmpFrame);
			//pbenc.Save(stream);
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

        /// <summary>
        /// キネクトをを解放します
        /// </summary>
        public void Uninitialize()
        {
            nui.Uninitialize();
        }
    }
}
