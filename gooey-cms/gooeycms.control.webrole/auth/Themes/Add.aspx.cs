using System;
using System.Web.UI;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Webrole.Control.App_Code;
using Gooeycms.Webrole.Controls;

namespace Gooeycms.Webrole.Control.Auth.Themes
{
    public partial class Add : ValidatedHelpPage
    {
        protected String PageAction = "Add";
        protected override void OnPageLoad(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckExists();
            }
        }

        private void CheckExists()
        {
            if (ThemeManager.Instance.GetAllBySite().Count == 0)
                this.Callout.Text = "You must setup a theme below before you will be able to create site pages";
        }

        protected void SaveTheme_Click(object sender, EventArgs e)
        {
            CurrentSite.Cache.Clear();
            try
            {
                ThemeManager manager = ThemeManager.Instance;
                CmsTheme result = manager.Add(this.ThemeName.Text, this.ThemeDescription.Text);

                this.ThemeName.Enabled = false;
                this.ThemeDescription.Enabled = false;
                this.Save.Enabled = false;

                Response.Redirect("./Templates.aspx?tid=" + result.ThemeGuid, true);
            }
            catch (Exception ex)
            {
                this.Status.StatusType = StatusPanel.StatusTypes.Error;
                this.Status.Text = ex.Message;
            }
            this.Status.ShowStatus = true;
        }
    }
}
