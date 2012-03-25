using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using System.Threading;

namespace NUInsatsu.Kinect
{
    class SkeletonSensorImpl : ISkeletonSensor
    {
        class SkeletonDataComparer : IComparer<SkeletonData>
        {
            public int Compare(SkeletonData lhs, SkeletonData rhs)
            {
                return (int)(lhs.Position.X - rhs.Position.X);
            }
        }

        /// <summary>
        /// SkeletonDataが追加されるまで待機するイベント
        /// </summary>
        private readonly AutoResetEvent AddSkeletonEvent = new AutoResetEvent(false);
        private MotionList motionList;
        private bool addSkeletonDataFlag = false;

        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;

        /// <summary>
        /// SkeletonSensorクラスを構築します
        /// </summary>
        public SkeletonSensorImpl()
        {
            Runtime nui;
            nui = NUInsatsu.Kinect.KinectInstanceManager.GetKinectInstance();

            if (nui != null)
            {
                nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            }
        }

        /// <summary>
        /// 指定時間モーションを取得する
        /// </summary>
        /// <param name="time">時間(秒)</param>
        /// <returns>モーション</returns>
        public MotionList GetMotionForSeconds(int time)
        {
            time = time * 10;

            motionList = new MotionList();

            for (int i = 0; i < time; i++)
            {
                // motionListにSkeletonDataがAddされるまで待機する
                addSkeletonDataFlag = true;
                AddSkeletonEvent.WaitOne();
                addSkeletonDataFlag = false;

                Thread.Sleep(100);
            }

            foreach (SkeletonDataList list in motionList)
            {
                // 左にいる人がコレクションの最初の方の要素になるように、x軸でソートする
                list.Sort(new SkeletonDataComparer());
            }

            return motionList;
        }

        /// <summary>
        /// リソースを解放します。利用し終わったら必ず呼び出してください。
        /// </summary>
        public void Dispose()
        {
            Runtime nui = NUInsatsu.Kinect.KinectInstanceManager.GetKinectInstance();
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
            if (e.SkeletonFrame == null) return;

            // SkeletonDataの保存が命じられている時、保存します。
            if (addSkeletonDataFlag)
            {
                motionList.Add(Skeletons2List(e.SkeletonFrame.Skeletons));
                AddSkeletonEvent.Set();
            }

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
