﻿using System;
using System.Web.UI.WebControls;
using Gooeycms.Business.Themes;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Webrole.Control.App_Code;
using Gooeycms.Business.Adapters;
using Gooeycms.Business.Images;

namespace Gooeycms.Webrole.Control.Auth.Themes
{
    public partial class Default : ValidatedHelpPage
    {
        protected override void OnPageLoad(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Because the gridview doesn't support radiogroups,
        /// we need to manually determine which one to set and uncheck
        /// any other ones to enforce the "one" only rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnEnableClick_Change(object sender, EventArgs e)
        {
            //Clear the existing selected row 
            foreach (GridViewRow oldrow in GridView1.Rows)
            {
                ((RadioButton)oldrow.FindControl("Enabled")).Checked = false;
            }

            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            ((RadioButton)row.FindControl("Enabled")).Checked = true;
        }

        /// <summary>
        /// Updates the theme that is being used within the site.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSaveThemes_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow oldrow in GridView1.Rows)
            {
                HiddenField guid = ((HiddenField)oldrow.FindControl("HiddenId"));
                RadioButton enabled = ((RadioButton)oldrow.FindControl("Enabled"));


                CmsTheme theme = ThemeManager.Instance.GetByGuid((Data.Guid.New(guid.Value)));
                if (theme != null)
                {
                    theme.IsEnabled = enabled.Checked;
                    ThemeManager.Instance.Save(theme);
                }
            }
        }

        protected void OnRowCommand_Click(object sender, GridViewCommandEventArgs e)
        {
            String guid = (String)e.CommandArgument;
            switch (e.CommandName)
            {
                case "deletetheme":
                    DeleteTheme(guid);
                    break;
                default:
                    throw new ArgumentException("The specified command is not supported: " + e.CommandName);
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ThemeAdapter theme = (ThemeAdapter)e.Row.DataItem;
                LinkButton button = (LinkButton)e.Row.FindControl("DeleteTheme");

                if (theme.Theme.IsEnabled)
                    button.OnClientClick = "alert('You may not delete the currently active theme.\\r\\r\\n\\nYou must set a new theme active and save those changes before deleting this theme.'); return false;";
            }
        }

        private void DeleteTheme(Data.Guid themeGuid)
        {
            CmsTheme theme = ThemeManager.Instance.GetByGuid(themeGuid);
            if ((theme != null) && (!theme.IsEnabled))
            {
                ThemeManager.Instance.Delete(theme);
            }

            this.GridView1.DataBind();
        }

        /*
        protected void OnDelete_Click(object sender, EventArgs e)
        {
            HiddenField field = GridViewHelper.FindControl<HiddenField>(sender, "HiddenId");

            try
            {
                ThemeManager.Instance.Delete(Int32.Parse(field.Value));
                this.StatusLabel.Text = "Successfully deleted theme";
                this.GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                this.StatusLabel.Text = "There was an error deleting theme: " + ex.Message;
            }
            this.StatusLabel.Visible = true;
        }
        */

        protected void ThemesDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["guid"] = SiteHelper.GetActiveSiteGuid().Value;
        }
    }
}
