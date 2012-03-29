using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Motion
{
    class JointUtility
    {
        /// <summary>
        /// NMXPのJoint要素の属性からJointIDを取得します.
        /// </summary>
        /// <param name="attribute">joint 要素の id 属性</param>
        /// <returns>JointID</returns>
	    public static JointID Attribute2id(String attribute) {
		    if (attribute.CompareTo("head") == 0) { return JointID.Head; }
            else if (attribute.CompareTo("spine") == 0) { return JointID.Spine; }
            else if (attribute.CompareTo("hip_center") == 0) { return JointID.HipCenter; }
            else if (attribute.CompareTo("hip_left") == 0) { return JointID.HipLeft; }
            else if (attribute.CompareTo("hip_right") == 0) { return JointID.HipRight; }
            else if (attribute.CompareTo("shoulder_center") == 0) { return JointID.ShoulderCenter; }
            else if (attribute.CompareTo("shoulder_left") == 0) { return JointID.ShoulderLeft ; }
            else if (attribute.CompareTo("shoulder_right") == 0) { return JointID.ShoulderRight ; }
            else if (attribute.CompareTo("elbow_left") == 0) { return JointID.ElbowLeft ; }
            else if (attribute.CompareTo("elbow_right") == 0) { return JointID.ElbowRight ; }
            else if (attribute.CompareTo("wrist_left") == 0) { return JointID.WristLeft ; }
            else if (attribute.CompareTo("wrist_right") == 0) { return JointID.WristRight ; }
            else if (attribute.CompareTo("hand_left") == 0) { return JointID.HandLeft ; }
            else if (attribute.CompareTo("hand_right") == 0) { return JointID.HandRight ; }
            else if (attribute.CompareTo("knee_left") == 0) { return JointID.KneeLeft ; }
            else if (attribute.CompareTo("knee_right") == 0) { return JointID.KneeRight ; }
            else if (attribute.CompareTo("ankle_left") == 0) { return JointID.AnkleLeft ; }
            else if (attribute.CompareTo("ankle_right") == 0) { return JointID.AnkleRight ; }
            else if (attribute.CompareTo("foot_left") == 0) { return JointID.FootLeft ; }
            else if (attribute.CompareTo("foot_right") == 0) { return JointID.FootRight ; }
            else 
            {
                throw new ArgumentException("Illegal JointID: " + attribute);
            }
	    }


        /// <summary>
        /// JointIDをキー用のトークンに変換します.
        /// </summary>
        /// <param name="id">JointID</param>
        /// <returns>キー用のトークン</returns>
	    public static String GetKeyToken(JointID id) {
		
		    switch (id) {
		    case JointID.Head:				return "a";
            case JointID.Spine:               return "b";
		    case JointID.HipCenter :          return "c";
		    case JointID.HipLeft :			return "d";
		    case JointID.HipRight :			return "e";
		    case JointID.ShoulderCenter :	return "f";
		    case JointID.ShoulderLeft :		return "g";
		    case JointID.ShoulderRight :	return "h";
		    case JointID.ElbowLeft :		return "i";
		    case JointID.ElbowRight :		return "j";
		    case JointID.WristLeft :		return "k";
		    case JointID.WristRight :		return "l";
		    case JointID.HandLeft :			return "m";
		    case JointID.HandRight :		return "n";
		    case JointID.KneeLeft :			return "o";
		    case JointID.KneeRight :		return "p";
		    case JointID.AnkleLeft :		return "q";
		    case JointID.AnkleRight :		return "r";
		    case JointID.FootLeft :			return "s";
		    case JointID.FootRight :		return "t";
		    default: throw new ArgumentException("Illegal JointID: " + id);
		    }
	    }

    }
}
