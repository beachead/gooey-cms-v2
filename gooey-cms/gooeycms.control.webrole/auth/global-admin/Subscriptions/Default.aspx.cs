﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gooeycms.Webrole.Control.auth.global_admin.Subscriptions
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String timeframe = Request.QueryString["timeframe"];
            if (!String.IsNullOrWhiteSpace(timeframe))
                this.LblTimeframe.Text = timeframe;
        }
    }
}