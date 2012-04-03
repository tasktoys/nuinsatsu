using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;
using System.IO;

namespace NUInsatsu.Document
{
    /// <summary>
    /// ドキュメントの入出力を統一的に扱うためのインタフェースです.
    /// </summary>
    class DocumentManager
    {
        private static DocumentManager instance = new DocumentManager();
        private DocumentFileIO io = null;
        private PrintController printer = null;
        private ScanController scanner = null;

        /// <summary>
        /// コンストラクタです.
        /// </summary>
        private DocumentManager()
        {
            SelectTypeIO();
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
        private void SelectTypeIO()
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

        /// <summary>
        /// パスモーションなしで印刷を行います.
        /// </summary>
        /// <param name="docKey">ドキュメント識別キー</param>
        public void Print(Key docKey)
        {
            printer = new PrintController();
            FileInfo fileInfo = io.Get(docKey);

            printer.Print(fileInfo);
        }

        /// <summary>
        /// スキャン処理を行います。
        /// </summary>
        public FileInfo Scan()
        {
            scanner = new ScanController();
            return scanner.Scan();
        }

        /// <summary>
        /// ドキュメント入出力クラスのインスタンスを取得します
        /// </summary>
        /// <returns>DocumentFileIOのインスタンス</returns>
        public DocumentFileIO GetIOInstance()
        {
            return io;
        }

    }
}
