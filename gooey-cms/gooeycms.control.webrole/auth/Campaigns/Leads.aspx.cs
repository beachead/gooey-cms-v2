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

namespace Gooeycms.Webrole.Control.auth.Campaigns
{
    public partial class Leads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadScriptManager.RegisterPostBackControl(this.BtnGenerateReport);
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

        protected void RadScriptManager_OnError(object sender, AsyncPostBackErrorEventArgs e)
        {
            this.RadScriptManager.AsyncPostBackErrorMessage = e.Exception.Message;
        }

        protected void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            String csv = LeadManager.Instance.GenerateCsvReport(ReportStartDate.SelectedDate, ReportEndDate.SelectedDate);
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