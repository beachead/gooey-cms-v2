using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Content;
using Gooeycms.Business.Content;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Page;
using Gooeycms.Business.Util;
using Gooeycms.Business.Membership;

namespace Gooeycms.Webrole.Control.auth.Content
{
    public partial class Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadContentTypes();
            }
            else
            {
                this.LoadContentTypeControls();
            }
        }

        private void LoadContentTypes()
        {
            IList<CmsContentType> types = ContentManager.Instance.GetContentTypes(ContentTypeFilter.IncludeGlobalTypes);

            this.LstContentTypes.Items.Clear();
            foreach (CmsContentType type in types)
            {
                ListItem item = new ListItem(type.Name, type.Guid);
                this.LstContentTypes.Items.Add(item);
            }
        }

        protected void BtnChooseContentType_Click(object sender, EventArgs e)
        {
            this.PanelAddContent.Visible = true;
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

        protected void BtnAddContent_Click(object sender, EventArgs e)
        {
            String typeGuid = this.LstContentTypes.SelectedValue;
            CmsContentType type = ContentManager.Instance.GetContentType(typeGuid);
            if (type != null)
            {
                Boolean required = Boolean.Parse(this.RequireRegistration.SelectedValue);
                if (required)
                {
                    if (String.IsNullOrWhiteSpace(this.RegistrationPage.Text))
                        throw new ArgumentException("You must select a registration page if the content requires registration");
                }

                CmsContent item = new CmsContent();
                item.PublishDate = DateTime.Now;
                item.ExpireDate = DateTime.Now.AddYears(100);
                item.SubscriptionId = CurrentSite.Guid.Value;
                item.Guid = System.Guid.NewGuid().ToString();
                item.Culture = CurrentSite.Culture;
                item.Author = LoggedInUser.Username;
                item.IsApproved = false;
                item.LastSaved = DateTime.Now;
                item.RequiresRegistration = required;
                item.RegistrationPage = this.RegistrationPage.SelectedValue;
                item.Content = this.TxtEditor.Text;
                item.ContentType = type;

                CmsContent content = ContentManager.Instance.CreateContent(item, this.ControlTable);
                String filterId = "";
                if (content != null)
                {
                    //TODO Attach tags to the content
                    filterId = content.ContentType.Guid;
                }
                Response.Redirect("./Default.aspx?s=1&filter=" + filterId, true);
            }
        }

        private void LoadContentTypeControls()
        {
            String typeGuid = this.LstContentTypes.SelectedValue;
            CmsContentType type = ContentManager.Instance.GetContentType(typeGuid);
            if (type != null)
            {
                this.TxtEditor.Visible = type.IsEditorVisible;
                IList<ContentWebControlManager.ContentWebControl> controls = ContentWebControlManager.Instance.GetContentTypeControls(typeGuid);
                foreach (ContentWebControlManager.ContentWebControl field in controls)
                {
                    TableRow row = GetTableRow(field);
                    this.ControlTable.Rows.Add(row);
                }
            }
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