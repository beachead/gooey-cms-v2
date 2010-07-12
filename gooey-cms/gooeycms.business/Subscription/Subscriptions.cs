using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Business.Membership;
using Gooeycms.Constants;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Business.Subscription
{
    public static class SubscriptionManager
    {
        public static bool IsSubdomainAvailable(String subdomain)
        {
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return (dao.FindBySubdomain(subdomain) == null);
        }

        public static IList<CmsSubscriptionPlan> GetSubscriptionPlans()
        {
            CmsSubscriptionPlanDao dao = new CmsSubscriptionPlanDao();
            return dao.FindSubscriptionPlans();
        }

        public static CmsSubscriptionPlan GetSubscriptionPlan(Registration registration)
        {
            SubscriptionPlans temp = (SubscriptionPlans)Enum.Parse(typeof(SubscriptionPlans), registration.SubscriptionPlanId.ToString(), true);
            CmsSubscriptionPlan plan = SubscriptionManager.GetSubscriptionPlan(temp.ToString());
            return plan;
        }

        public static CmsSubscriptionPlan GetSubscriptionPlan(String sku)
        {
            CmsSubscriptionPlanDao dao = new CmsSubscriptionPlanDao();
            CmsSubscriptionPlan cmsPlan = dao.FindBySku(sku);

            return cmsPlan;
        }

        public static CmsSubscriptionPlan GetSubscriptionPlan(SubscriptionPlans plan)
        {
            return GetSubscriptionPlan(plan.ToString());
        }

        public static CmsSubscription CreateFromRegistration(Registration registration)
        {
            MembershipUserWrapper wrapper = MembershipUtil.FindByUsername(registration.Email);
            if (!wrapper.IsValid())
                throw new ApplicationException("The registration has not been properly saved to the database.");

            CmsSubscription subscription = new CmsSubscription();
            subscription.Guid = registration.Guid;
            subscription.Created = DateTime.Now;
            subscription.Subdomain = registration.Sitename;
            subscription.Domain = registration.Domain;
            subscription.StagingDomain = registration.Staging;
            subscription.SubscriptionPlanId = registration.SubscriptionPlanId;
            subscription.Expires = DateTime.Now.AddYears(100);
            subscription.IsDisabled = false;
            subscription.PrimaryUserGuid = wrapper.UserInfo.Guid;
            subscription.IsSalesforceEnabled = registration.IsSalesforceEnabled;
            subscription.IsGenericOptionsEnabled = registration.IsGenericOptionEnabled;

            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            using (Transaction tx = new Transaction())
            {
                dao.SaveObject(subscription);
                tx.Commit();
            }

            using (Transaction tx = new Transaction())
            {
                dao.AddUserToSubscription(wrapper.UserInfo.Id, subscription.Id);
                tx.Commit();
            }

            return subscription;
        }

        public static string GetDescription(Registration registration)
        {
            CmsSubscriptionPlan plan = GetSubscriptionPlan(registration);

            StringBuilder desc = new StringBuilder();
            desc.Append(plan.Name + " @ $" + plan.Price + " / month");
            if (registration.IsSalesforceEnabled)
                desc.Append(" + Salesforce Option @ $" + GooeyConfigManager.SalesForcePrice + " / month");

            return desc.ToString();
        }

        public static string GetShortDescription(String header, Registration registration)
        {
            CmsSubscriptionPlan plan = GetSubscriptionPlan(registration);

            StringBuilder desc = new StringBuilder();
            desc.Append(header + " " + plan.Name);
            if (registration.IsSalesforceEnabled)
                desc.Append(" + Salesforce");

            return desc.ToString();
        }

        public static Double CalculateCost(Registration registration)
        {
            CmsSubscriptionPlan plan = GetSubscriptionPlan(registration);

            Double total = (double)plan.Price;
            if (registration.IsSalesforceEnabled)
                total += GooeyConfigManager.SalesForcePrice;

            return total;
        }

        public static IList<CmsSubscription> GetSubscriptionsByUserId(int userId)
        {

            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return dao.FindByUserId(userId);
        }

        public static CmsSubscription GetSubscription(String siteGuid)
        {
            CmsSubscriptionDao dao = new CmsSubscriptionDao();
            return dao.FindByGuid(siteGuid);
        }
    }
}
