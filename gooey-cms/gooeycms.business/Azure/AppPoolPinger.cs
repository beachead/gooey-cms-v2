using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using Gooeycms.Business.Subscription;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Azure
{
    public class AppPoolPinger
    {
        public AppPoolPinger()
        {
        }

        public void Ping(String url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }

        public void PingGooeyCmsSites()
        {
            var subscriptions = SubscriptionManager.GetAllSubscriptions().OrderBy(a => Guid.NewGuid());
            int size = subscriptions.Count();
            int maxpos = (size >= 100) ? 100 : size;

            foreach (CmsSubscription subscription in subscriptions)
            {
                String domain = (!String.IsNullOrEmpty(subscription.Domain)) ? subscription.Domain : subscription.StagingDomain;

                if (!String.IsNullOrEmpty(domain))
                {
                    String url = "http://" + domain;
                    try
                    {
                        Ping(url);
                    }
                    catch (Exception)
                    {
                        Logging.Database.Write("ping-error", "Failed to ping gooey url: " + url);
                    }
                }
            }
        }
    }
}
