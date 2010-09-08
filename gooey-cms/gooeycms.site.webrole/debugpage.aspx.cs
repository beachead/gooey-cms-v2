using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gooeycms.Data.Model.Form;
using Gooeycms.Business.Util;
using Gooeycms.Business.Forms;

namespace Gooeycms.webrole.sites
{
    public partial class debugpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnTest_Click(object sender, EventArgs e)
        {
            CmsForm form = new CmsForm();
            form.Guid = System.Guid.NewGuid().ToString();
            form.SubscriptionId = CurrentSite.Guid.Value;
            form.Email = "test@test.com";
            form.IpAddress = "192.168.1.1";
            form.RawCampaigns = "";
            form.FormUrl = "/test.aspx";
            form.Inserted = DateTime.Now;
            form._FormKeys = "test||test1||";
            form._FormValues = "test||test1||";

            FormManager.Instance.Save(form);
        }
    }
}