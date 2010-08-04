using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Azure
{
    [Serializable]
    public class WebroleMessage : IQueueMessage
    {
        public Type MessageProcessor { get; set; }
        public Object Message { get; set; }
        public string MessageId { get; set; }
    }
}
