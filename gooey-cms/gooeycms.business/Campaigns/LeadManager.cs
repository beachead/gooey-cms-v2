using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Form;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Campaigns
{
    public class LeadManager
    {
        public enum FormDataMode
        {
            IncludeFormData,
            ExcludeFormatData
        }

        private static LeadManager instance = new LeadManager();
        public LeadManager() { }
        public static LeadManager Instance { get { return LeadManager.instance; } }

        public IList<CmsForm> GetUniqueLeadResponses(DateTime? startdate, DateTime? enddate, FormDataMode mode)
        {
            return GetUniqueLeadResponses(CurrentSite.Guid, startdate, enddate, mode);
        }

        public IList<CmsForm> GetUniqueLeadResponses(Data.Guid siteGuid, DateTime? startdate, DateTime? enddate, FormDataMode mode)
        {
            IList<CmsForm> results = new List<CmsForm>();

            if ((startdate.HasValue) && (enddate.HasValue))
            {
                CmsFormDao dao = new CmsFormDao();

                if (mode == FormDataMode.ExcludeFormatData)
                {
                    IList<String> forms = dao.FindUniqueForms(siteGuid, startdate.Value, enddate.Value);
                    foreach (String form in forms)
                    {
                        CmsForm temp = new CmsForm();
                        temp.FormUrl = form;

                        results.Add(temp);
                    }
                }
                else if (mode == FormDataMode.IncludeFormData)
                {
                    results = dao.FindUniqueResponses(siteGuid, startdate.Value, enddate.Value, null);
                }
            }
            return results;
        }

        public String GenerateCsvReport(DateTime? startdate, DateTime? enddate, IList<String> filterPages)
        {
            return GenerateCsvReport(CurrentSite.Guid, startdate, enddate, filterPages);
        }

        public string GenerateCsvReport(Data.Guid siteGuid, DateTime? startdate, DateTime? enddate, IList<String> filterPages)
        {
            CmsFormDao dao = new CmsFormDao();
            IList<CmsForm> forms = dao.FindUniqueResponses(siteGuid, startdate.Value, enddate.Value, filterPages);

            HashSet<String> headerKeys = new HashSet<string>();
            foreach (CmsForm form in forms)
            {
                foreach (String key in form.FormFields.Keys)
                    headerKeys.Add(key);
            }

            StringBuilder csv = new StringBuilder();
            csv.Append("Date\tEmail\tResource\tCampaigns\tDownload");
            foreach (String key in headerKeys)
            {
                csv.Append("\t").Append(key);
            }
            csv.AppendLine();

            foreach (CmsForm form in forms)
            {
                csv.AppendFormat("{0}\t", form.Inserted).AppendFormat("{0}\t",form.Email).AppendFormat("{0}\t", form.FormUrl).AppendFormat("{0}\t", form.RawCampaigns).AppendFormat("{0}\t",form.DownloadedFile);
                foreach (String key in headerKeys)
                {
                    String value = "";
                    if (form.FormFields.ContainsKey(key))
                        value = form.FormFields[key];
                    csv.AppendFormat("{0}\t", value);
                }
                csv.AppendLine();
            }
            return csv.ToString();
        }
    }
}
