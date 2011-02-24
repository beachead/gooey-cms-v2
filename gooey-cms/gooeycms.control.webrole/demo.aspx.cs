using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using System.Web.Security;
using Gooeycms.Business;
using Gooeycms.Business.Store;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Util;
using Gooeycms.Business.Crypto;

namespace Gooeycms.Webrole.Control
{
    public partial class demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String guid = Request.QueryString["g"];
            CmsSubscription subscription = SubscriptionManager.GetSubscription(guid);
            if (subscription != null)
            {
                if (subscription.IsDemo)
                {
                    //Set the site cookies to the demo account
                    SiteHelper.SetActiveSiteCookie(guid);
                    FormsAuthentication.RedirectFromLoginPage(MembershipUtil.DemoAccountUsername, false);
                }
                else if (LoggedInUser.IsGlobalAdmin)
                {
                    String token = Request.QueryString["t"];
                    Boolean isTokenValid = TokenManager.IsValid(guid + LoggedInUser.Wrapper.UserInfo.Guid, token);
                    if (isTokenValid)
                    {
                        if (subscription.IsRemoteSupportEnabled)
                        {
                            Logging.Database.Write("admin-remote-login", "Global admin " + LoggedInUser.Username + " automatically logged into customer site " + subscription.Domain + ", guid:" + subscription.Guid + ", as-user:" + subscription.PrimaryUser);

                            //Set the site cookies to the demo account
                            SiteHelper.SetActiveSiteCookie(guid);
                            FormsAuthentication.RedirectFromLoginPage(subscription.PrimaryUser.Username, false);
                        }
                        else
                        {
                            Logging.Database.Write("admin-remote-login", "Global admin remote login failed. Subscription: " + subscription.Domain + ", Reason: Remote support is not enabled for the subscription.");
                            Response.Redirect("~/auth/global-admin/subscriptions/AllSubscriptions.aspx?m=Remote+support+is+not+enabled+for+this+subscription", true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("./Login.aspx", true);
                }
            }            
        }
    }
}