using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Util
{
    public static class UtcDateTime
    {
        public static DateTime Now
        {
            get { return DateTime.Now.ToUniversalTime(); }
        }
    }
}
