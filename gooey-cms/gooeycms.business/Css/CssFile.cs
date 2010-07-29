﻿using System;

namespace Gooeycms.Business.Css
{
    public class CssFile 
    {
        public const String Separator = "-";
        public const String Extension = ".css";

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
