using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Storage
{
    public interface IQueueMessage
    {
        String MessageId { get; set; }
    }
}
