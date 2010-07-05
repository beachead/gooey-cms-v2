using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Subscription;

namespace gooeycms.test
{
    class Program
    {
        static void Main(string[] args)
        {
            ISubscriptionProcessor processor = SubscriptionProcessorFactory.Instance.GetSubscriptionProcessor(null);
        }
    }
}
