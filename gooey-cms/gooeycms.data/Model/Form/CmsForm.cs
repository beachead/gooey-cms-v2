using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Constants;

namespace Gooeycms.Data.Model.Form
{
    public class CmsForm : BasePersistedItem
    {
        public const char FieldSeparator = TextConstants.DefaultSeparator;

        public virtual String Guid { get; set; }
        public virtual String SubscriptionId { get; set; }
        public virtual DateTime Inserted { get; set; }
        public virtual String FormUrl { get; set; }
        public virtual String IpAddress { get; set; }
        public virtual String Email { get; set; }
        public virtual String RawCampaigns { get; set; }
        public virtual String DownloadedFile { get; set; }
        public virtual String _FormKeys { get; set; }
        public virtual String _FormValues { get; set; }

        private Dictionary<String, String> _parsedFormFields = null;
        public virtual Dictionary<String, String> FormFields
        {
            get
            {
                if (_parsedFormFields == null)
                {
                    String[] keys = _FormKeys.Split(FieldSeparator);
                    String[] values = _FormValues.Split(FieldSeparator);

                    if (keys.Length == values.Length)
                    {
                        _parsedFormFields = new Dictionary<string, string>();
                        for (int i = 0; i < keys.Length; i++)
                        {
                            if (!String.IsNullOrEmpty(keys[i]))
                                _parsedFormFields[keys[i]] = values[i].Trim();
                        }
                    }
                }

                return _parsedFormFields;
            }
        }
    }
}
