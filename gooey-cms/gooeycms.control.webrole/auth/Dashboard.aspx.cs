using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Webrole.Control.auth
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(Membership.GetUser().UserName);
                IList<CmsSubscription> subscriptions = SubscriptionManager.GetSubscriptionsByUserId(wrapper.UserInfo.Id);
                foreach (CmsSubscription subscription in subscriptions)
                {
                    ListItem item = new ListItem(subscription.DefaultDisplayName, subscription.Guid);
                    this.AvailableSites.Items.Add(item);
                }
            }
        }

        protected void BtnManageSite_Click(Object sender, EventArgs e)
        {
            SiteHelper.SetActiveSiteCookie(this.AvailableSites.SelectedValue);
            SiteHelper.Configure(Data.Guid.New(this.AvailableSites.SelectedValue));

            Response.Redirect("~/auth/Default.aspx");
        }
    }
}
