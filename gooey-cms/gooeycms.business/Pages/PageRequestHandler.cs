using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Gooeycms.Business.Pages
{
    public class PageRequestHandler : Page
    {
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("THIS IS A TEST " + Request.RawUrl);
            base.Render(writer);
        }
    }
}
