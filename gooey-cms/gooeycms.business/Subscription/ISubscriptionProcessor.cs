using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using System.Web;

namespace Gooeycms.Business.Subscription
{
    public interface ISubscriptionProcessor
    {
        /// <summary>
        /// Creates a new subscription from the registration.
        /// </summary>
        /// <param name="registration"></param>
        void ProcessRequest(HttpRequest request);

        bool IsDebug { get; }
    }
}
