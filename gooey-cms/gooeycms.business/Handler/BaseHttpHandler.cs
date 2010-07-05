using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Gooeycms.Business.Handler
{
    public abstract class BaseHttpHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Process(context);
        }

        protected abstract void Process(HttpContext context);

        #endregion
    }
}
