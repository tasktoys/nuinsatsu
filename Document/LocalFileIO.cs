using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUInsatsu.Motion;
using System.IO;

namespace NUInsatsu.Document
{
    /// <summary>
    /// ドキュメント入出力インタフェースDocumentManagerのローカルファイルシステム入出力による実装です。
    /// </summary>
    class LocalFileIO : DocumentFileIO
    {
        const String DOC_DIR = ".\\documents";

        bool DocumentFileIO.Put(Key docKey, FileInfo file)
        {
            throw new NotImplementedException();
        }

        bool DocumentFileIO.Put(Key docKey, Key passKey, FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }

        FileInfo DocumentFileIO.Get(Key docKey)
        {
            if (docKey == null)
            {
                throw new NullReferenceException("doc key is null");
            }
            Key passKey = new Key("0");

            // 明示的にインタフェースを実装しているので、
            // 同じクラス内でもインタフェース経由でしかメソッドを呼ぶことができない
            DocumentFileIO io = this;

            return io.Get(docKey, passKey);
        }

        FileInfo DocumentFileIO.Get(Key docKey, Key passKey)
        {
            if (docKey == null)
            {
                throw new NullReferenceException("doc key is null");
            }
            if (passKey == null)
            {
                throw new NullReferenceException("pass key is null");
            }

            String[] targetFileNames = Directory.GetFiles(DOC_DIR, docKey + "-" + passKey + "*.*", SearchOption.AllDirectories);

            if (targetFileNames.Length == 0)
            {
                throw new DocumentNotFoundException("File not found:" + docKey);
            }

            return new FileInfo(targetFileNames.First());
        }

        FileInfo[] DocumentFileIO.GetAll()
        {
            throw new NotImplementedException();
        }

        bool DocumentFileIO.Exists(Key docKey)
        {
            throw new NotImplementedException();
        }

        Key DocumentFileIO.GetFaceKey(Key docKey)
        {
            throw new NotImplementedException();
        }

        int DocumentFileIO.GetDocumentCount()
        {
            throw new NotImplementedException();
        }

        void DocumentFileIO.deleteAll()
        {
            throw new NotImplementedException();
        }

        Key DocumentFileIO.GetNearestDocument(Key docKey)
        {
            List<Key> keyList = getRegisteredKeyList();

            Key targetKey = DistanceUtility.GetNearestDocumentKey(docKey, keyList);

            return targetKey;
        }

        bool DocumentFileIO.IsPassRequired(Key docKey)
        {
            if (docKey == null)
            {
                throw new NullReferenceException("doc key is null");
            }

            String[] fileNames = Directory.GetFiles(DOC_DIR, docKey + "-*", SearchOption.AllDirectories);
            if (fileNames.Length <= 0)
            {
                String str = String.Format("Requested key {0} is not found.", docKey.KeyString);
                throw new DocumentNotFoundException(str);
            }
            else
            {
                String fileName = fileNames.First();
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Name.StartsWith(docKey.KeyString + "-0"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 登録されているドキュメントのキー一覧を取得します。
        /// </summary>
        /// <remarks>
        /// ドキュメントを保存するディレクトリが無い場合、ディレクトリを生成し、空のリストを返します。
        /// </remarks>
        /// <returns>キー一覧</returns>
        private List<Key> getRegisteredKeyList()
        {
            try
            {
                // documentsディレクトリ以下のファイルパスを取得
                String[] filePasses = Directory.EnumerateFiles(DOC_DIR).ToArray();
                // ファイルパスからキー文字列を抽出
                String[] keyStrings = filePasses.Select(getKey).ToArray();

                // キー文字列からKeyクラスに変換
                return keyStrings.Select(str => new Key(str)).ToList();
            }
            catch (DirectoryNotFoundException)
            {
                // DOC_DIRで指定されているディレクトリが無い場合、ディレクトリを生成し、空のリストを返します。
                Directory.CreateDirectory(DOC_DIR);
                return new List<Key>();
            }
        }

        /// <summary>
        /// ファイルパスからキー文字列を抽出します。
        /// </summary>
        /// <param name="filePass">ファイルパス</param>
        /// <returns>キー文字列</returns>
        private String getKey(String filePass)
        {
            int lastIndex = filePass.LastIndexOf("\\");
            int indexOf = filePass.IndexOf('-');

            String s = filePass.Substring(lastIndex + 1, indexOf - lastIndex - 1);

            return s;
        }


    }
}
