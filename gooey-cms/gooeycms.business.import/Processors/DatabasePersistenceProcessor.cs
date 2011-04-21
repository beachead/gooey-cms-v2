using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Import;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Import.Processors
{
    public class DatabasePersistenceProcessor : ICheckedPipelineStep
    {
        private String siteGuid;
        private String siteHash;

        public string ImportSiteHash
        {
            get
            {
                return this.siteHash;
            }
            set
            {
                this.siteHash = value;
            }
        }

        public DatabasePersistenceProcessor(Data.Guid siteGuid)
        {
            this.siteGuid = siteGuid.Value;
        }

        public void Process(NCrawler.Crawler crawler, NCrawler.PropertyBag propertyBag)
        {
            String uri = propertyBag.Step.Uri.ToString();
            try
            {
                ImportedItem item = new ImportedItem();
                item.ImportHash = this.siteHash;
                item.Guid = System.Guid.NewGuid().ToString();
                item.SubscriptionId = this.siteGuid;
                item.ContentType = propertyBag.ContentType;
                item.ContentEncoding = propertyBag.ContentEncoding;
                item.Expires = UtcDateTime.Now.AddHours(6);
                item.Inserted = UtcDateTime.Now;
                item.Title = propertyBag.Title;
                item.Uri = propertyBag.Step.Uri.ToString();

                ImportedItemDao dao = new ImportedItemDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Save<ImportedItem>(item);
                    tx.Commit();
                }
            }
            catch (Exception e)
            {
                Logging.Database.Write("import-error", "Unexpected exception importing url:" + uri + ", stack:" + e.StackTrace);
            }
        }

        public void PerformErrorCheck()
        {
            //Check if this site hash is already in the database, if so, remove it
            ImportedItemDao dao = new ImportedItemDao();
            dao.DeleteAllByImportHash(Data.Hash.New(this.siteHash));
        }
    }
}
