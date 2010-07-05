using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsSubscriptionPlanDao : BaseDao
    {
        public IList<CmsSubscriptionPlan> FindSubscriptionPlans()
        {
            return base.FindAll<CmsSubscriptionPlan>();
        }

        public CmsSubscriptionPlan FindBySku(string sku)
        {
            String hql = "select plan from CmsSubscriptionPlan plan where plan.SKU = :sku";
            return base.NewHqlQuery(hql).SetString("sku", sku).UniqueResult<CmsSubscriptionPlan>();
        }
    }
}
