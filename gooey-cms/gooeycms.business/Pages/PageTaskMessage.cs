using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;
using Gooeycms.Data.Model.Page;

namespace Gooeycms.Business.Pages
{
    [Serializable]
    public class PageTaskMessage : IQueueMessage
    {
        public enum Actions
        {
            Save,
            Delete
        }

        public Actions Action { get; set; }
        public string MessageId { get; set; }
        public CmsPage Page { get; set; }
    }
}
