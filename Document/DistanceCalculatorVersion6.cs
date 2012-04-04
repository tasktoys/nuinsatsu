using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;

namespace NUInsatsu.Document
{
    /// <summary>
    /// モーションアルゴリズムVersion6用のモーション識別子の誤差を吸収し、目的のドキュメントを見つけやすくするための処理を提供します。
    /// </summary>
    class DistanceCalculatorVersion6 : DistanceCalculator
    {
        /// <summary>
        /// 識別子の一覧と、モーション識別子をマッチングし、近いドキュメント識別子を探します。
        /// </summary>
        /// <param name="motion">モーション識別子</param>
        /// <param name="keyList">識別子の一覧</param>
        /// <returns>該当するドキュメント識別子</returns>
        public override Key GetNearestDocumentKey(Key motion, List<Key> keyList)
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

                int dist = Distance(motion.KeyString, item.KeyString);

                // 距離が閾値以下ならば、目的のドキュメントの候補とみなします
                if (dist <= threshold)
                {
                    Console.WriteLine("[DistanceUtility] source: {0}", motion.KeyString);
                    Console.WriteLine("[DistanceUtility] target: {0}", item.KeyString);
                    Console.WriteLine("[DistanceUtility] Distance: {0}", dist);
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
        /// ２つの文字列の距離を調べます。
        /// </summary>
        /// <param name="px">文字列1</param>
        /// <param name="py">文字列2</param>
        /// <returns>Version6用距離</returns>
        private static int Distance(String px, String py)
        {
            int distance = 0;
            String[] input = px.Split(new char[] { '#' });
            String[] searched = py.Split(new char[] { '#' });
            List<String> input_list = new List<String>();
            List<String> searched_list = new List<String>();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].Equals(""))
                {
                    ;
                }
                else
                {
                    input_list.Add(input[i]);
                }
            }

            for (int i = 0; i < searched.Length; i++)
            {
                if (searched[i].Equals(""))
                {
                    ;
                }
                else
                {
                    searched_list.Add(searched[i]);
                }
            }

            for (int i = 0; i < input_list.Count; i++)
            {
                if (i < searched_list.Count)
                {
                    distance += StringDistance(input_list.ElementAt<String>(i), searched_list.ElementAt<String>(i));
                }
                else
                {
                    distance += input_list.ElementAt<String>(i).Length;
                }
            }
            if (input_list.Count < searched_list.Count)
            {
                for (int i = input_list.Count; i < searched_list.Count; i++)
                {
                    distance += searched_list.ElementAt<String>(i).Length;
                }
            }
            return distance;
        }

        static private int StringDistance(String input_string, String searched_string)
        {
            int distance = 0;
            for (int i = 0; i < input_string.Length; i++)
            {
                String c = input_string.Substring(i, 1);
                if (searched_string.Contains(c) == false)
                {
                    distance++;
                }
            }
            for (int i = 0; i < searched_string.Length; i++)
            {
                String c = searched_string.Substring(i, 1);
                if (input_string.Contains(c) == false)
                {
                    distance++;
                }
            }
            return distance;
        }
    }
}
