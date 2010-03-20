using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model
{
    public abstract class BasePersistedItem
    {
        public virtual int Id { get; set; }

        public override bool Equals(object obj)
        {
            return ((BasePersistedItem)obj).Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return (this.Id.GetHashCode());
        }
    }
}
