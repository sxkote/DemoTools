using SX.Common.Shared.Enums;
using SX.Common.Shared.Services;
using System;
using System.IO;

namespace SX.Common.Shared.Models
{
    public class FileName
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        private FileName() { }

        public FileName(string name)
        { _name = name; }

        public string Extension
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this.Name))
                    return "";

                return (new System.IO.FileInfo(this.Name)).Extension;
            }
        }

        public string NameWithoutExtension
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this.Name))
                    return "";

                return Path.GetFileNameWithoutExtension(this.Name);
            }
        }

        public string MimeType
        {
            get { return CommonService.GetMimeMapping(this.Name); }
        }

        public FileType Type
        {
            get { return DefineType(this.Extension); }
        }

        public override string ToString() => this.Name;

        public override int GetHashCode() => this.Name.GetHashCode();

        public override bool Equals(object obj) => this.Name.Equals(obj);

        static public FileType DefineType(string extension)
        {
            string ext = extension.TrimStart('.').Trim().ToLower();
            switch (ext)
            {
                case "jpeg":
                case "jpg":
                case "bmp":
                case "png":
                case "gif":
                case "tiff":
                    return FileType.Image;

                case "avi":
                case "mpeg":
                case "mp4":
                case "wmv":
                case "mov":
                    return FileType.Video;

                case "mp3":
                case "wav":
                case "wma":
                    return FileType.Audio;

                case "pdf":
                    return FileType.PDF;

                case "doc":
                case "docx":
                case "dotx":
                    return FileType.Word;

                case "xls":
                case "xlsx":
                    return FileType.Excel;

                case "ppt":
                case "pptx":
                    return FileType.PowerPoint;

                case "txt":
                    return FileType.Text;

                case "zip":
                case "7z":
                case "rar":
                    return FileType.ZIP;

                case "xml":
                    return FileType.XML;

                case "json":
                    return FileType.JSON;

                default:
                    return FileType.File;
            }

        }

        static public implicit operator FileName(string filename)
        { return new FileName(filename); }

        static public implicit operator string(FileName filename)
        { return filename.Name; }
    }
}
