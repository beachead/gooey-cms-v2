﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Redirects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LnkAddRedirect_Click(Object sender, EventArgs e)
        {
            this.RedirectGridView.MasterTableView.InsertItem();
            this.RedirectGridView.DataBind();
        }
    }
}