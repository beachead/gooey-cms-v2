using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Store
{
    public enum PackageTypes
    {
        Site,
        Theme
    }

    public class Package : BasePersistedItem
    {
        public virtual String OwnerSubscriptionId { get; set; }
        public virtual String Guid { get; set; }
        public virtual String PackageTypeString { get; set; }
        public virtual String Title { get; set; }
        public virtual String Features { get; set; }
        public virtual Int32 PageCount { get; set; }
        public virtual String Category { get; set; }
        public virtual Double Price { get; set; }
        public virtual Boolean IsApproved { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Approved { get; set; }

        public virtual IList<String> FeatureList
        {
            get
            {
                List<String> result = new List<String>();
                if (Features != null)
                {
                    String[] items = Features.Split('\n');
                    foreach (String item in items)
                    {
                        if (!String.IsNullOrEmpty(item))
                            result.Add(item.Trim());
                    }
                }

                return result;
            }
        }
    }
}
