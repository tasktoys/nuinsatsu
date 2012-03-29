using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;

namespace NUInsatsu.Document
{
    /// <summary>
    /// モーション識別子の誤差を吸収し、目的のドキュメントを見つけやすくするための処理を提供します。
    /// </summary>
    class DistanceUtility
    {
        /// <summary>
        /// 識別子の一覧と、モーション識別子をマッチングし、近いドキュメント識別子を探します。
        /// </summary>
        /// <param name="motion">モーション識別子</param>
        /// <param name="keyList">識別子の一覧</param>
        /// <returns>該当するドキュメント識別子</returns>
        public static Key GetNearestDocumentKey(Key motion, List<Key> keyList)
        {
            List<Key> candidateList = new List<Key>();

            Config config = Config.Load();
            int threshold = config.MatchingThreshold;

            Console.WriteLine("[DistanceUtility] Request Key: {0} KeyList size: {1}", motion.KeyString, keyList.Count);
            Console.WriteLine("[DistanceUtility] Matching threshold: {0}", threshold);

            // ドキュメントを線形探索
            foreach (Key item in keyList)
            {
                // パーフェクトマッチするドキュメントがあれば、それを返す
                if (motion.KeyString == item.KeyString)
                {
                    Console.WriteLine("[DistanceUtility] Congratulations! Your motion is perfect match.");
                    return item;
                }

                int dist = distance(motion.KeyString, item.KeyString);

                // 距離が閾値以下ならば、目的のドキュメントの候補とみなします
                if (dist <= threshold)
                {
                    Console.WriteLine("[DistanceUtility] source: {0}", motion.KeyString);
                    Console.WriteLine("[DistanceUtility] target: {0}", item.KeyString);
                    Console.WriteLine("[DistanceUtility] distance: {0}", dist);
                    item.Distance = dist;
                    candidateList.Add(item);
                }
            }

            if (candidateList.Count <= 0)
            {
                // 近いドキュメントが見つからなかった
                Console.WriteLine("[DistanceUtility] mismatching");
                throw new DocumentNotFoundException("mismatching");
            }
            else
            {
                // 近いドキュメントが見つかった
                candidateList.Sort();
                return candidateList.FirstOrDefault();
            }
        }

        /// <summary>
        /// ２つの文字列のレーベンシュタイン距離を調べます。
        /// </summary>
        /// <param name="px">文字列1</param>
        /// <param name="py">文字列2</param>
        /// <returns>レーベンシュタイン距離</returns>
        private static int distance(String px, String py)
        {
            int len1 = px.Length;
            int len2 = py.Length;

            int[,] row = new int[len1 + 1, len2 + 1];
            int i, j;

            for (i = 0; i < len1 + 1; i++) row[i, 0] = i;
            for (i = 0; i < len2 + 1; i++) row[0, i] = i;

            for (i = 1; i <= len1; ++i)
            {
                for (j = 1; j <= len2; ++j)
                {
                    String pxtmp = px.Substring(i - 1, 1);
                    String pytmp = py.Substring(j - 1, 1);

                    int mintmp =
                        Math.Min((int)(row[i - 1, j - 1])
                        + (pxtmp == pytmp ? 0 : 1),
                        (int)(row[i, j - 1]) + 1);

                    row[i, j] = Math.Min(mintmp, (int)(row[i - 1, j]) + 1);
                }
            }

            return (int)(row[len1, len2]);
        }
    }
}
