using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;
using System.Collections.Concurrent;
using Gooeycms.Business.Util;

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
                    List<String> status = new List<String>();
                    try
                    {
                        ImportManager.SendStartEmail(message.CompletionEmail);

                        startedImports[message.ImportHash] = UtcDateTime.Now;
                        Logging.Database.Write("import-site-worker", "Started site import. Date:" + UtcDateTime.Now + ", Hash:" + message.ImportHash + ", subscriptionId:" + message.SubscriptionId);
                        status = ImportManager.Instance.Import(Data.Hash.New(message.ImportHash), message.SubscriptionId, message.DeleteExisting);
                        Logging.Database.Write("import-site-worker", "Successfully completed site import. Date:" + UtcDateTime.Now + ", Hash:" + message.ImportHash + ", subscriptionId:" + message.SubscriptionId);

                        ImportManager.SendCompletionEmail(status, message.CompletionEmail, true);
                    }
                    catch (Exception e)
                    {
                        Logging.Database.Write("import-site-worker", "Unexpected exception. Message:" + e.Message + ", Stack:" + e.StackTrace);
                        status.Add("The site import failed. Reason:" + e.Message);

                        ImportManager.SendCompletionEmail(status, message.CompletionEmail, false);
                    }
                    finally
                    {
                        startedImports.Remove(message.ImportHash);
                    }
                }
                else
                {
                    Logging.Database.Write("import-site-worker", "Detected that import is already running for " + message.ImportHash + ", started at " + startedImports[message.ImportHash].ToString());
                }
            }
        }
    }
}
