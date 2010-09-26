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
                if (!subscription.IsDemo)
                    Response.Redirect("./Login.aspx", true);

                //Perform an automatic login for this account
                if (Membership.ValidateUser(MembershipUtil.DemoAccountUsername, MembershipUtil.DemoAccountPassword))
                {
                    //Set the site cookies to the demo account
                    SiteHelper.SetActiveSiteCookie(guid);

                    FormsAuthentication.RedirectFromLoginPage(MembershipUtil.DemoAccountUsername, false);
                }
                else
                    Response.Redirect("./Login.aspx", true);
            }
        }
    }
}