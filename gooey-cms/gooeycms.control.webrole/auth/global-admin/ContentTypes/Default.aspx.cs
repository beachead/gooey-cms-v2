using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;
using Gooeycms.Business.Web.Microsoft;

namespace Gooeycms.Webrole.Control.auth.global_admin.ContentTypes
{
    public partial class Default : System.Web.UI.Page
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
                case "editid":
                    EditContentType(field.Value);
                    break;
            }
        }

        private void EditContentType(String guid)
        {
            CmsContentType contentType = ContentManager.Instance.GetContentType(Data.Guid.New(guid));

            this.ExistingContentTypeGuid.Value = guid;
            this.ContentDispayName.Text = contentType.DisplayName;
            this.ContentDescription.Text = contentType.Description;

            this.ContentFileYes.Checked = contentType.IsFileType;
            this.ContentFileNo.Checked = !contentType.IsFileType;

            this.ContentEditorYes.Checked = contentType.IsEditorVisible;
            this.ContentEditorNo.Checked = !contentType.IsEditorVisible;

            this.ContentSystemName.Text = contentType.Name;
            this.ContentSystemName.Enabled = false;

            this.BtnAddContentType.Text = "Save";
        }

        private void DeleteContentType(String guid)
        {
            CmsContentType contentType = ContentManager.Instance.GetContentType(Data.Guid.New(guid));
            ContentManager.Instance.Delete(contentType);

            this.ExistingContentTypes.DataBind();
        }

        protected void LnkAddNewType_Click(Object sender, EventArgs e)
        {
            this.ExistingContentTypeGuid.Value = String.Empty;
            this.ContentDispayName.Text = String.Empty;
            this.ContentDescription.Text = String.Empty;

            this.ContentFileYes.Checked = false;
            this.ContentFileNo.Checked = true;

            this.ContentEditorYes.Checked = true;
            this.ContentEditorNo.Checked = false;

            this.ContentSystemName.Text = String.Empty;
            this.ContentSystemName.Enabled = true;

            this.BtnAddContentType.Text = "Add";
        }


        protected void AddContentType_Click(object sender, EventArgs e)
        {
            try
            {
                String guid = this.ExistingContentTypeGuid.Value;
                CmsContentType type = ContentManager.Instance.GetContentType(guid);

                Boolean isNew = false;
                if (type == null)
                {
                    type = new CmsContentType();
                    isNew = true;
                }

                type.IsGlobalType = true;
                type.DisplayName = this.ContentDispayName.Text;
                type.Name = this.ContentSystemName.Text.Replace(" ", "_");
                type.Description = this.ContentDescription.Text;
                type.IsFileType = this.ContentFileYes.Checked;
                type.IsEditorVisible = this.ContentEditorYes.Checked;

                if (isNew)
                    ContentManager.Instance.AddContentType(type);
                else
                    ContentManager.Instance.Save(type);

                if (isNew)
                {
                    CmsContentTypeField field = new CmsContentTypeField();
                    field.Name = "Title";
                    field.ObjectType = "System.String";
                    field.IsRequired = true;
                    field.Description = "Title";
                    field.SystemName = "title";
                    field.FieldType = CmsContentTypeField.Textbox;

                    ContentManager.Instance.AddContentTypeField(type, field);
                }

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