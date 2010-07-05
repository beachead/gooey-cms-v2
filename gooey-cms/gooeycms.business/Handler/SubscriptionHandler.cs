using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Handler
{
    public class SubscriptionHandler : BaseHttpHandler
    {
        protected override void Process(System.Web.HttpContext context)
        {
            ISubscriptionProcessor processor = SubscriptionProcessorFactory.Instance.GetSubscriptionProcessor();

            Logging.Info("Processing subscription request using processor:" + processor.GetType().FullName);
            processor.ProcessRequest(context.Request);
        }
    }
}
