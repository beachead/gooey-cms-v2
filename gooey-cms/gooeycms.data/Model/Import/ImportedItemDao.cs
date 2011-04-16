using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Data.Model.Import
{
    public class ImportedItemDao : BaseDao
    {
        public ImportedItem FindByGuid(Data.Guid importGuid)
        {
            String hql = "select item from ImportedItem item where item.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", importGuid.Value).UniqueResult<ImportedItem>();
        }

        public void DeleteBySiteHash(string siteHash)
        {
            using (Transaction tx = new Transaction())
            {
                foreach (ImportedItem item in FindbyHash(Hash.New(siteHash)))
                    base.Delete<ImportedItem>(item);

                tx.Commit();
            }
        }

        public IList<ImportedItem> FindbyHash(Hash hash)
        {
            String hql = "select items from ImportedItem items where items.ImportHash = :hash order by items.ContentType, items.Uri asc";
            return base.NewHqlQuery(hql).SetString("hash", hash.Value).List<ImportedItem>();
        }

        public void RemoveImportedItems(IList<Data.Guid> removed)
        {
            using (Transaction tx = new Transaction())
            {
                foreach (Data.Guid guid in removed)
                {
                    ImportedItem item = FindByGuid(guid);
                    if (item != null)
                        base.Delete<ImportedItem>(item);
                }
                tx.Commit();
            }
        }
    }
}
