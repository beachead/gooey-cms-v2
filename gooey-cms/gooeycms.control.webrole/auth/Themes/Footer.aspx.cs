﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Theme;
using Gooeycms.Business.Themes;

namespace Gooeycms.Webrole.Control.auth.Themes
{
    public partial class Footer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.TxtFooter.ImageBrowserQuerystring = "tid=" + Request.QueryString["tid"];
            if (!Page.IsPostBack)
            {
                CmsTheme theme = GetSelectedTheme();
                this.TxtFooter.Text = theme.Footer;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            CmsTheme theme = this.GetSelectedTheme();
            theme.Footer = this.TxtFooter.Text;

            ThemeManager.Instance.Save(theme);
            this.LblStatus.Text = "Successfully saved footer.";
            this.LblStatus.ForeColor = System.Drawing.Color.Green;
        }

        private CmsTheme GetSelectedTheme()
        {
            String guid = Request.QueryString["tid"];
            CmsTheme theme = ThemeManager.Instance.GetByGuid(Data.Guid.New(guid));
            if (theme == null)
                throw new ArgumentException("The specified theme id is not valid.");

            return theme;
        }
    }
}