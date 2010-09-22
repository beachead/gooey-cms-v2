using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Store
{
    public class ReceiptDao : BaseDao
    {
        public Receipt FindByGuid(Guid receiptGuid)
        {
            String hql = "select receipt from Receipt receipt where receipt.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", receiptGuid.Value).UniqueResult<Receipt>();
        }
    }
}
