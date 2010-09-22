using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Store;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Business.Store
{
    public class ReceiptManager
    {
        private static ReceiptManager instance = new ReceiptManager();
        private ReceiptManager() { }
        public static ReceiptManager Instance { get { return ReceiptManager.instance; } }

        public void Issue(Receipt receipt)
        {
            receipt.Processed = DateTime.MaxValue;
            receipt.PaidDeveloperOn = DateTime.MaxValue;

            ReceiptDao dao = new ReceiptDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<Receipt>(receipt);
                tx.Commit();
            }
        }

        public Receipt GetByGuid(Data.Guid receiptGuid)
        {
            ReceiptDao dao = new ReceiptDao();
            return dao.FindByGuid(receiptGuid);
        }

        public void Update(Receipt receipt)
        {
            ReceiptDao dao = new ReceiptDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<Receipt>(receipt);
                tx.Commit();
            }
        }
    }
}
