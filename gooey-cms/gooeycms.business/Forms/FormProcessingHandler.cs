using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Handler;

namespace Gooeycms.Business.Forms
{
    public class FormProcessingHandler : BaseHttpHandler
    {
        protected override void Process(System.Web.HttpContext context)
        {
            context.Response.Redirect("http://www.msnbc.com");          
        }
    }
}
