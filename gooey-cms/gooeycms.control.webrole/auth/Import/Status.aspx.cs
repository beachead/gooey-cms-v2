﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gooeycms.Webrole.Control.auth.Import
{
    public partial class Status : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.LblToken.Text = Server.HtmlEncode(Request.QueryString["g"]);
            }
        }
    }
}