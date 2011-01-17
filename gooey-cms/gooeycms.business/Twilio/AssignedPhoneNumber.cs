using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;

namespace Gooeycms.Business.Twilio
{
    public class AssignedPhoneNumber
    {
        public String Sid { get; set; }
        public String AccountId { get; set; }
        public String FriendlyName { get; set; }
        public String PhoneNumber { get; set; }
        public String VoiceUrl { get; set; }
        public DateTime Created { get; set; }
        public String InfoUri { get; set; }

        public CmsSubscriptionPhoneNumber AsCmsSubscriptionPhoneNumber()
        {
            CmsSubscriptionPhoneNumber result = new CmsSubscriptionPhoneNumber();
            result.AccountSid = AccountId;
            result.Sid = Sid;
            result.FriendlyPhoneNumber = FriendlyName;
            result.PhoneNumber = PhoneNumber;
            result.TwilioInfoUrl = InfoUri;

            return result;
        }
    }
}
