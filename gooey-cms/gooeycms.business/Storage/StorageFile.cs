using System;
using System.IO;

namespace Gooeycms.Business.Storage
{
    public struct StorageFile
    {
        public Uri Uri { get; set; }
        public String Filename { get; set; }
        public String Name
        {
            get
            {
                FileInfo info = new FileInfo(Filename);
                return Filename.Replace(info.Extension, "");
            }
        }

        public String ContentType { get; set; }
        public byte[] Data { get; set; }

        public String Url
        {
            get { return this.Uri.ToString(); }
        }

        public String ThumbnailUrl
        {
            get 
            {
                return Url.Replace(Filename, Name + "-thumb.jpg");
            }
        }
    }
}
