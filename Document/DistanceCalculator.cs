using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;

namespace NUInsatsu.Document
{
    abstract class DistanceCalculator
    {
        public static DistanceCalculator CreateInstance()
        {
            Config config = Config.Load();
            if (config.MotionAlgorithm == "6")
            {
                return new DistanceCalculatorVersion6();
            }
            else
            {
                return new LevenshteinDistance();
            }
        }

        /// <summary>
        /// 識別子の一覧と、モーション識別子をマッチングし、近いドキュメント識別子を探します。
        /// </summary>
        /// <param name="motion">モーション識別子</param>
        /// <param name="keyList">識別子の一覧</param>
        /// <returns>該当するドキュメント識別子</returns>
        public abstract Key GetNearestDocumentKey(Key motion, List<Key> keyList);

    }
}