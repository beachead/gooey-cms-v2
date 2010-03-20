using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beachead.Persistence
{
    public class PersistenceException : Exception
    {
        public PersistenceException() : base() { }
        public PersistenceException(String msg, Exception e) : base(msg, e) { }
        public PersistenceException(Exception e) : base(e.Message,e) { }
        public PersistenceException(String msg) : base(msg) { }
    }
}
