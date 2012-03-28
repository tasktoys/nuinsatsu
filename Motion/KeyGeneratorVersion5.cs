using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Motion
{
    class KeyGeneratorVersion5 : KeyGenerator
    {
        public Key Generate(List<SkeletonTimeline> timelineList)
        {
            bool[] is_used = new bool[20];
            is_used[(int)JointID.HipCenter] = false;
            is_used[(int)JointID.Spine] = false;
            is_used[(int)JointID.ShoulderCenter] = false;
            is_used[(int)JointID.Head] = true;
            is_used[(int)JointID.ShoulderLeft] = false;

            is_used[(int)JointID.ElbowLeft] = false;
            is_used[(int)JointID.WristLeft] = false;
            is_used[(int)JointID.HandLeft] = true;
            is_used[(int)JointID.ShoulderRight] = false;
            is_used[(int)JointID.ElbowRight] = false;

            is_used[(int)JointID.WristRight] = false;
            is_used[(int)JointID.HandRight] = true;
            is_used[(int)JointID.HipLeft] = false;
            is_used[(int)JointID.KneeLeft] = false;
            is_used[(int)JointID.AnkleLeft] = true;

            is_used[(int)JointID.FootLeft] = false;
            is_used[(int)JointID.HipRight] = false;
            is_used[(int)JointID.KneeRight] = false;
            is_used[(int)JointID.AnkleRight] = true;
            is_used[(int)JointID.FootRight] = false;

            //for (int i = 0; i < is_used.length; i++)
            //is_used[i] = true;

            float[] threshold = new float[20];
            threshold[(int)JointID.Spine] = 0.2F;
            threshold[(int)JointID.ShoulderCenter] = 0.2F;
            threshold[(int)JointID.Head] = 0.2F;
            threshold[(int)JointID.ShoulderLeft] = 0.2F;

            threshold[(int)JointID.ElbowLeft] = 0.2F;
            threshold[(int)JointID.WristLeft] = 0.2F;
            threshold[(int)JointID.HandLeft] = 0.6F;
            threshold[(int)JointID.ShoulderRight] = 0.2F;
            threshold[(int)JointID.ElbowRight] = 0.2F;

            threshold[(int)JointID.WristRight] = 0.2F;
            threshold[(int)JointID.HandRight] = 0.6F;
            threshold[(int)JointID.HipLeft] = 0.2F;
            threshold[(int)JointID.KneeLeft] = 0.2F;
            threshold[(int)JointID.AnkleLeft] = 0.2F;

            threshold[(int)JointID.FootLeft] = 0.2F;
            threshold[(int)JointID.HipRight] = 0.2F;
            threshold[(int)JointID.KneeRight] = 0.25F;
            threshold[(int)JointID.AnkleRight] = 0.2F;
            threshold[(int)JointID.FootRight] = 0.2F;

            String hash = "DOC"; // prefix

            if (timelineList.Count < 1)
            {
                System.Console.Error.WriteLine("[KeyGeneratorVersion5]timelineListの要素数が0以下です.");
            }

            foreach(var timeline in timelineList) {
                //SkeletonTimeline timeline = (SkeletonTimeline)timelineList.get(0);
                float[,,] timelinearray = TimelineToArray(timeline);
                hash = MakeHash(hash, timelinearray, threshold, is_used);
            }

            //if (timelineList.size() > 1)
            //{
            //    SkeletonTimeline timeline2 = (SkeletonTimeline)timelineList.get(1);
            //    float[][][] timelinearray2 = TimelineToArray(timeline2);
            //    hash = MakeHash(hash, timelinearray2, threshold, is_used);
            //}

            Key key = new Key();
            key.KeyString = hash;
            return key;
        }

        private String MakeHash(String hash, float[,,] data, float[] threshold, bool[] is_used)
	    {
		    KeyGeneratorHelper helper = new KeyGeneratorHelper();
		    int njoint = data.GetLength(1);

		    // 標準化
		    data = helper.Standardlization(data);
		    float[,] buf = new float[data.GetLength(1),3];
            for (int i = 0; i < buf.GetLength(0); i++)
            {
                for (int x = 0; x < buf.GetLength(1); x++)
                {
                    buf[i, x] = data[0, i, x];
                }
            }
		    //for (int joint = 0; joint < njoint; joint++) {
		    for (int t = 0; t < data.GetLength(0); t++) {
		        for (int joint = 0; joint < njoint; joint++) {
			        if (is_used[joint] == true) {
                        float[] xyz = new float[data.GetLength(2)];
                        float[] buf_xyz = new float[buf.GetLength(1)];
                        for (int x = 0; x < data.GetLength(2); x++)
                        {
                            xyz[x] = data[t, joint, x];
                        }
                        for (int x = 0; x < buf.GetLength(1); x++)
                        {
                            buf_xyz[x] = buf[joint, x];
                        }
					    if (threshold[joint] < helper.GetDistance(buf_xyz, xyz)) {
						    hash += JointUtility.GetKeyToken((JointID)joint);
                            for (int x = 0; x < data.GetLength(2); x++)
                            {
                                buf[joint, x] = data[t, joint, x];
                            }
					    }
				    }
			    }
		    }
	        return hash;
	    }

        
        private float[,,] TimelineToArray(SkeletonTimeline timeline)
        {
		    Skeleton sk = timeline[0];
            int t = 0;
            int joint = 0;
		    float[,,] a = new float[timeline.Count,sk.Count,3];
		    foreach (var skel in timeline) {
			    //Skeleton skeleton = (Skeleton) timeline.get(t);
			    foreach (var dic in skel) {
				    //Point p = (Point) skeleton.get(new Integer(joint));
                    Point p = dic.Value;
				    a[t,joint,0] = p.X;
				    a[t,joint,1] = p.Y;
				    a[t,joint,2] = p.Z/10000;
                    joint++;
			    }
                joint = 0;
                t++;
		    }
		    return a;
	    }

    }
}
