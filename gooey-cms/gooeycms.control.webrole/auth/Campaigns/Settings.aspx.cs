using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (CurrentSite.Configuration.IsGoogleAnalyticsEnabled)
                {
                    this.RdoGoogleEnabledNo.Checked = false;
                    this.RdoGoogleEnabledYes.Checked = true;
                }
                else
                {
                    this.RdoGoogleEnabledNo.Checked = true;
                    this.RdoGoogleEnabledYes.Checked = false;
                }

                this.TxtGoogleAccountId.Text = CurrentSite.Configuration.GoogleAccountId;
            }
        }

        protected void BtnSaveGoogle_Click(object sender, EventArgs e)
        {
            Boolean isEnabled = (this.RdoGoogleEnabledYes.Checked);
            String accountId = (this.TxtGoogleAccountId.Text);

            CurrentSite.Configuration.GoogleAccountId = accountId;
            CurrentSite.Configuration.IsGoogleAnalyticsEnabled = isEnabled;
        }
    }
}