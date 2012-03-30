using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Motion
{
    /// <summary>
    /// モーションのパターンを示す識別子です。
    /// </summary>
    class Key : IComparable
    {
        public int Distance { get; set; }

        private String keyString;
        public String KeyString
        {
            get { return keyString; }
            set
            {
                if (keyString == value) throw new NullReferenceException("Key is null.");
                if (value.Length <= 0) throw new ArgumentException("key is empty.");
                keyString = value;
            }
        }

        /// <summary>
        /// 空のキーオブジェクトを生成します。
        /// </summary>
        public Key() { }

        /// <summary>
        /// キー文字列を指定してキーオブジェクトを生成します。
        /// </summary>
        /// <param name="key">キー文字列</param>
        public Key(String keyString)
        {
            this.keyString = keyString;
        }

        public int CompareTo(object obj)
        {
            // ヌルより大きい
            if (obj == null)
            {
                return 1;
            }

            // 他の型とは比較できない
            if (this.GetType() != obj.GetType())
            {
                throw new ArgumentException("別の型とは比較できません。", "obj");
            }

            return this.Distance - ((Key)obj).Distance;
        }

        public override string ToString()
        {
            return KeyString;
        }
    }
}
