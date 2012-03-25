using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUInsatsu.Motion
{
    /// <summary>
    /// モーションのパターンを示す識別子です。
    /// </summary>
    class Key
    {
        public int Distance { get; set; }

        private String keyString;
        public String KeyString
        {
            get { return keyString; }
            set
            {
                if (keyString == null) throw new NullReferenceException("Key is null.");
                if (keyString.Length <= 0) throw new ArgumentException("key is empty.");
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


    }
}
