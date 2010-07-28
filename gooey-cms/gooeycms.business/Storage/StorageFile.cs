using System;

namespace Gooeycms.Business.Storage
{
    public struct StorageFile
    {
        public Uri Uri { get; set; }
        public String Filename { get; set; }
        public String ContentType { get; set; }
        public byte[] Data { get; set; }

        public String Url
        {
            get { return this.Uri.ToString(); }
        }
    }
}
