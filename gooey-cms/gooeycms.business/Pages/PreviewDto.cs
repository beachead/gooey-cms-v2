using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Pages
{
    [Serializable]
    public class PreviewDto : IQueueMessage
    {
        public String MessageId { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public String TemplateName { get; set; }
    }
}
