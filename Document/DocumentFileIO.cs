using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;

namespace NUInsatsu.Document
{
    /// <summary>
    /// ドキュメント入出力を抽象化するインタフェースです.
    /// </summary>
    interface DocumentFileIO
    {
        /// <summary>
        /// モーションから生成されたキーを与えて、それに最も近いドキュメントのキーを取得します。
        /// </summary>
        /// <param name="docKey">モーションから生成他ドキュメントキー</param>
        /// <returns>登録済みドキュメントキー</returns>
        Key GetNearestDocument(Key docKey);
    }
}
