using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using Gooeycms.Data.Model.Form;
using Gooeycms.Constants;

namespace Gooeycms.Business.Forms.Plugins
{
    public class DatabaseStorePlugin : FormPlugin
    {
        public const char FIELD_SEPARATOR = CmsForm.FieldSeparator;

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
                    if (!key.Equals("filename"))
                    {
                        keys.Append(key).Append(FIELD_SEPARATOR);
                        values.Append(base.GetField(key)).Append(FIELD_SEPARATOR);
                    }
                }
            }

            CmsForm form = new CmsForm();
            form.Guid = System.Guid.NewGuid().ToString();
            form.SubscriptionId = CurrentSite.Guid.Value;
            form.DownloadedFile = common.Filename;
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
