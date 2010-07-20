using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Security.Application;

namespace Beachead.Core.Markup.Forms.Fields
{
    public abstract class BaseFormField : IFormField
    {
        private String id;
        public String Markup { get; set; }
        public String Description { get; set; }
        public Boolean IsRequired { get; set; }
        public Boolean IsHidden { get; set; }
        public string Id
        {
            get { return this.id; }
            set
            {
                this.id = value.Replace(" ", "_");
            }
        }

        public String CssRequired
        {
            get { return (this.IsRequired) ? "required" : ""; }
        }

        public String CssHidden
        {
            get { return (this.IsHidden) ? "display:none;" : ""; }
        }

        public String HtmlEncode(String text)
        {
            return AntiXss.HtmlEncode(text);
        }

        public abstract String Format();
    }
}
