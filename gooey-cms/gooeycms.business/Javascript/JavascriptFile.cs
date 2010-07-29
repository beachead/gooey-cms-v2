using System;

namespace Gooeycms.Business.Javascript
{
    public class JavascriptFile
    {
        public const String Separator = "-";
        public const String Extension = ".js";

        public Boolean IsEnabled { get; set; }
        public String Content { get; set; }
        public String FullName { get; set; }

        public String Name
        {
            get { return FullName; }
            set { this.FullName = value; }
        }
    }
}
