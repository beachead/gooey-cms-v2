using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Billing;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Business.Billing
{
    public class BillingManager
    {
        private static BillingManager instance = new BillingManager();
        private BillingManager() { }
        public static BillingManager Instance { get { return BillingManager.instance; } }

        public void Add(BillingHistory history)
        {
            BillingHistoryDao dao = new BillingHistoryDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<BillingHistory>(history);
                tx.Commit();
            }
        }
    }
}
