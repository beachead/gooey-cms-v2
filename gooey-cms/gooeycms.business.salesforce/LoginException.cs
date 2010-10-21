using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gooeycms.business.salesforce
{
    /// <summary>
    /// Exception which is thrown on any login error
    /// </summary>
    public class LoginException : Exception
    {
        private LoginStatus status;

        public LoginException(String msg) : base(msg) { }
        public LoginException(String msg, Exception e) : base(msg, e) { }

        public LoginException(String msg, LoginStatus status)
            : base(msg)
        {
            this.status = status;
        }

        public LoginStatus Status
        {
            get { return this.status; }
        }
    }
}
