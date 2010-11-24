using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;
using Gooeycms.Business.Membership;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Util;
using Gooeycms.Business.Subscription;
using Gooeycms.Extensions;
using CuteWebUI;
using System.IO;

namespace Gooeycms.Webrole.Control.auth.Developer
{
    public partial class Settings : System.Web.UI.Page
    {
         protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DoDataBind();
            }
        }

        private void DoDataBind()
        {
            UserInfo user = LoggedInUser.Wrapper.UserInfo;
            this.LogoSrc.ImageUrl = Logos.GetImageSrc(user.Logo);

        }


        protected void BtnUploadLogo_Click(Object sender, EventArgs e)
        {
            CmsSubscription subscription = SubscriptionManager.GetSubscription(CurrentSite.Guid);
            if (LogoFile.HasFile)
            {
                FileInfo info = new FileInfo(LogoFile.FileName);

                UserInfo user = LoggedInUser.Wrapper.UserInfo;
                String name = user.Logo;
                if (name.IsEmpty())
                    name = System.Guid.NewGuid().ToString() + info.Extension;

                Logos.SaveLogoFile(user, name, LogoFile.FileBytes);
                this.LogoSrc.ImageUrl = Logos.GetImageSrc(name);
                this.LblStatus.Text = "Successfully updated logo. All of your packages will now use this new logo.";
            }
        }
    }
}