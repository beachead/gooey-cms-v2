using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Content;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.Content
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool restore = (!Page.IsPostBack);
            LoadExistingFields(restore);

            if (!Page.IsPostBack)
                LoadStaticData();
        }

        protected void BtnSaveContent_Click(object sender, EventArgs e)
        {
            String guid = Request.QueryString["tid"];
            CmsContent item = ContentManager.Instance.GetContent(guid);

            String filterId = "";
            if (item != null)
            {
                Boolean requireRegistration = Boolean.Parse(this.RequireRegistration.SelectedValue);
                String regPage = null;
                if (requireRegistration)
                {
                    regPage = this.RegistrationPage.SelectedValue;
                    if (String.IsNullOrEmpty(regPage))
                        throw new ArgumentException("You must select a registration page if the content requires registration.");
                }

                item.Content = this.TxtEditor.Text;
                item.Author = LoggedInUser.Username;
                item.LastSaved = UtcDateTime.Now;
                item.RequiresRegistration = requireRegistration;
                item.RegistrationPage = regPage;

                ContentManager.Instance.Update(item, this.ControlTable);
                filterId = item.ContentType.Guid;
            }
            Response.Redirect("./Default.aspx?s=1&filter=" + filterId, true);
        }

        protected void RequireRegistration_Change(object sender, EventArgs e)
        {
            Boolean required = Boolean.Parse(this.RequireRegistration.SelectedValue);
            if (required)
            {
                PageManager manager = PageManager.Instance;

                this.RegistrationPage.Items.Clear();
                ListItem blank = new ListItem("<Choose Page>", "");
                this.RegistrationPage.Items.Add(blank);

                IList<CmsPage> pages = PageManager.Instance.Filter(PageManager.Filters.AllPages);
                foreach (CmsPage page in pages)
                {
                    ListItem item = new ListItem(page.Url, page.Url);
                    this.RegistrationPage.Items.Add(item);
                }
                this.RegistrationPage.Visible = true;
            }
            else
                this.RegistrationPage.Visible = false;
        }

        private void LoadStaticData()
        {
            CmsContent item = this.GetContent();

            this.TxtEditor.Text = item.Content;
            this.TxtTags.Text = item.TagsAsString();
            this.PublishDate.SelectedDate = item.PublishDate;
            this.ExpireDate.SelectedDate = item.ExpireDate;

            this.RequireRegistration.SelectedValue = (item.RequiresRegistration) ? "True" : "False";
            if (item.RequiresRegistration)
            {
                RequireRegistration_Change(null, null);
                this.RegistrationPage.SelectedValue = item.RegistrationPage;
            }

        }

        private void LoadExistingFields(bool restore)
        {
            CmsContent item = this.GetContent();
            CmsContentType type = item.ContentType;
            if (type != null)
            {
                this.TxtEditor.Visible = type.IsEditorVisible;

                IList<ContentWebControlManager.ContentWebControl> controls = ContentWebControlManager.Instance.GetContentTypeControls(type.Guid);
                foreach (ContentWebControlManager.ContentWebControl control in controls)
                {
                    if (!(control.Control is FileUpload))
                    {
                        String[] parts = control.Control.ID.Split('_');
                        String propertyName = parts[2];

                        CmsContentField itemField = item.FindField(propertyName);
                        String value = "";
                        if (itemField != null)
                            value = item.FindField(propertyName).Value;

                        if (restore)
                            ContentWebControlManager.SetControl(control.Control, value);
                    }
                    TableRow row = GetTableRow(control);
                    this.ControlTable.Rows.Add(row);

                    if (control.Control is FileUpload)
                    {
                        //Add the filename onto the field's value
                        String value = item.FindField("filename").Value;
                        TableRow filenameRow = new TableRow();
                        TableCell cell = new TableCell();
                        cell.ColumnSpan = 2;
                        cell.Text = "(Existing file:" + value + ")";

                        filenameRow.Cells.Add(cell);
                        this.ControlTable.Rows.Add(filenameRow);
                    }
                }
            }
        }

        private CmsContent GetContent()
        {
            String guid = Request.QueryString["tid"];
            return ContentManager.Instance.GetContent(guid);
        }

        private TableRow GetTableRow(ContentWebControlManager.ContentWebControl field)
        {
            TableRow row = new TableRow();

            String label = field.Field.Name;
            if (!label.EndsWith(":"))
                label = label + ":";

            TableCell cell = new TableCell();

            Literal literal = new Literal();
            literal.Text = "<b>" + label;
            if (field.Field.IsRequired)
                literal.Text = literal.Text + " * ";
            literal.Text = literal.Text + "</b><br />";

            cell.Controls.Add(literal);
            cell.Controls.Add(field.Control);
            if (field.Field.IsRequired)
            {
                RequiredFieldValidator required = new RequiredFieldValidator();
                required.ControlToValidate = field.Control.ID;
                if (!field.DisplayError)
                    required.Display = ValidatorDisplay.None;
                else
                {
                    required.ErrorMessage = "* required";
                }
                cell.Controls.Add(required);
            }

            row.Cells.Add(cell);

            return row;
        }
    }
}