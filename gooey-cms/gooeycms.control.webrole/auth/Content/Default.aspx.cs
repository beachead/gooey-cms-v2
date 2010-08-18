using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;

namespace Gooeycms.Webrole.Control.auth.Content
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<CmsContentType> types = ContentManager.Instance.GetContentTypes();
                foreach (CmsContentType type in types)
                {
                    ListItem item = new ListItem(type.Name, type.Guid);
                    this.LstContentTypes.Items.Add(item);
                }
            }
        }
    }
}