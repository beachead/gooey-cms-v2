using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Form;

namespace Gooeycms.Business.Forms.Plugins
{
    public class DatabaseStorePlugin : FormPlugin
    {
        public override bool IsEnabled()
        {
            return true;
        }

        public override bool IsExceptionFatal()
        {
            return true;
        }

        public override void Process()
        {
            CommonInfo common = base.ParseCommonInfo();

            StringBuilder keys = new StringBuilder();
            StringBuilder values = new StringBuilder();

            foreach (String key in base.FormFields.Keys)
            {
                if (base.IsValidField(key))
                {
                    keys.Append(key).Append("||");
                    values.Append(base.GetField(key)).Append("||");
                }
            }

            CmsForm form = new CmsForm();
            form.Guid = System.Guid.NewGuid().ToString();
            form.SubscriptionId = CurrentSite.Guid.Value;
            form.Email = common.Email;
            form.IpAddress = common.IpAddress;
            form.RawCampaigns = common.Campaigns;
            form.FormUrl = common.Resource;
            form.Inserted = DateTime.Now;
            form._FormKeys = keys.ToString();
            form._FormValues = values.ToString();

            FormManager.Instance.Save(form);
        }
    }
}
