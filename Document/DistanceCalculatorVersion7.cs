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

            Console.WriteLine("[DistanceCalculatorVersion7] Request Key: {0} KeyList size: {1}", motion.KeyString, keyList.Count);
            Console.WriteLine("[DistanceCalculatorVersion7] Matching threshold: {0}", threshold);

            // ドキュメントを線形探索
            foreach (Key item in keyList)
            {
                // パーフェクトマッチするドキュメントがあれば、それを返す
                if (motion.KeyString == item.KeyString)
                {
                    Console.WriteLine("[DistanceCalculatorVersion7] Congratulations! Your motion is perfect match.");
                    return item;
                }

                int dist = Distance(motion.KeyString, item.KeyString);

                Console.WriteLine("[DistanceCalculatorVersion7] source: {0}", motion.KeyString);
                Console.WriteLine("[DistanceCalculatorVersion7] target: {0}", item.KeyString);
                Console.WriteLine("[DistanceCalculatorVersion7] Distance: {0}", dist);

                // 距離が閾値以下ならば、目的のドキュメントの候補とみなします
                if (dist <= threshold)
                {
                    item.Distance = dist;
                    candidateList.Add(item);
                }
            }

            if (candidateList.Count <= 0)
            {
                // 近いドキュメントが見つからなかった
                Console.WriteLine("[DistanceCalculatorVersion7] mismatching");
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
            KeyValuePair<String, int>[] pairs1 = ParseJointElement(input_string);
            KeyValuePair<String, int>[] pairs2 = ParseJointElement(searched_string);

            int distance = JointWeight(pairs1,pairs2);
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

        /// <summary>
        /// MotionAlgorithmV7の識別子のある時間から、関節とその変位を生成します
        /// </summary>
        /// <remarks>
        /// "a1j3m2"といった文字列を、aと1のペアー、jと3のペアー、mと2のペアーを作り、配列にして返す
        /// </remarks>
        /// <param name="s">アルゴリズムv7の時間（"a1j3m2"など)</param>
        /// <returns>関節とその変異の配列</returns>
        static KeyValuePair<String, int>[] ParseJointElement(String s)
        {
            Regex rx = new Regex(@"[a-z][0-9]");

            MatchCollection mc = rx.Matches(s);

            List<KeyValuePair<String, int>> pairList = new List<KeyValuePair<string, int>>();

            foreach (Match m in mc)
            {
                String v = m.Value[0].ToString();
                int d = int.Parse(m.Value[1].ToString());

                pairList.Add(new KeyValuePair<string, int>(v, d));
            }

            return pairList.ToArray();
        }


        /// <summary>
        /// 関節とその変異の配列から、その変位を返します
        /// </summary>
        /// <param name="joints1">比較する間接と変位の配列</param>
        /// <param name="joints2">比較する間接と変位の配列</param>
        /// <returns>変位</returns>
        static int JointWeight(KeyValuePair<String, int>[] joints1, KeyValuePair<String, int>[] joints2)
        {
            int weight = 0;

            // 両方の配列に入っている関節と、joints1にしか入っていない関節を足す
            foreach (KeyValuePair<String, int> joint in joints1)
            {
                KeyValuePair<String, int> p = Array.Find(joints2, delegate(KeyValuePair<String, int> val) { return val.Key == joint.Key; });

                if (p.Key == null)
                {
                    weight += joint.Value;
                }
                else
                {
                    weight += Math.Abs(p.Value - joint.Value);
                }
            }

            // joints2にしか入っていない関節を足す
            foreach (KeyValuePair<String, int> joint in joints2)
            {
                KeyValuePair<String, int> p = Array.Find(joints1, delegate(KeyValuePair<String, int> val) { return val.Key == joint.Key; });

                if (p.Key == null)
                {
                    weight += joint.Value;
                }
            }

            return weight;
        }
    }
}

