using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;
using System.IO;

namespace NUInsatsu.Document
{
    /// <summary>
    /// ドキュメント入出力を抽象化するインタフェースです.
    /// </summary>
    interface DocumentFileIO
    {
        /// <summary>
        /// ドキュメントキー、ファイルの実体をドキュメント管理に登録します。
        /// </summary>
        /// <param name="docKey">ドキュメントキー</param>
        /// <param name="file">ドキュメントファイル情報</param>
        /// <returns>成功でtrue,失敗でfalse</returns>
        /// <exception cref="NullReferenceException">引数が空の場合</exception>
        bool Put(Key docKey, FileInfo file);

        /// <summary>
        /// ドキュメントキー、パスキー、パスファイルの実体をドキュメント管理に登録します。
        /// </summary>
        /// <param name="docKey">ドキュメントキー</param>
        /// <param name="passKey">パスキー</param>
        /// <param name="file">ドキュメントファイル情報</param>
        /// <returns>成功でtrue,失敗でfalse</returns>
        /// <exception cref="NullReferenceException">引数が空の場合</exception>
        bool Put(Key docKey, Key passKey, FileInfo file);

        /// <summary>
        /// ドキュメントキーを指定しファイルの実体を取得します。
        /// </summary>
        /// <param name="docKey">ドキュメントキー</param>
        /// <returns>ドキュメントファイル情報</returns>
        /// <exception cref="NullReferenceException">引数が空の場合</exception>
        /// <exception cref="DocumentNotFoundException">対象のドキュメントが見つからなかった場合</exception>
        FileInfo Get(Key docKey);

        /// <summary>
        /// ドキュメントキーとパスキーを指定しファイルの実体を取得します。
        /// </summary>
        /// <param name="docKey">ドキュメントキー</param>
        /// <param name="passKey">パスキー</param>
        /// <returns>ドキュメントファイル情報</returns>
        FileInfo Get(Key docKey, Key passKey);

        /// <summary>
        /// 全ての登録済みドキュメントのリストを取得します。
        /// </summary>
        /// <returns>ドキュメント情報一覧</returns>
        FileInfo[] GetAll();

        /// <summary>
        /// モーションから生成されたキーを与えて、それに最も近いドキュメントのキーを取得します。
        /// </summary>
        /// <param name="docKey">モーションから生成他ドキュメントキー</param>
        /// <returns>登録済みドキュメントキー</returns>
        /// <exception cref="DocumentNotFoundException">対称のドキュメントが見つからなかった場合</exception>
        Key GetNearestDocument(Key docKey);

        /// <summary>
        /// 指定したドキュメントキーのドキュメントが惣菜するか判定します
        /// </summary>
        /// <param name="docKey">ドキュメントキー</param>
        /// <returns>存在すればtrue,存在しなければfalse</returns>
        bool Exists(Key docKey);

        /// <summary>
        /// パスキーが必要なドキュメントか判定します。
        /// </summary>
        /// <param name="docKey">ドキュメントキー</param>
        /// <returns>パスキーが必要な場合はtrue,必要ない場合はfalse</returns>
        /// <exception cref="NullReferenceException">引数が空の場合</exception>
        /// <exception cref="DocumentNotFoundException">キーに該当するドキュメントが存在しない場合</exception>
        bool IsPassRequired(Key docKey);

        /// <summary>
        /// 指定したドキュメントキーの顔認証データを取得します。
        /// </summary>
        /// <param name="docKey">ドキュメントキー</param>
        /// <returns>顔認証キー</returns>
        /// <exception cref="NullReferenceException">ドキュメントキーが空の場合</exception>
        /// <exception cref="DocumentNotFoundException">キーに該当するドキュメントが存在しない場合</exception>
        Key GetFaceKey(Key docKey);

        /// <summary>
        /// 登録済みドキュメントの総数を取得します。
        /// </summary>
        /// <returns>総数</returns>
        int GetDocumentCount();

        /// <summary>
        /// 全ての登録済みドキュメントを削除します。
        /// </summary>
        void DeleteAll();
    }
}
