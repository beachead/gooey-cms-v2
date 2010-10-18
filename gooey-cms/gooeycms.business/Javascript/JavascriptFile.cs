using System;

namespace Gooeycms.Business.Javascript
{
    [Serializable]
    public class JavascriptFile : IComparable<JavascriptFile>
    {
        public const String Separator = "-";
        public const String Extension = ".js";

        public Boolean IsEnabled { get; set; }
        public Int32 SortOrder { get; set; }
        public String Content { get; set; }
        public String FullName { get; set; }

        public String Name
        {
            get { return FullName; }
            set { this.FullName = value; }
        }

        public int CompareTo(JavascriptFile other)
        {
            return (this.SortOrder.CompareTo(other.SortOrder));
        }
    }
}
