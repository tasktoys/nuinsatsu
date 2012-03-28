using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NUInsatsu.Motion;
using System.IO;
using Microsoft.Research.Kinect.Nui;

namespace NUInsatsu.Net
{
    /// <summary>
    /// NMXPのmotionに対するパースを行います.
    /// </summary>
    class MotionResponseParser
    {
        /// <summary>
        /// XMLからPointクラスを生成します.
        /// </summary>
        /// <param name="reader">パースを行うXmlReader</param>
        /// <returns>生成されたPointクラス</returns>
        private Point parsePoint(XmlReader reader)
        {
            float x = 0, y = 0, z = 0;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "joint")
                    {
                        break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "x")
                    {
                        x = float.Parse(reader.ReadString());
                    }
                    else if (reader.Name == "y")
                    {
                        y = float.Parse(reader.ReadString());
                    }
                    else if (reader.Name == "z")
                    {
                        z = float.Parse(reader.ReadString());
                    }

                }
            }

            return new Point(x, y, z);
        }

        /// <summary>
        /// XMLからSkeletonクラスを生成します.
        /// </summary>
        /// <param name="reader">パースを行うXmlReader</param>
        /// <returns>生成されたSkeletonクラス</returns>
        private Skeleton parseSkeleton(XmlReader reader)
        {
            Skeleton skeleton = new Skeleton();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    // frameの終了要素タグを読み込んだとき、Skeletonの構成要素は一通り読み終わったので、生成したインスタンスを返す
                    if (reader.Name == "frame")
                    {
                        break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "joint")
                    {
                        JointID jointID = JointUtility.Attribute2id(reader.GetAttribute("id"));
                        Point point = parsePoint(reader);
                        skeleton.Add(jointID, point);
                    }
                }
            }

            return skeleton;
        }

        /// <summary>
        /// XMLからSkeletonTimelineを生成します.
        /// </summary>
        /// <param name="reader">パースを行うXML</param>
        /// <returns>生成されたSkeletonTimeline</returns>
        private SkeletonTimeline parseSkeletonTimeline(XmlReader reader)
        {
            SkeletonTimeline timeline = new SkeletonTimeline();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "timeline")
                    {
                        return timeline;
                    }
                }

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "frame")
                    {
                        Skeleton skeleton = parseSkeleton(reader);
                        timeline.Add(skeleton);
                    }
                }
            }
            return timeline;
        }

        /// <summary>
        /// XMLをパースし、モーション情報を生成します.
        /// </summary>
        /// <returns></returns>
        public List<SkeletonTimeline> Parse(String str)
        {
            Stream fs = new MemoryStream(Encoding.Unicode.GetBytes(str));

            List<SkeletonTimeline> skeletonTimelineList = new List<SkeletonTimeline>();

            try
            {
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "error")
                            {
                                throw new NMXPErrorMessageException();
                            }
                            else if (reader.Name == "timeline")
                            {
                                SkeletonTimeline timeline = parseSkeletonTimeline(reader);
                                skeletonTimelineList.Add(timeline);
                            }
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                throw new NMXPParseErrorException();
            }

            return skeletonTimelineList;
        }
    }
}
