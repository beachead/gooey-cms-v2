using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Subscription
{
    public class CmsSubscriptionPhoneDao : BaseDao
    {

        public IList<CmsSubscriptionPhoneNumber> FindBySiteGuid(Guid siteGuid)
        {
            String hql = "select numbers from CmsSubscriptionPhoneNumber numbers where numbers.SubscriptionId = :guid order by friendly_phone_number asc";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsSubscriptionPhoneNumber>();
        }

        public CmsSubscriptionPhoneNumber FindByPhoneNumber(string number)
        {
            String hql = "select number from CmsSubscriptionPhoneNumber number where number.PhoneNumber = :number";
            return base.NewHqlQuery(hql).SetString("number", number).UniqueResult<CmsSubscriptionPhoneNumber>();
        }
    }
}
