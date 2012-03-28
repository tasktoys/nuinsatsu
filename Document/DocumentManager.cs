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

    }
}
