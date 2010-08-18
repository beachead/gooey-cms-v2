using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Web.Microsoft;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;

namespace Gooeycms.Webrole.Control.auth.Content
{
    public partial class ContentTypes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            HiddenField field = GridViewHelper.FindControl<HiddenField>(e.CommandSource, "ContentTypeId");
            switch (e.CommandName)
            {
                case "deleteid":
                    DeleteContentType(field.Value);
                    break;
            }
        }

        private void DeleteContentType(String guid)
        {
            CmsContentType contentType = ContentManager.Instance.GetContentType(Data.Guid.New(guid));
            ContentManager.Instance.Delete(contentType);

            this.ExistingContentTypes.DataBind();
        }

        protected void AddContentType_Click(object sender, EventArgs e)
        {
            try
            {
                CmsContentType type = new CmsContentType();
                type.Name = this.ContentName.Text;
                type.Description = this.ContentDescription.Text;
                type.IsFileType = this.ContentFileYes.Checked;
                type.IsEditorVisible = this.ContentEditorYes.Checked;

                ContentManager.Instance.AddContentType(type);
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