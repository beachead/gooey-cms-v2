using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Membership
{
    class MembershipException : Exception
    {
        private string p;

        public MembershipException(string message) : base(message)
        {
        }
    }
}
