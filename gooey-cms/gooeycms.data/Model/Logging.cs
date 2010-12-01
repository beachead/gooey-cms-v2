using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model
{
    public class Logging : BasePersistedItem
    {
        public virtual DateTime Inserted { get; set; }
        public virtual String EventType { get; set; }
        public virtual String EventMessage { get; set; }
        public virtual String ErrorCode { get; set; }
        public virtual String Exception { get; set; }
    }

    public class LoggingDao : BaseDao
    {
    }
}
