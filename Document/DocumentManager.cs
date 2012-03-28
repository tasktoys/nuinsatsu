using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;

namespace NUInsatsu.Document
{
    /// <summary>
    /// ドキュメントの入出力を統一的に扱うためのインタフェースです.
    /// </summary>
    class DocumentManager
    {
        private static DocumentManager instance = new DocumentManager();

        /// <summary>
        /// コンストラクタです.
        /// </summary>
        private DocumentManager()
        {

        }

        /// <summary>
        /// インスタンスを取得します.
        /// </summary>
        /// <returns>インスタンス</returns>
        public static DocumentManager GetInstance()
        {
            if (instance == null)
            {
                instance = new DocumentManager();
            }

            return instance;
        }

        /// <summary>
        /// モーションから生成したキーを与えて、それに最も近いドキュメントのキーを取得します.
        /// </summary>
        /// <param name="docKey">モーションから生成したドキュメントキー</param>
        /// <returns>登録済ドキュメントキー</returns>
        public Key GetNearestDocumentKey(Key docKey)
        {
        }

    }
}
