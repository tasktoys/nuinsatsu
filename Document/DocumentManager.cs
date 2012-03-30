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
        private DocumentFileIO io = null;

        /// <summary>
        /// コンストラクタです.
        /// </summary>
        private DocumentManager()
        {
            selectTypeIO();
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
        /// 指定されたドキュメントキーにはパスキーが必要かどうか判定します.
        /// </summary>
        /// <param name="docKey">ドキュメントキー</param>
        /// <returns>パスキーが必要ならtrue、不要ならfalse</returns>
        public bool IsPassRequired(Key docKey)
        {
            return io.IsPassRequired(docKey);
        }

        /// <summary>
        /// モーションから生成したキーを与えて、それに最も近いドキュメントのキーを取得します.
        /// </summary>
        /// <param name="docKey">モーションから生成したドキュメントキー</param>
        /// <returns>登録済ドキュメントキー</returns>
        public Key GetNearestDocumentKey(Key docKey)
        {
            return io.GetNearestDocument(docKey);
        }

        /// <summary>
        /// DocumentFileIOの種類をプロパティから設定します。
        /// </summary>
        private void selectTypeIO()
        {
            Config config = Config.Load();

            if (config.DocumentIOType == "local")
            {
                io = new LocalFileIO();
            }
            else
            {
                io = new LocalFileIO();
            }
        }

    }
}
