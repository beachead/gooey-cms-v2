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

        public const String NotApplicable = "n/a";

        public const String SubscriptionModification = "Subscription Modification";
        public const String Downgrade = "Downgrade";
        public const String Upgrade = "Upgrade";
        public const String Purchase = "Purchase";

        public void AddHistory(Data.Guid subscriptionId, String paypalProfileId, String transactionId, String transactionType, Double amount, String description)
        {
            BillingHistory history = new BillingHistory();
            history.SubscriptionId = subscriptionId.Value;
            history.PaypalProfileId = paypalProfileId;
            history.TxAmount = amount;
            history.TxDate = DateTime.Now;
            history.TxDescription = description;
            history.TxId = transactionId;
            history.TxType = transactionType;

            Add(history);
        }

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
