using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUInsatsu.Motion;

namespace NUInsatsu.Document
{
    /// <summary>
    /// モーションアルゴリズムVersion7用のモーション識別子の誤差を吸収し、目的のドキュメントを見つけやすくするための処理を提供します。
    /// </summary>
    class DistanceCalculatorVersion7 : DistanceCalculator
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
        /// <returns>Version7用距離</returns>
        private static int Distance(String px, String py)
        {
            int distance = 0;
            String[] input = px.Split(new char[] { '#' });
            String[] searched = py.Split(new char[] { '#' });
            List<String> input_list = new List<String>();
            List<String> searched_list = new List<String>();

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i].Equals("") == false)
                {
                    input_list.Add(input[i]);
                }
            }

            for (int i = 1; i < searched.Length; i++)
            {
                if (searched[i].Equals("") == false)
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
                    distance += StringDistance(input_list.ElementAt<String>(i), "");
                }
            }

            if (input_list.Count < searched_list.Count)
            {
                for (int i = input_list.Count; i < searched_list.Count; i++)
                {
                    distance += StringDistance(searched_list.ElementAt<String>(i), "");
                }
            }
            return distance;
        }

        static private int StringDistance(String input_string, String searched_string)
        {
            int distance = 0;
            String[] input = input_string.Split(new char[] { '&' });
            String[] searched = searched_string.Split(new char[] { '&' });
            List<String> input_list = new List<String>();
            List<String> searched_list = new List<String>();

            int state = 0;
            Regex alphabet_reg = new Regex(@"\w");
            Regex number_reg = new Regex(@"\d");

            for (int i = 0; i < input.Length; i++)
            {
                if (input.ElementAt<String>(i).Equals(""))
                {
                    ;
                }
                else if (alphabet_reg.IsMatch(input[i]) && state == 0)
                {
                    input_list.Add(input[i]);
                    state = 1;
                }
                else if (number_reg.IsMatch(input[i]) && state == 1)
                {
                    input_list.Add(input[i]);
                    state = 0;
                }
                else
                {
                    Console.Error.WriteLine("Key format is not Version7");
                }
            }

            state = 0;
            for (int i = 0; i < searched.Length; i++)
            {
                if (searched.ElementAt<String>(i).Equals(""))
                {
                    ;
                }
                else if (alphabet_reg.IsMatch(searched[i]) && state == 0)
                {
                    searched_list.Add(searched[i]);
                    state = 1;
                }
                else if (number_reg.IsMatch(searched[i]) && state == 1)
                {
                    searched_list.Add(searched[i]);
                    state = 0;
                }
                else
                {
                    Console.Error.WriteLine("Key format is not Version7");
                }
            }

            //if ((int)(input_list.Count / 2) != 0 || (int)(searched_list.Count / 2) != 0)
            //{
            //    Console.Error.WriteLine("Key format is not Version7");
            //    return 0;
            //}

            for (int i = 0; i < input_list.Count; i += 2)
            {
                String c = input_list.ElementAt<String>(i);
                if (searched_list.Contains(c) == false)
                {
                    distance += int.Parse(input_list.ElementAt<String>(i + 1));
                }
                else
                {
                    distance += FindDifference(searched_list, c, int.Parse(input_list.ElementAt<String>(i + 1)));
                }
            }

            for (int i = 0; i < searched_list.Count; i += 2)
            {
                String c = searched_list.ElementAt<String>(i);
                if (input_list.Contains(c) == false)
                {
                    distance += int.Parse(searched_list.ElementAt<String>(i + 1));
                }
            }

            return distance;
        }

        private static int FindDifference(List<String> data, String c, int from)
        {
            int index = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (data.ElementAt<String>(i).Equals(c))
                {
                    index = i;
                }
            }
            return Math.Abs(int.Parse(data.ElementAt<String>(index + 1)) - from);
        }
    }
}

