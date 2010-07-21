using System;

namespace Gooeycms.Webrole.Control.auth.Pages
{
    public partial class Default : App_Code.HelpPage
    {
        protected override void OnPageLoad(object sender, EventArgs e)
        {
            Master.SetNavigationOn(Secure.NavigationType.Pages);
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
