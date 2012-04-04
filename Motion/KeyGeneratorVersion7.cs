using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;


namespace NUInsatsu.Motion
{
    class KeyGeneratorVersion7 : KeyGenerator
    {
        KeyGeneratorHelper helper = new KeyGeneratorHelper();
        int time_width;
        float[] move_threshold = new float[20];
        float[] area_threshold = new float[20];
        bool[] is_used = new bool[20];

        public KeyGeneratorVersion7()
        {
            time_width = 5;

            move_threshold[(int)JointID.Head] = 0.1F;

            move_threshold[(int)JointID.HandRight] = 0.1F;
            move_threshold[(int)JointID.HandLeft] = 0.1F;
            move_threshold[(int)JointID.ElbowRight] = 0.1F;
            move_threshold[(int)JointID.ElbowLeft] = 0.1F;
            move_threshold[(int)JointID.ShoulderRight] = 0.1F;
            move_threshold[(int)JointID.ShoulderCenter] = 0.1F;
            move_threshold[(int)JointID.ShoulderLeft] = 0.1F;

            move_threshold[(int)JointID.AnkleLeft] = 0.1F;
            move_threshold[(int)JointID.AnkleRight] = 0.1F;
            move_threshold[(int)JointID.FootLeft] = 0.1F;
            move_threshold[(int)JointID.FootRight] = 0.1F;
            move_threshold[(int)JointID.KneeLeft] = 0.1F;
            move_threshold[(int)JointID.KneeRight] = 0.1F;

            move_threshold[(int)JointID.WristRight] = 0.1F;
            move_threshold[(int)JointID.WristLeft] = 0.1F;
            move_threshold[(int)JointID.Spine] = 0.1F;
            move_threshold[(int)JointID.HipRight] = 0.1F;
            move_threshold[(int)JointID.HipCenter] = 0.1F;
            move_threshold[(int)JointID.HipLeft] = 0.1F;


            for (int i = 0; i < area_threshold.GetLength(0); i++)
            {
                area_threshold[i] = 0;//(float)(move_threshold[i] * time_width * 0.5);
            }
            area_threshold[(int)JointID.Head] = 0.1F;

            area_threshold[(int)JointID.HandRight] = 0.2F;
            area_threshold[(int)JointID.HandLeft] = 0.2F;
            area_threshold[(int)JointID.ElbowRight] = 0.1F;
            area_threshold[(int)JointID.ElbowLeft] = 0.1F;
            area_threshold[(int)JointID.ShoulderRight] = 0.1F;
            area_threshold[(int)JointID.ShoulderCenter] = 0.1F;
            area_threshold[(int)JointID.ShoulderLeft] = 0.1F;

            area_threshold[(int)JointID.AnkleLeft] = 0.1F;
            area_threshold[(int)JointID.AnkleRight] = 0.1F;
            area_threshold[(int)JointID.FootLeft] = 0.1F;
            area_threshold[(int)JointID.FootRight] = 0.1F;
            area_threshold[(int)JointID.KneeLeft] = 0.1F;
            area_threshold[(int)JointID.KneeRight] = 0.1F;

            area_threshold[(int)JointID.WristRight] = 0.1F;
            area_threshold[(int)JointID.WristLeft] = 0.1F;
            area_threshold[(int)JointID.Spine] = 0.1F;
            area_threshold[(int)JointID.HipRight] = 0.1F;
            area_threshold[(int)JointID.HipCenter] = 0.1F;
            area_threshold[(int)JointID.HipLeft] = 0.1F;

            is_used[(int)JointID.Head] = true;

            is_used[(int)JointID.HandRight] = true;
            is_used[(int)JointID.HandLeft] = true;
            is_used[(int)JointID.ElbowRight] = true;
            is_used[(int)JointID.ElbowLeft] = true;
            is_used[(int)JointID.ShoulderRight] = false;
            is_used[(int)JointID.ShoulderCenter] = true;
            is_used[(int)JointID.ShoulderLeft] = false;

            is_used[(int)JointID.AnkleLeft] = true;
            is_used[(int)JointID.AnkleRight] = true;
            is_used[(int)JointID.FootLeft] = false;
            is_used[(int)JointID.FootRight] = false;
            is_used[(int)JointID.KneeLeft] = true;
            is_used[(int)JointID.KneeRight] = true;

            is_used[(int)JointID.WristRight] = false;
            is_used[(int)JointID.WristLeft] = false;
            is_used[(int)JointID.Spine] = false;
            is_used[(int)JointID.HipRight] = false;
            is_used[(int)JointID.HipLeft] = false;
            is_used[(int)JointID.HipCenter] = true;
        }

        public Key Generate(List<SkeletonTimeline> timelineList)
        {
            String hash = "DOC#";

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
            return hash;
        }

        private float[,] ConvertToVariation(float[, ,] data)
        {
            float[,] var = new float[data.GetLength(0),data.GetLength(1)];
            for (int t = 0; t < data.GetLength(0)-1; t++)
            {
                for (int joint = 0; joint < data.GetLength(1); joint++)
                {
                    if (is_used[joint] == true)
                    {
                        float[] tem1 = new float[3];
                        float[] tem2 = new float[3];
                        for (int xyz = 0; xyz < 3; xyz++)
                        {
                            tem1[xyz] = data[t, joint, xyz];
                            tem2[xyz] = data[t + 1, joint, xyz];
                        }
                        var[t, joint] = helper.GetDistance(tem1, tem2);
                    }
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
                        float half_area = 0.0F;
                        int middle_time = start_time;
                        for (int t2 = start_time; t2 < t; t2++)
                        {
                            if (half_area <= tem_area / 2)
                            {
                                half_area += var_thr[t2, joint];
                            }
                            else if (middle_time == start_time)
                            {
                                middle_time = t2;
                            }
                        }
                        area[middle_time, joint] = tem_area;
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
            int time_checked = 0;
            for (int t = 0; t < area_thr.GetLength(0); t += time_width)
            {
                for (int joint = 0; joint < area_thr.GetLength(1); joint++) 
                {
                    for (int dt = 0; dt < time_width; dt++)
                    {
                        if (area_thr[t + dt, joint] != 0.0F)
                        {
                            hash += JointUtility.GetKeyToken((JointID)joint);
                        }
                    }
                }
                hash += "#";
                time_checked = t;
            }

            for (int joint = 0; joint < area_thr.GetLength(1); joint++) 
            {
                    for (int t = time_checked + 1; t < area_thr.GetLength(0); t++)
                    {
                        if (area_thr[t, joint] != 0.0F)
                        {
                            hash += JointUtility.GetKeyToken((JointID)joint);
                        }
                    }
            }
            hash += "#";
            return hash; 
        }

        private float[, ,] TimelineToArray(SkeletonTimeline timeline)
        {
            Skeleton sk = timeline[0];
            int t = 0;
            float[, ,] a = new float[timeline.Count, sk.Count, 3];
            foreach (var skel in timeline)
            {
                foreach (var dic in skel)
                {
                    Point p = dic.Value;
                    int joint = (int)dic.Key;
                    a[t, joint, 0] = p.X;
                    a[t, joint, 1] = p.Y;
                    a[t, joint, 2] = p.Z;
                }
                t++;
            }
            return a;
        }
    }
}
