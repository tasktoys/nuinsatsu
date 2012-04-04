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

        public bool Put(Key docKey, FileInfo file)
        {
            if (docKey == null) throw new NullReferenceException("doc key is null");
            if (file == null)   throw new NullReferenceException("file is null");
            return Put(docKey, new Key("0"), file);
        }

        public bool Put(Key docKey, Key passKey, FileInfo file)
        {
            if (docKey == null) throw new NullReferenceException("doc key is null");
            if (passKey == null)throw new NullReferenceException("pass key is null");
            if (file == null)   throw new NullReferenceException("file is null");

            String ext = GetExtension(file);
            file.MoveTo(DOC_DIR + "\\" + docKey.KeyString + "-" + passKey.KeyString + "." + ext);
            return true;
        }

        public FileInfo Get(Key docKey)
        {
            if (docKey == null)
            {
                throw new NullReferenceException("doc key is null");
            }
            Key passKey = new Key("0");

            return Get(docKey, passKey);
        }

        public FileInfo Get(Key docKey, Key passKey)
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

        public FileInfo[] GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Exists(Key docKey)
        {
            if (docKey == null) throw new NullReferenceException("doc key is null");

            String[] fileNames = Directory.GetFiles(DOC_DIR, docKey + "-*", SearchOption.AllDirectories);
            if (fileNames.Length <= 0)
                return false;
            else
                return true;
        }

        public Key GetFaceKey(Key docKey)
        {
            throw new NotImplementedException();
        }

        public int GetDocumentCount()
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public Key GetNearestDocument(Key docKey)
        {
            List<Key> keyList = GetRegisteredKeyList();

            DistanceCalculator Calulator = DistanceCalculator.CreateInstance();

            Key targetKey = Calulator.GetNearestDocumentKey(docKey, keyList);

            return targetKey;
        }

        public bool IsPassRequired(Key docKey)
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
        private List<Key> GetRegisteredKeyList()
        {
            try
            {
                // documentsディレクトリ以下のファイルパスを取得
                String[] filePasses = Directory.EnumerateFiles(DOC_DIR).ToArray();
                // ファイルパスからキー文字列を抽出
                String[] keyStrings = filePasses.Select(GetKey).ToArray();

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
        private String GetKey(String filePass)
        {
            int lastIndex = filePass.LastIndexOf("\\");
            int indexOf = filePass.IndexOf('-');

            String s = filePass.Substring(lastIndex + 1, indexOf - lastIndex - 1);

            return s;
        }

        /// <summary>
        /// ファイルから拡張子を抽出します。
        /// </summary>
        /// <param name="file">ファイル</param>
        /// <returns>拡張子</returns>
        private String GetExtension(FileInfo file)
        {
            String fileExt = file.Name;
            int point = file.Name.LastIndexOf(".");
            if (point != -1)
                return fileExt.Substring(point + 1);
            return fileExt;
        }
    }
}
