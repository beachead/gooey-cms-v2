using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCrawler;
using NCrawler.Events;

namespace Gooeycms.Business.Import.Events
{
    public enum CrawlProgressEventType
    {
        BeforeCrawl,
        AfterCrawl,
        DownloadStarted,
        DownloadCompleted,
        CrawlError
    }

    public class CrawlProgressEventArgs : EventArgs
    {
        public Uri @Uri { get; set; }
        public CrawlProgressEventType EventType { get; set; }
    }
}
