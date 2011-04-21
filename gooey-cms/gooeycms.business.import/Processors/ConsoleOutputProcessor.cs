using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Import;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Import.Processors
{
    public class ConsoleOutputProcessor : IPipelineStep
    {
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

        public void Process(NCrawler.Crawler crawler, NCrawler.PropertyBag propertyBag)
        {
            String uri = propertyBag.Step.Uri.ToString();
            ImportedItem item = new ImportedItem();
            item.ImportHash = this.siteHash;
            item.Guid = System.Guid.NewGuid().ToString();
            item.SubscriptionId = "test";
            item.ContentType = propertyBag.ContentType;
            item.ContentEncoding = propertyBag.ContentEncoding;
            item.Expires = UtcDateTime.Now.AddHours(6);
            item.Inserted = UtcDateTime.Now;
            item.Title = propertyBag.Title;
            item.Uri = propertyBag.Step.Uri.ToString();

            Console.WriteLine(">>>" + item.Uri + "," + item.ContentType);
        }
    }
}
