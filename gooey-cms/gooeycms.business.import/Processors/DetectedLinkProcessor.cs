using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Import.Processors
{
    /// <summary>
    /// This processor class stores all of the links that the crawler finds.
    /// It does not actually process any of the data.
    /// </summary>
    public class DetectedLinkProcessor : IPipelineStep
    {
        private static Object key = new Object();
        private static List<CrawlerEntry> links = new List<CrawlerEntry>();

        public static void Init()
        {
            lock (key)
            {
                links = new List<CrawlerEntry>();
            }
        }

        public void Process(NCrawler.Crawler crawler, NCrawler.PropertyBag propertyBag)
        {
            CrawlerEntry entry = new CrawlerEntry();
            entry.Uri = propertyBag.Step.Uri;
            entry.ContentType = propertyBag.ContentType;

            lock (key)
            {
                links.Add(entry);
            }
        }


        public static IList<CrawlerEntry> Links
        {
            get 
            {
                lock (key)
                {
                    return new List<CrawlerEntry>(DetectedLinkProcessor.links);
                }
            }
        }

        public string ImportSiteHash
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
            set
            {
            }
        }
    }
}
