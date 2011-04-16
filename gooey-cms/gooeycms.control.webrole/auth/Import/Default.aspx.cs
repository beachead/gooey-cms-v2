using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Business.Import;
using Gooeycms.Business.Import.Processors;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.Import
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnCrawlSite_Click(Object sender, EventArgs e)
        {
            Uri uri;
            Uri.TryCreate(this.TxtSiteUrl.Text, UriKind.Absolute, out uri);

            GooeyCrawler crawler = new GooeyCrawler(uri);
            crawler.AddPipelineStep(new DatabasePersistenceProcessor(CurrentSite.Guid));
            String importHash = crawler.Crawl();

            Response.Redirect("./SelectImport.aspx?g=" + importHash);
        }
    }
}