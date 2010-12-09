﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;
using Telerik.Web.UI;

namespace Gooeycms.Webrole.Control.auth.global_admin.Subscriptions
{
    public partial class AllSubscriptions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AllSubscriptionsTable_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void RadAjaxManager_AjaxRequest(Object sender, AjaxRequestEventArgs e)
        {
            AllSubscriptionsTable.Rebind();
        }

        protected void AllSubscriptionsTable_ItemCommand(Object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            String guid = (String)e.CommandArgument;
            switch (e.CommandName)
            {
                case "DeleteSubscription":
                    Delete(guid);
                    break;
                default:
                    throw new ArgumentException("The command " + e.CommandName + " is not supported");
            }
        }

        private void Delete(String guid)
        {
            CmsSubscription subscription = SubscriptionManager.GetSubscription(guid);
            SubscriptionManager.CancelSubscription(subscription);

            this.AllSubscriptionsTable.DataBind();
        }
    }
}