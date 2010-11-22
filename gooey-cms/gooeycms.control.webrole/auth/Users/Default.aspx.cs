﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Webrole.Control.auth.Users
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void RadScriptManager_OnAjaxError(Object sender, AsyncPostBackErrorEventArgs e)
        {
            String message = e.Exception.Message;
            if (e.Exception.InnerException != null)
                message = e.Exception.InnerException.Message;

            RadScriptManager.AsyncPostBackErrorMessage = message;
        }

        protected void LnkAddUser_Click(Object sender, EventArgs e)
        {
            this.UserGridView.MasterTableView.InsertItem();
            this.UserGridView.DataBind();
        }
    }
}