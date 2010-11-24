using System;
using Gooeycms.Data.Model.Site;
using Gooeycms.Business.Web;
using System.Collections.Generic;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class DefaultOld : App_Code.ValidatedHelpPage
    {
        protected override void OnPageLoad(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Anthem.Manager.Register(this);
            }
        }

        [Anthem.Method]
        public int LoadPageData(String filter)
        {
            Filter.Value = filter;
            this.PageListing.Visible = true;
            this.PageListing.DataBind();

            return PageListing.Rows.Count;
        }
    }
}
