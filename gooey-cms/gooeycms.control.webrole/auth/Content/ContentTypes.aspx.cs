using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Web.Microsoft;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.Content
{
    public partial class ContentTypes : App_Code.ValidatedHelpPage
    {
        protected override void OnPageLoad(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                IList<CmsContentType> types = ContentManager.Instance.GetGlobalContentTypes();
                foreach (CmsContentType type in types)
                {
                    ListItem item = new ListItem(type.DisplayName, type.Guid);
                    this.LstGlobalTypes.Items.Add(item);
                }
            }
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

            this.BtnAddContent.Text = "Save";
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

            this.BtnAddContent.Text = "Add";
        }

        protected void BtnDuplicate_Click(Object sender, EventArgs e)
        {
            String guid = this.LstGlobalTypes.SelectedValue;
            CmsContentType systemType = ContentManager.Instance.GetContentType(guid);

            ContentManager.Instance.Duplicate(CurrentSite.Guid, systemType);

            this.ExistingContentTypes.DataBind();
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

                type.DisplayName = this.ContentDispayName.Text;
                type.Name = this.ContentSystemName.Text.Replace(" ","_");
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

                    //Set the default title to the one we just created
                    type.TitleFieldName = field.SystemName;
                    ContentManager.Instance.Save(type);
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