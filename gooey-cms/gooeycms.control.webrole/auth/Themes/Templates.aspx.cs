using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Webrole.Control.App_Code;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Themes;

namespace Gooeycms.Webrole.Control.auth.Themes
{
    public partial class Templates : ValidatedPage
    {
        protected override void OnLoad(object sender, EventArgs e)
        {
            Master.SetNavigationOn(Secure.NavigationType.Site);

            if (!Page.IsPostBack)
            {
                CmsTheme theme = ThemeManager.Instance.GetByGuid(Request.QueryString["tid"]);
                this.ThemeName.Text = theme.Name;

                LoadAvailableTemplates(theme);
            }
        }

        private void LoadAvailableTemplates(CmsTheme theme)
        {
            this.TemplateType.Items.Clear();

            IList<String> names = TemplateManager.Instance.GetAvailableGlobalTemplateTypeNames(theme);
            foreach (String name in names)
            {
                ListItem item = new ListItem(name, name);
                this.TemplateType.Items.Add(item);
            }
        }

        protected void AddCustomType_Click(object sender, EventArgs e)
        {
            if (this.CustomTemplateType.Visible)
            {
                this.TemplateType.Visible = true;
                this.AddCustomType.Visible = true;

                this.UseGlobalType.Visible = false;
                this.CustomTemplateType.Visible = false;
                this.CustomTemplateTypeLabel.Visible = false;
            }
            else
            {
                this.TemplateType.Visible = false;
                this.AddCustomType.Visible = false;

                this.UseGlobalType.Visible = true;
                this.CustomTemplateType.Visible = true;
                this.CustomTemplateTypeLabel.Visible = true;
            }
        }

        protected void OnSave_Click(object sender, EventArgs e)
        {
        }
    }
}
