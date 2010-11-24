using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;

namespace Gooeycms.Webrole.Control
{
    public partial class SecureNoNavigation : System.Web.UI.MasterPage
    {
        protected static String Root = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();

            this.LoggedInUsername.Text = LoggedInUser.Wrapper.UserInfo.Firstname;
            if (!Page.IsPostBack)
            {
                LoadWebsites();
            }
        }

        private void LoadWebsites()
        {
            MembershipUserWrapper wrapper = LoggedInUser.Wrapper;
            IList<CmsSubscription> subscriptions = SubscriptionManager.GetSubscriptionsByUserId(wrapper.UserInfo.Id);
            foreach (CmsSubscription subscription in subscriptions)
            {
                ListItem item = new ListItem(subscription.DefaultDisplayName, subscription.Guid);
                if (CurrentSite.IsAvailable)
                {
                    if (subscription.Guid.Equals(CurrentSite.Guid.Value))
                        item.Attributes["style"] = "font-weight:bold;";
                }

                this.LstWebsites.Items.Add(item);
            }
        }

        protected void LstWebsites_Click(Object sender, BulletedListEventArgs e)
        {
            ListItem item = this.LstWebsites.Items[e.Index];
            SiteHelper.SetActiveSiteCookie(item.Value);
            SiteHelper.Configure(Data.Guid.New(item.Value));

            Response.Redirect("~/auth/Default.aspx");
        }


        protected void OnLogout_Click(Object sender, EventArgs e)
        {
            Response.Redirect("~/login.aspx");
        }
    }
}