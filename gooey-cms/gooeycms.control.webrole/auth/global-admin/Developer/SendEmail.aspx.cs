using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Subscription;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Web;

namespace Gooeycms.Webrole.Control.auth.global_admin.Developer
{
    public partial class SendEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Page.IsPostBack)
                {
                    String guid = Request.QueryString["package"];
                    Package package = SitePackageManager.NewInstance.GetPackage(guid);
                    if (package != null)
                    {
                        CmsSubscription subscription = SubscriptionManager.GetSubscription(package.OwnerSubscriptionId);
                        String userGuid = subscription.PrimaryUserGuid;
                        UserInfo info = MembershipUtil.FindByUserGuid(userGuid).UserInfo;

                        this.ToAddress.Text = info.Email;
                    }
                }
            }
        }

        protected void BtnSend_Click(Object sender, EventArgs e)
        {
            String to = this.ToAddress.Text;
            String from = this.FromAddress.Text;
            String subject = this.Subject.Text;
            String body = this.Body.Text;

            EmailClient client = EmailClient.GetDefaultClient();
            client.ToAddress = to;
            client.FromAddress = from;
            client.Send(subject, body);

            ClientScript.RegisterClientScriptBlock(typeof(SendEmail),"closeWindow", "<script type='text/javascript'>closeWindow()</script>");
        }
    }
}