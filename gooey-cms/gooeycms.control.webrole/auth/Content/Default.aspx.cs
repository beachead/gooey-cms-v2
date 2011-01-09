using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;
using Gooeycms.Business.Web.Microsoft;

namespace Gooeycms.Webrole.Control.auth.Content
{
    public partial class Default : App_Code.ValidatedHelpPage
    {
        protected override void OnPageLoad(object sender, EventArgs e)
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


        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            HiddenField field = GridViewHelper.FindControl<HiddenField>(e.CommandSource, "ContentId");
            switch (e.CommandName)
            {
                case "deleteid":
                    Delete(field.Value);
                    break;
            }
        }

        private void Delete(String guid)
        {
            CmsContent content = ContentManager.Instance.GetContent(guid);
            ContentManager.Instance.Delete(content);

            this.ContentTable.DataBind();
        }
    }
}