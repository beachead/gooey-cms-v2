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
    public partial class ContentTypeFields : App_Code.ValidatedHelpPage
    {
        protected override void OnPageLoad(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String guid = Request.QueryString["tid"];
                CmsContentType type = ContentManager.Instance.GetContentType(guid);
                this.LblContentTypeName.Text = type.Name + " (" + type.Description + ")";

                IList<CmsContentTypeField> fields = ContentManager.Instance.GetContentTypeFields(guid);

                ListItem defaultItem = new ListItem("...select...", "");
                this.LstDefaultDisplayField.Items.Add(defaultItem);
                foreach (CmsContentTypeField field in fields)
                {
                    ListItem item = new ListItem(field.Name, field.SystemName);
                    this.LstDefaultDisplayField.Items.Add(item);
                    this.LstDefaultDisplayField.SelectedValue = type.TitleFieldName;
                }
            }
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

        protected void BtnUpdateDefaultDisplayField_Click(Object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.LstDefaultDisplayField.SelectedValue))
            {
                String guid = Request.QueryString["tid"];
                CmsContentType type = ContentManager.Instance.GetContentType(guid);
                type.TitleFieldName = this.LstDefaultDisplayField.SelectedValue;

                ContentManager.Instance.Save(type);
            }
        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            HiddenField field = GridViewHelper.FindControl<HiddenField>(e.CommandSource, "ExistingFieldId");
            switch (e.CommandName)
            {
                case "editid":
                    Edit(Int32.Parse(field.Value));
                    break;
                case "deleteid":
                    Delete(Int32.Parse(field.Value));
                    break;
            }
        }

        private void Edit(int primaryKey)
        {
            String guid = Request.QueryString["tid"];
            CmsContentTypeField field = ContentManager.Instance.GetContentTypeField(guid, primaryKey);
            if (field != null)
            {
                this.ExistingId.Value = primaryKey.ToString();
                this.FieldType.SelectedValue = field.FieldType;

                this.DropdownFields.Visible = false;
                this.TextAreaFields.Visible = false;
                if (field.FieldType.Equals("Dropdown"))
                {
                    this.DropdownFields.Visible = true;
                    this.Options.Text = field._SelectOptions;
                }
                else if (field.FieldType.Equals("Textarea"))
                {
                    this.TextAreaFields.Visible = true;
                    this.Rows.Text = field.Rows.ToString();
                    this.Cols.Text = field.Columns.ToString();
                }
                
                this.TxtName.Text = field.Name;
                this.TxtSystemName.Text = field.SystemName;
                this.TxtSystemName.Enabled = false;
                this.Description.Text = field.Description;
                this.ChkRequiredField.Checked = field.IsRequired;
                this.Add.Text = "Update Field";
            }
        }

        private void Delete(int primaryKey)
        {
            //Prior to deleting make sure that this field isn't the title field, if so, make them choose a new one
            String guid = Request.QueryString["tid"];

            CmsContentType type = ContentManager.Instance.GetContentType(guid);
            CmsContentTypeField field = ContentManager.Instance.GetContentTypeField(type.Guid, primaryKey);

            if (field.SystemName.Equals(type.TitleFieldName))
            {
                this.LblStatus.Text = "You must reassign the display field prior to deleting this field.";
                this.LblStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                ContentManager.Instance.DeleteField(guid, primaryKey);
                this.FieldTable.DataBind();
            }
        }

        protected void BtnAddNew_Click(object sender, EventArgs e)
        {
            this.DropdownFields.Visible = false;
            this.TextAreaFields.Visible = false;
            this.FieldType.SelectedIndex = 0;
            this.TxtName.Text = "";
            this.TxtSystemName.Text = "";
            this.TxtSystemName.Enabled = true;
            this.Description.Text = "";
            this.ChkRequiredField.Checked = false;
            this.Cols.Text = "";
            this.Rows.Text = "";
            this.Options.Text = "";
            this.ExistingId.Value = "";
            this.Add.Text = "Add Field";
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

            int existing = 0;
            Int32.TryParse(this.ExistingId.Value, out existing);

            CmsContentTypeField field = ContentManager.Instance.GetContentTypeField(guid, existing);
            if (field == null)
                field = new CmsContentTypeField();

            field.Parent = type;
            field.SystemName = this.TxtSystemName.Text;
            field.Name = this.TxtName.Text;
            field.Description = this.Description.Text;
            field.IsRequired = this.ChkRequiredField.Checked;
            this.SetFieldProperties(field);

            ContentManager.Instance.AddContentTypeField(type, field);

            BtnAddNew_Click(sender, e);

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