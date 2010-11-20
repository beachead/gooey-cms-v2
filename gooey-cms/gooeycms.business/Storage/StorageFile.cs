using System;
using System.IO;
using System.Diagnostics;

namespace Gooeycms.Business.Storage
{
    [DebuggerDisplay("Filename = {Filename}, Uri = {Uri}, Size = {Size}")]
    [Serializable]
    public class StorageFile
    {
        public long Size { get; set; }
        public Uri Uri { get; set; }
        public String Filename { get; set; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public System.Collections.Specialized.NameValueCollection Metadata { get; set; }
        public String ContentType { get; set; }
        public byte[] Data { get; set; }

        public String Name
        {
            get
            {
                FileInfo info = new FileInfo(Filename);
                return Filename.Replace(info.Extension, "");
            }
        }

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

        public bool Exists()
        {
            return (!String.IsNullOrEmpty(this.Filename));
        }
    }
}
