using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;
using System.Collections.Concurrent;

namespace Gooeycms.Business.Import
{
    public class ImportSiteWorker
    {
        private QueueManager manager;
        private IDictionary<String, DateTime> startedImports = new ConcurrentDictionary<string, DateTime>();

        public ImportSiteWorker()
        {
            manager = new QueueManager(QueueNames.ImportSiteQueue);
        }

        public Boolean IsRunning { get; set; }

        public void ProcessMessages()
        {
            IList<ImportSiteMessage> messages = manager.GetAll<ImportSiteMessage>();
            foreach (ImportSiteMessage message in messages)
            {
                if (!startedImports.ContainsKey(message.ImportHash))
                {
                    try
                    {
                        startedImports[message.ImportHash] = DateTime.Now;
                        Logging.Database.Write("import-site-worker", "Began processing import of site hash:" + message.ImportHash);

                        ImportManager.Instance.Import(Data.Hash.New(message.ImportHash), message.SubscriptionId);
                    }
                    catch (Exception) { }
                }
                else
                {
                    Logging.Database.Write("import-site-worker", "Detected that import is already running for " + message.ImportHash + ", started at " + startedImports[message.ImportHash].ToString());
                }
            }
        }
    }
}
