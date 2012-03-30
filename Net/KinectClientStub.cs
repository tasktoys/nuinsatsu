﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using NUInsatsu.Motion;

namespace NUInsatsu.Net
{
    /// <summary>
    /// KinectClientデバッグ用スタブです.
    /// </summary>
    class KinectClientStub : KinectClient
    {
        bool KinectClient.Connect()
        {
            return true;
        }

        bool KinectClient.Close()
        {
            return true;
        }

        List<Motion.SkeletonTimeline> KinectClient.GetMotionList()
        {
            // SkeletonTimelieのリストを作る（全データ)
            List<SkeletonTimeline> motionList = new List<SkeletonTimeline>();

            // SkeletonTimelineを作る（一人分のデータの塊）
            SkeletonTimeline t1 = new SkeletonTimeline();

            Random random = new Random();
            // Skeleton(ある時刻の体のデータ)を作る(30個)
            foreach( int i in Enumerable.Range(0,30))
            {
                Skeleton skeleton = new Skeleton();

                float x = (float)random.NextDouble();
                float y = (float)random.NextDouble();
                float z = (float)random.NextDouble();

                Point point = new Point(x, y, z);

                skeleton.Add(JointID.Head, point);
                skeleton.Add(JointID.Spine, point);
                skeleton.Add(JointID.HipCenter, point);
                skeleton.Add(JointID.HipLeft, point);
                skeleton.Add(JointID.HipRight, point);
                skeleton.Add(JointID.ShoulderCenter, point);
                skeleton.Add(JointID.ShoulderLeft, point);
                skeleton.Add(JointID.ShoulderRight, point);
                skeleton.Add(JointID.ElbowLeft, point);
                skeleton.Add(JointID.ElbowRight, point);
                skeleton.Add(JointID.WristLeft, point);
                skeleton.Add(JointID.WristRight, point);
                skeleton.Add(JointID.HandLeft, point);
                skeleton.Add(JointID.HandRight, point);
                skeleton.Add(JointID.KneeLeft, point);
                skeleton.Add(JointID.KneeRight, point);
                skeleton.Add(JointID.AnkleLeft, point);
                skeleton.Add(JointID.AnkleRight, point);
                skeleton.Add(JointID.FootLeft, point);
                skeleton.Add(JointID.FootRight, point);

                t1.Add(skeleton);
            }

            motionList.Add(t1);

            return motionList;

        }

        System.IO.FileInfo KinectClient.GetFaceImage()
        {
            throw new NotImplementedException();
        }

        void KinectClient.SendTest()
        {
            throw new NotImplementedException();
        }

        void KinectClient.SendNavigation(string navigation)
        {
            throw new NotImplementedException();
        }
    }
}