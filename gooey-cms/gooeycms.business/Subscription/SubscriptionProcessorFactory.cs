using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Gooeycms.Business.Subscription
{
    public class SubscriptionProcessorFactory
    {
        public static SubscriptionProcessorFactory instance;

        static SubscriptionProcessorFactory()
        {
            SubscriptionProcessorFactory.instance = new SubscriptionProcessorFactory();
        }

        public static SubscriptionProcessorFactory Instance
        {
            get { return SubscriptionProcessorFactory.instance; }
        }

        public ISubscriptionProcessor GetSubscriptionProcessor()
        {
            Type type = GooeyConfigManager.SubscriptionProcessorClassType;
            return (ISubscriptionProcessor)Activator.CreateInstance(type);
        }
    }
}
