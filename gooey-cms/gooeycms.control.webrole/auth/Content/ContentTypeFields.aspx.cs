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
    public partial class ContentTypeFields : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void FieldType_Change(object sender, EventArgs e)
        {
            this.TextAreaFields.Visible = false;
            this.DropdownFields.Visible = false;
            switch (this.FieldType.SelectedValue.ToLower())
            {
                case "textarea":
                    this.TextAreaFields.Visible = true;
                    break;
                case "dropdown":
                    this.DropdownFields.Visible = true;
                    break;
            }
        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            HiddenField field = GridViewHelper.FindControl<HiddenField>(e.CommandSource, "ExistingFieldId");
            switch (e.CommandName)
            {
                case "deleteid":
                    Delete(Int32.Parse(field.Value));
                    break;
            }
        }

        private void Delete(int primaryKey)
        {
            String guid = Request.QueryString["tid"];
            ContentManager.Instance.DeleteField(guid, primaryKey);

            this.FieldTable.DataBind();
        }

        /// <summary>
        /// Adds a new field to the content type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Add_Click(object sender, EventArgs e)
        {
            String guid = Request.QueryString["tid"];
            CmsContentType type = ContentManager.Instance.GetContentType(guid);

            CmsContentTypeField field = new CmsContentTypeField();
            field.Parent = type;
            field.SystemName = this.TxtSystemName.Text;
            field.Name = this.TxtName.Text;
            field.Description = this.Description.Text;
            field.IsRequired = this.ChkRequiredField.Checked;
            this.SetFieldProperties(field);

            ContentManager.Instance.AddContentTypeField(type, field);

            this.FieldType.SelectedIndex = 0;
            this.TxtName.Text = "";
            this.TxtSystemName.Text = "";
            this.Description.Text = "";

            this.FieldTable.DataBind();
        }

        private void SetFieldProperties(CmsContentTypeField field)
        {
            field.FieldType = this.FieldType.SelectedValue;

            //Set any specific attributes for field types that are unique
            if (this.FieldType.SelectedValue.ToLower().Equals("textarea"))
            {
                field.Columns = Int32.Parse(this.Cols.Text);
                field.Rows = Int32.Parse(this.Rows.Text);
            }
            if (this.FieldType.SelectedValue.ToLower().Equals("dropdown"))
            {
                field._SelectOptions = this.Options.Text;
            }

            //set the object type
            switch (field.FieldType.ToLower())
            {
                case CmsContentTypeField.Datetime:
                    field.ObjectType = typeof(System.DateTime).FullName;
                    break;
                default:
                    field.ObjectType = typeof(System.String).FullName;
                    break;
            }
        }
    }
}