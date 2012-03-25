using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using System.Threading;
using System.IO;

namespace NUInsatsu.Motion
{
    /// <summary>
    /// モーション固有の文字列（キー）を生成します。削除予定。
    /// </summary>
    class KeyMaker
    {
        private static readonly JointID[] jointIDList = {
                                    JointID.AnkleLeft,
                                    JointID.AnkleRight,
                                    JointID.ElbowLeft,
                                    JointID.ElbowRight,
                                    JointID.FootLeft,
                                    JointID.FootRight,
                                    JointID.HandLeft,
                                    JointID.HandRight,
                                    JointID.Head,
                                    JointID.HipCenter,
                                    JointID.HipLeft,
                                    JointID.HipRight,
                                    JointID.KneeLeft,
                                    JointID.KneeRight,
                                    JointID.ShoulderCenter,
                                    JointID.ShoulderLeft,
                                    JointID.ShoulderRight,
                                    JointID.Spine,
                                    JointID.WristLeft,
                                    JointID.WristRight
                                };

        /// <summary>
        /// モーションと対となるキーを生成します
        /// </summary>
        /// <returns>生成されたキー</returns>
        public String Make(MotionList motions)
        {
            return CreateNMXPResponseMessage(motions);
        }

        private static String CreateNMXPResponseMessage(MotionList motionList)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();

            result.Append("<?xml version=\"1.0\"?>");
            result.Append("<nmxp version=\"2.0\">");
            result.Append("<response type=\"motion\">");

            // Persons
            int persons = motionList[0].Count;
            for (int i = 0; i < persons; i++)
            {
                result.Append("<timeline id=\"" + i + "\">");

                // Timeline
                for (int j = 0; j < motionList.Count; j++)
                {
                    result.Append("<frame count=\"" + j + "\">");
                    SkeletonData skeletonData = motionList[j][i];

                    foreach (JointID jointID in jointIDList)
                    {
                        result.Append("<joint id=\"" + JointID2String(jointID) + "\">");
                        result.Append("<x>" + skeletonData.Joints[jointID].Position.X + "</x>");
                        result.Append("<y>" + skeletonData.Joints[jointID].Position.Y + "</y>");
                        result.Append("<z>" + skeletonData.Joints[jointID].Position.Z + "</z>");
                        result.Append("</joint>");

                    }
                    result.Append("</frame>");
                }
                result.Append("</timeline>");
            }
            result.Append("</response>");
            result.Append("</nmxp>");

            return result.ToString();
        }

        private static String JointID2String(JointID jointID)
        {
            switch (jointID) {
                case JointID.AnkleLeft:     return "ankle_left";
                case JointID.AnkleRight:    return "ankle_right";
                case JointID.ElbowLeft:     return "elbow_left";
                case JointID.ElbowRight:    return "elbow_right";
                case JointID.FootLeft:      return "foot_left";
                case JointID.FootRight:     return "foot_right";
                case JointID.HandLeft:      return "hand_left";
                case JointID.HandRight:     return "hand_right";
                case JointID.Head:          return "head";
                case JointID.HipCenter:     return "hip_center";
                case JointID.HipLeft:       return "hip_left";
                case JointID.HipRight:      return "hip_right";
                case JointID.KneeLeft:      return "knee_left";
                case JointID.KneeRight:     return "knee_right";
                case JointID.ShoulderCenter:return "shoulder_center";
                case JointID.ShoulderLeft:  return "shoulder_left";
                case JointID.ShoulderRight: return "shoulder_right";
                case JointID.Spine:         return "spine";
                case JointID.WristLeft:     return "wrist_left";
                case JointID.WristRight:    return "wrist_right";
                default:                    return "ERROR";
            }
        }
    }
}