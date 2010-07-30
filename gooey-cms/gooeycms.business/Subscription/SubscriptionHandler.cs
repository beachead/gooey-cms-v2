
using Gooeycms.Business.Handler;
namespace Gooeycms.Business.Subscription
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
