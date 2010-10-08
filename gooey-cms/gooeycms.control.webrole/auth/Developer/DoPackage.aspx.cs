using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Store;
using Gooeycms.Business.Store;

namespace Gooeycms.Webrole.Control.auth.Developer
{
    public partial class DoPackage : System.Web.UI.Page, Gooeycms.Business.Store.SitePackageManager.IPackageStatusNotifier
    {
        private static Dictionary<String, Pair> events = new Dictionary<string, Pair>();
        private static HashSet<String> eventKeys = new HashSet<String>();
        private static Gooeycms.Business.Store.SitePackageManager.IPackageStatusNotifier notifier = new DoPackage();

        private String guid;
        protected void Page_Load(object sender, EventArgs e)
        {
            guid = Request.QueryString["g"];
            if (!Page.IsPostBack)
            {
                this.ExistingGuid.Value = guid;
                Package package = SitePackageManager.NewInstance.GetPackage(guid);
                if (package == null)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "self-close", @"<script language=""javascript"" type=""text/javascript"">self.close();</script>");

                this.LblSiteTitle.Text = package.Title;
            }

            List<String> localEventKeys = eventKeys.ToList<String>();
            if (eventKeys.Count > 100)
            {
                for (int i = 0; i < 50; i++)
                {
                    String key = localEventKeys[i];
                    events.Remove(key);
                }
            }
        }


        [System.Web.Services.WebMethod()]
        public static void DoPackageSite(String guid)
        {
            SitePackageManager manager = SitePackageManager.NewInstance;

            Package package = manager.GetPackage(guid);
            manager.CreatePackage(package, notifier);
            manager.DeployDemoPackage(package.Guid, notifier);
        }

        [System.Web.Services.WebMethod()]
        public static String DoUpdateStatus(String guid)
        {
            String result = "";
            if (events.ContainsKey(guid))
            {
                Pair pair = events[guid];
                if (pair != null)
                {
                    result = (String)pair.Second + "," + (String)pair.First;
                }
            }
            return result;
        }

        public void OnNotify(string guid, string eventName, int stepCount, int maxSteps)
        {
            double dec = (double)stepCount / (double)maxSteps;
            int result = (int)Math.Round(dec * 100, 0);

            Pair pair = new Pair(eventName, result.ToString());
            events[guid] = pair;
            eventKeys.Add(guid);
        }
    }
}