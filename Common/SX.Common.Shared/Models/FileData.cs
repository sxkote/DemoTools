using SX.Common.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SX.Common.Shared.Models
{
    public class FileData
    {
        private FileName _filename;
        private byte[] _data;

        public FileName FileName { get => _filename; }

        public byte[] Data { get => _data; } 

        public FileType Type { get => _filename.Type; }

        public string GetHashMD5() => _data == null ? "" : CalculateMD5(_data);

        public long GetSize() => _data == null ? 0 : _data.Length;

        public bool IsCompressed() => this.FileName.Type == FileType.ZIP;

        public FileData(FileName filename, byte[] data = null)
        {
            _filename = filename;
            _data = data;
        }

        static public string CalculateMD5(byte[] data)
        {
            byte[] hash;

            using (var md5 = System.Security.Cryptography.MD5.Create())
                hash = md5.ComputeHash(data);

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sBuilder.Append(hash[i].ToString("x2"));

            return sBuilder.ToString();
        }

        static public implicit operator byte[](FileData data)
        {
            return data?.Data;
        }

        public override string ToString()
        {
            return this.FileName?.Name ?? "";
        }
    }
}
