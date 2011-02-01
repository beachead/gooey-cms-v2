using System;
using Gooeycms.Business.Cache;
using Gooeycms.Business;

namespace Gooeycms.Webrole.Control.auth.global_admin.Controls
{
    public partial class Cache : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["clear"] != null)
            {
                CacheManager.Instance.ClearAll();
                GooeyConfigManager.ClearCache();

                Response.Write("Cache was successfully cleared");
            }
        }
    }
}