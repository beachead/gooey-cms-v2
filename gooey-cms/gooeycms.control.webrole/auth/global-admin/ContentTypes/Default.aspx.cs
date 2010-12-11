using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;

namespace Gooeycms.Webrole.Control.auth.global_admin.ContentTypes
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
/*            HiddenField field = GridViewHelper.FindControl<HiddenField>(e.CommandSource, "ContentTypeId");
            switch (e.CommandName)
            {
                case "deleteid":
                    DeleteContentType(field.Value);
                    break;
            }*/
        }


        protected void AddContentType_Click(object sender, EventArgs e)
        {
            try
            {
                CmsContentType type = new CmsContentType();
                type.IsGlobalType = true;
                type.Name = this.ContentName.Text;
                type.Description = this.ContentDescription.Text;
                type.IsFileType = this.ContentFileYes.Checked;
                type.IsEditorVisible = this.ContentEditorYes.Checked;

                ContentManager.Instance.AddContentType(null,type);
                Response.Redirect("./ContentTypeFields.aspx?tid=" + Server.UrlEncode(type.Guid), true);
            }
            catch (Exception ex)
            {
                Status.ForeColor = System.Drawing.Color.Red;
                Status.Text = ex.Message;
            }
        }
    }
}