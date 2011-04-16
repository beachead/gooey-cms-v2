using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Import.Events;
using NCrawler.Extensions;
using NCrawler;
using NCrawler.HtmlProcessor;
using NCrawler.Services;
using System.Text.RegularExpressions;
using Gooeycms.Business.Import.Processors;
using Gooeycms.Business.Crypto;

namespace Gooeycms.Business.Import
{
    public class GooeyCrawler
    {
        private Uri root;
        private String siteHash;

        private IList<NCrawler.Interfaces.IPipelineStep> pipelineSteps = new List<NCrawler.Interfaces.IPipelineStep>();

        public event EventHandler<CrawlProgressEventArgs> Progress;
        private void OnProgress(CrawlProgressEventArgs e)
        {
            Progress.ExecuteEvent(this, () => e);
        }

        public GooeyCrawler(Uri root)
        {
            this.root = root;
            this.siteHash = TextHash.MD5(this.root.ToString()).Value;
        }

        public GooeyCrawler AddPipelineStep(IPipelineStep step)
        {
            step.ImportSiteHash = siteHash;
            this.pipelineSteps.Add(step);

            return this;
        }

        public String Crawl(int maxThreadCount = 4, int maxCrawlDepth = 100)
        {
            //Use an in-memory setup
            NCrawlerModule.Setup();

            //Always push the html document processor onto the pipeline
            this.pipelineSteps.Insert(0, new HtmlDocumentProcessor());
            using (Crawler c = new Crawler(this.root,this.pipelineSteps.ToArray<NCrawler.Interfaces.IPipelineStep>())
                  {
                      MaximumThreadCount = maxThreadCount,
                      MaximumCrawlDepth = maxCrawlDepth,
                      AdhereToRobotRules = false,
                  })
            {
                c.BeforeDownload += new EventHandler<NCrawler.Events.BeforeDownloadEventArgs>(c_BeforeDownload);
                c.AfterDownload += new EventHandler<NCrawler.Events.AfterDownloadEventArgs>(c_AfterDownload);
                c.DownloadProgress += new EventHandler<NCrawler.Events.DownloadProgressEventArgs>(c_DownloadProgress);
                c.Crawl();
            }

            return this.siteHash;
        }

        void c_AfterDownload(object sender, NCrawler.Events.AfterDownloadEventArgs e)
        {
            CrawlProgressEventArgs e2 = new CrawlProgressEventArgs();
            e2.Uri = e.CrawlStep.Uri;
            e2.EventType = CrawlProgressEventType.AfterCrawl;

            OnProgress(e2);
        }

        void c_DownloadProgress(object sender, NCrawler.Events.DownloadProgressEventArgs e)
        {
        }

        void c_BeforeDownload(object sender, NCrawler.Events.BeforeDownloadEventArgs e)
        {
            CrawlProgressEventArgs e2 = new CrawlProgressEventArgs();
            e2.Uri = e.CrawlStep.Uri;
            e2.EventType = CrawlProgressEventType.BeforeCrawl;

            OnProgress(e2);
        }
    }
}
