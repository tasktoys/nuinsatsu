using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Motion
{
    class KeyGeneratorHelper
    {
        public float[,,] Standardlization(float[,,] timelinearray)
        {
		    // point[0] x座標, point[1] y座標, point[2] z座標
		    float[] anklel = new float[3];
		    float[] ankler = new float[3];
		    float[] shoulderl = new float[3];
		    float[] shoulderr = new float[3];
		    float[] head = new float[3];

		    for (int i = 0; i < 2; i++) {
			    anklel[i] = timelinearray[0,(int)JointID.AnkleLeft, i];
			    ankler[i] = timelinearray[0,(int)JointID.AnkleRight, i];
			    shoulderl[i] = timelinearray[0,(int)JointID.HandLeft, i];
			    shoulderr[i] = timelinearray[0,(int)JointID.HandRight, i];
			    head[i] = timelinearray[0,(int)JointID.Head, i];
		    }

		    float[,,] data = new float[timelinearray.GetLength(0),timelinearray.GetLength(1),timelinearray.GetLength(2)];
		    for (int t = 0; t < data.GetLength(0); t++)
			    for (int joint = 0; joint < data.GetLength(1); joint++)
				    for (int x = 0; x < data.GetLength(2); x++) {
					    data[t,joint,x] = timelinearray[t,joint,x];
				    }

		    // (体の中心のx座標,頭のy座標,体の中心のz座標)　という点を原点にする
		    float[] center = new float[3];
		    for (int i = 0; i < center.GetLength(0); i++)
			    center[i] = (anklel[i] + ankler[i] + shoulderl[i] + shoulderr[i]) / 4;

		    center[1] = head[1];
		    data = Centerize(data, center);

		    // 体の向きを真正面を向いているようにそろえる
		    float theta = 0;
		    if (Math.Abs(shoulderl[0] - shoulderr[0]) != 0) {
			    float xabs = Math.Abs(shoulderl[0]) + Math.Abs(shoulderr[0]);
			    float zabs = Math.Abs(shoulderl[2]) + Math.Abs(shoulderr[2]);
			    theta = (float) (Math.Atan(zabs / xabs));
		    }

		    // 左肩が前で右肩が後ろのときは時計回り
		    if (shoulderl[2] < shoulderr[2])
			    data = Rotate(data, -1 * theta);
		    // 右肩が前で左肩が後ろのときは反時計回り
		    else if (shoulderr[2] < shoulderl[2])
			    data = Rotate(data, theta);

		    // 座標の規格化
		    float ratio = (anklel[1] + ankler[1]) / 2 - head[1];
		    if (0 < ratio)
			    data = Normalize(data, ratio);
		    else
			    data = Normalize(data, 1);

		    return data;
	    }

        private float[,,] Rotate(float[,,] data, float theta)
        {
            for (int t = 0; t < data.GetLength(0); t++)
                for (int joint = 0; joint < data.GetLength(1); joint++)
                {
                    float x = data[t,joint,0];
                    float y = data[t,joint,1];
                    float z = data[t,joint,2];
                    data[t,joint,0] = (float)(x * Math.Cos(theta) - z
                            * Math.Sin(theta));
                    data[t,joint,1] = y;
                    data[t,joint,2] = (float)(x * Math.Sin(theta) + z
                            * Math.Cos(theta));
                }
            return data;
        }

        private float[,,] Centerize(float[,,] data, float[] center)
        {
            for (int t = 0; t < data.GetLength(0); t++)
                for (int joint = 0; joint < data.GetLength(1); joint++)
                    for (int x = 0; x < data.GetLength(2); x++)
                        data[t,joint,x] -= center[x];
            return data;
        }

        private float[,,] Normalize(float[,,] data, float ratio)
        {
            for (int t = 0; t < data.GetLength(0); t++)
                for (int joint = 0; joint < data.GetLength(1); joint++)
                    for (int x = 0; x < data.GetLength(2); x++)
                        data[t,joint,x] = data[t,joint,x] / ratio;
            return data;
        }

        public float GetDistance(float[] data1, float[] data2)
        {
            float distance = 0.0F;
            float dx = (data1[0] - data2[0]) * (data1[0] - data2[0]);
            float dy = (data1[1] - data2[1]) * (data1[1] - data2[1]);
            float dz = (data1[2] - data2[2]) * (data1[2] - data2[2]);
            distance = (float)(Math.Sqrt(dx + dy + dz));
            return distance;
        }

    }
}
