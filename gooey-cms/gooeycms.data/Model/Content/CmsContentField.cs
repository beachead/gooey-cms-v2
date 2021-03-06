﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Content
{
    [Serializable]
    public class CmsContentField : BasePersistedItem
    {
        public virtual CmsContent Parent { get; set; }
        public virtual String ObjectType { get; set; }
        public virtual String Name { get; set; }
        public virtual String Value { get; set; }

        public virtual IComparable GetValueAsComparable()
        {
            IComparable result = null;
            if (ObjectType.Equals("System.DateTime"))
                result = DateTime.Parse(this.Value);
            else
                result = this.Value;

            return result;
        }

        public virtual Object AsObject()
        {
            Object result = null;
            if (ObjectType.Equals("System.DateTime"))
                result = String.Format("{0:MM/dd/yyyy}",DateTime.Parse(this.Value));
            else
                result = this.Value.Trim();

            return result;
        }

        public virtual String AsString()
        {
            String result = null;
            if (ObjectType.Equals("System.DateTime"))
                result = String.Format("{0:MM/dd/yyyy}", DateTime.Parse(this.Value));
            else
                result = this.Value.Trim();

            return result;
        }
    }
}
