using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Text;
using Gooeycms.Data.Model.Form;
using Gooeycms.Business.Campaigns;
using Gooeycms.Business.Util;

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Leads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentSite.Subscription.IsCampaignEnabled)
                Response.Redirect("~/auth/default.aspx?addon=campaigns", true);

            Master.SetTitle("Lead Report");
            Master.RegisterPostBackControl(this.BtnGenerateReport);
        }

        protected void BtnFilterDate_Click(object sender, EventArgs e)
        {
            DateTime? startDate = this.ReportStartDate.SelectedDate;
            DateTime? endDate = this.ReportEndDate.SelectedDate;

            this.LstSelectPages.Items.Clear();
            IList<CmsForm> forms = LeadManager.Instance.GetUniqueLeadResponses(startDate, endDate, LeadManager.FormDataMode.ExcludeFormatData);
            foreach (CmsForm form in forms)
            {
                ListItem item = new ListItem(form.FormUrl, form.FormUrl);
                this.LstSelectPages.Items.Add(item);
            }

            int rows = (forms.Count < 15) ? forms.Count + 1 : 15;
            this.LstSelectPages.Rows = rows;
            this.SelectPagesPanel.Visible = true;
        }

        protected void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            IList<String> selectedPages = new List<String>();
            foreach (ListItem item in this.LstSelectPages.Items)
            {
                if (item.Selected)
                    selectedPages.Add(item.Value);
            }

            String csv = LeadManager.Instance.GenerateCsvReport(ReportStartDate.SelectedDate, ReportEndDate.SelectedDate, selectedPages);
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=test.csv");
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            Response.BinaryWrite(UTF8Encoding.Unicode.GetBytes(csv));
            Response.End();
        }
    }
}