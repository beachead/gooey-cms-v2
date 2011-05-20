using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Import
{
    [Serializable]
    public class ImportSiteMessage : IQueueMessage
    {
        public String ImportHash { get; set; }
        public String SubscriptionId { get; set; }
        public String CompletionEmail { get; set; }
        public string MessageId { get; set; }
        public Boolean DeleteExisting { get; set; }
        public Boolean ReplacePhoneNumbers { get; set; }
    }
}
