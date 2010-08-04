using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Storage
{
    [Serializable]
    public class QueueMessageWrapper
    {
        public Boolean IsExternal { get; set; }
        public String ExternalGuid { get; set; }
        public byte [] BinaryData { get; set; }
    }
}
