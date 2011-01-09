using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Webrole.Control.auth.Users
{
    public partial class Default : App_Code.ValidatedHelpPage
    {
        protected override void OnPageLoad(object sender, EventArgs e)
        {
        }

        protected void LnkAddUser_Click(Object sender, EventArgs e)
        {
            this.UserGridView.MasterTableView.InsertItem();
            this.UserGridView.DataBind();
        }
    }
}