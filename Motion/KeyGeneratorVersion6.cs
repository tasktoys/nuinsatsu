using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Motion
{
    class KeyGeneratorVersion6 : KeyGenerator
    {
        KeyGeneratorHelper helper = new KeyGeneratorHelper();
        float[] move_threshold = new float[20];
        float[] area_threshold = new float[20];
        bool[] is_used = new bool[20];

        KeyGeneratorVersion6()
        {
            for (int i = 0; i < move_threshold.GetLength(0); i++)
            {
                move_threshold[i] = 0.2F;
            }

            for (int i = 0; i < area_threshold.GetLength(0); i++)
            {
                area_threshold[i] = 0.2F;
            }

            for (int i = 0; i < is_used.GetLength(0); i++)
            {
                is_used[i] = true;
            }
        }

        public Key Generate(List<SkeletonTimeline> timelineList)
        {
            String hash = "DOC";

            foreach (var timeline in timelineList)
            {
                float[, ,] timelinearray = TimelineToArray(timeline);
                hash += MakeHash(timelinearray);
            }

            Key key = new Key();
            key.KeyString = hash;
            return key;
        }

        private String MakeHash(float[,,] data)
        {
            data = helper.Standardlization(data);
            float[,] var = ConvertToVariation(data);
            float[,] var_thr = ApplyMoveThreshold(var);
            float[,] area = ConvertToArea(var_thr);
            float[,] area_thr = ApplyAreaThreshold(area);
            String hash = ConvertToHash(area_thr);
            return "hoge";
        }

        private float[,] ConvertToVariation(float[, ,] data)
        {
            float[,] var = new float[data.GetLength(0),data.GetLength(1)];
            for (int t = 0; t < data.GetLength(0)-1; t++)
            {
                for (int joint = 0; joint < data.GetLength(1); joint++)
                {
                    float[] tem1 = new float[3];
                    float[] tem2 = new float[3];
                    for (int xyz = 0; xyz < 3; xyz++) {
                        tem1[xyz] = data[t,joint,xyz];
                        tem2[xyz] = data[t+1,joint,xyz];
                    }
                    var[t,joint] = helper.GetDistance(tem1,tem2);
                }
            }
            return var;
        }

        private float[,] ApplyMoveThreshold(float[,] var)
        {
            for (int t = 0; t < var.GetLength(0); t++)
            {
                for (int joint = 0; joint < var.GetLength(1); joint++)
                {
                    var[t, joint] -= move_threshold[joint];
                    if (var[t, joint] < 0)
                    {
                        var[t, joint] = 0;
                    }
                }
            }
            return var;
        }

        private float[,] ConvertToArea(float[,] var_thr)
        {
            int state = 0;
            int start_time = 0;
            float tem_area = 0.0F;
            float[,] area = new float[var_thr.GetLength(0), var_thr.GetLength(1)];
            for (int joint = 0; joint < var_thr.GetLength(1); joint++)
            {
                for (int t = 0; t < var_thr.GetLength(0); t++)
                {
                    if (0 < var_thr[t, joint] && state == 0)
                    {
                        start_time = t;
                        state = 1;
                    }
                    else if (0 < var_thr[t,joint] && state == 1)
                    {
                        tem_area += var_thr[t, joint];
                    }
                    else if ((0 < var_thr[t, joint]) == false && state == 1)
                    {
                        area[(int)(start_time+t)/2, joint] = tem_area;
                        tem_area = 0.0F;
                        state = 0;
                    }
                }
            }
            return area;
        }

        private float[,] ApplyAreaThreshold(float[,] area)
        {
            for (int t = 0; t < area.GetLength(0); t++)
            {
                for (int joint = 0; joint < area.GetLength(1); joint++)
                {
                    area[t, joint] -= area_threshold[joint];
                    if (area[t, joint] < 0)
                    {
                        area[t, joint] = 0;
                    }
                }
            }
            return area;
        }

        private String ConvertToHash(float[,] area_thr)
        {
            String hash = "";
            for (int t = 0; t < area_thr.GetLength(0); t++)
            {
                for (int joint = 0; joint < area_thr.GetLength(1); joint++)
                {
                    if (area_thr[t, joint] != 0.0F)
                    {
                        hash += JointUtility.GetKeyToken((JointID)joint);
                    }
                }
            }
            return hash; 
        }

        private float[, ,] TimelineToArray(SkeletonTimeline timeline)
        {
            Skeleton sk = timeline[0];
            int t = 0;
            int joint = 0;
            float[, ,] a = new float[timeline.Count, sk.Count, 3];
            foreach (var skel in timeline)
            {
                foreach (var dic in skel)
                {
                    Point p = dic.Value;
                    a[t, joint, 0] = p.X;
                    a[t, joint, 1] = p.Y;
                    a[t, joint, 2] = p.Z / 10000;
                    joint++;
                }
                joint = 0;
                t++;
            }
            return a;
        }
    }
}
