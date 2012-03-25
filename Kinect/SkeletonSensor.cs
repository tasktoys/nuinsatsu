using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using System.Threading;

namespace NUInsatsu.Kinect
{
    class SkeletonSensor : IDisposable
    {
        class SkeletonDataComparer : IComparer<SkeletonData>
        {
            public int Compare(SkeletonData lhs, SkeletonData rhs)
            {
                return (int)(lhs.Position.X - rhs.Position.X);
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// この先、キネクトが無かった場合NULLパターンを利用することを考えての実装です。
        /// </summary>
        /// <returns>生成されたインスタンス</returns>
        public static SkeletonSensor CreateInstance()
        {
            return new SkeletonSensor();
        }

        //現在のスケルトンフレームを格納
        private SkeletonFrame currentSkeletonFrame = null;

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
        /// SkeletonSensorクラスを構築します
        /// </summary>
        private SkeletonSensor()
        {
            Runtime nui;
            nui = NUInsatsu.Kinect.KinectManager.GetKinect();

            if (nui != null)
            {
                nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            }
        }

        /// <summary>
        /// 指定時間モーションを取得する
        /// </summary>
        /// <param name="time">時間</param>
        /// <returns>モーション</returns>
        public MotionList GetMotionForSeconds(int time)
        {
            System.Console.WriteLine("[Camera]time:{0}!", time);

            MotionList motionList = new MotionList();

            for (int i = 0; i < time; i++)
            {
                //現在のスケルトンを得る
                SkeletonDataList datas = Skeletons2List(CurrentSkeletonFrame.Skeletons);
                //左にいる人がコレクションの最初の方の要素になるように、x軸でソートする
                datas.Sort(new SkeletonDataComparer());
                motionList.Add(datas);
                Thread.Sleep(100);
            }

            return motionList;
        }

        /// <summary>
        /// リソースを解放します。利用し終わったら必ず呼び出してください。
        /// </summary>
        public void Dispose()
        {
            Runtime nui = NUInsatsu.Kinect.KinectManager.GetKinect();
            if (nui != null)
            {
                nui.SkeletonFrameReady -= new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            }
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
    }
}
