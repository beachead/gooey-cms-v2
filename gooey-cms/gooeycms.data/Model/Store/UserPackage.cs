using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Store
{
    public class UserPackage : BasePersistedItem
    {
        public virtual String UserGuid { get; set; }
        public virtual String PackageGuid { get; set; }
        public virtual String PackageTitle { get; set; }
        public virtual String PackageType { get; set; }
    }
}
