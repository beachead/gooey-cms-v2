using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Storage
{
    [Serializable]
    public abstract class SortableAssetFile : IComparable<SortableAssetFile>
    {
        public const String DefaultSeparator = "-";
        public const String JavascriptExtension = ".js";
        public const String CssExtension = ".css";

        public abstract String Separator { get; }
        public abstract String Extension { get; }

        public Boolean IsEnabled { get; set; }
        public Int32 SortOrder { get; set; }
        public String Content { get; set; }
        public String FullName { get; set; }
        public DateTime LastModified { get; set; }

        public String Name
        {
            get { return FullName; }
            set { this.FullName = value; }
        }

        public int CompareTo(SortableAssetFile other)
        {
            return (this.SortOrder.CompareTo(other.SortOrder));
        }
    }
}
