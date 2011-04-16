using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Import
{
    public class CrawlerEntry
    {
        public String ContentType { get; set; }
        public Int32 ContentLength { get; set; }
        public String Depth { get; set; }
        public Uri @Uri { get; set; }
    }
}
