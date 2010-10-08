using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Storage
{
    [Serializable]
    public class BlobSnapshot
    {
        public DateTime? SnapshotTime { get; set; }
        public Uri Uri { get; set; }
        public String Filename { get; set; }
        public String ContentType { get; set; }
    }
}
