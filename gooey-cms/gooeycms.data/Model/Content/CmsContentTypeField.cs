using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Content
{
    public class CmsContentTypeField : BasePersistedItem
    {
        public const String Textbox = "textbox";
        public const String Textarea = "textarea";
        public const String Dropdown = "dropdown";
        public const String Datetime = "datetime";

        private Boolean isSystemDefault = false;

        public virtual CmsContentType Parent { get; set; }
        public virtual int Position { get; set; }
        public virtual Boolean IsRequired { get; set; }
        public virtual String SystemName { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String FieldType { get; set; }
        public virtual String ObjectType { get; set; }
        public virtual int Columns { get; set; }
        public virtual int Rows { get; set; }
        public virtual String _SelectOptions { get; set; }

        public virtual Boolean IsSystemDefault
        {
            get { return this.isSystemDefault; }
            set { this.isSystemDefault = value; }
        }

        public virtual Boolean IsUserField
        {
            get { return !IsSystemDefault; }
        }

        public virtual IList<String> SelectOptions
        {
            get 
            { 
                List<String> result = new List<string>();
                String [] items = this._SelectOptions.Split('\n');
                foreach (String item in items)
                    result.Add(item.Trim());

                return result;
            }
        }

    }
}
