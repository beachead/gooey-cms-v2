using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Storage
{
    public class QueueManager
    {
        public const string PreviewQueueName = "preview-queue-{0}";

        public static String GetPreviewQueueName(Data.Guid guid)
        {
            return String.Format(PreviewQueueName, guid.Value);
        }

        static QueueManager()
        {
            AzureBlobStorageClient client = new AzureBlobStorageClient();
        }

        private String queueName;
        public QueueManager(String queueName)
        {
            this.queueName = queueName;
        }

        public void Put<T>(T obj, TimeSpan ttl) where T : IQueueMessage
        {
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting(StorageHelper.StorageConfigName);
            CloudQueueClient client = account.CreateCloudQueueClient();
            CloudQueue queue = client.GetQueueReference(this.queueName);
            queue.CreateIfNotExist();

            byte[] data = Serializer.ToByteArray<T>(obj);
            CloudQueueMessage message = new CloudQueueMessage(data);
            queue.AddMessage(message, ttl);
        }

        public void Put<T>(T obj) where T : IQueueMessage
        {
            Put<T>(obj, TimeSpan.FromMinutes(2));
        }

        public IList<T> PeekAll<T>() where T : IQueueMessage
        {
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("ActiveStorageConnectionString");
            CloudQueueClient client = account.CreateCloudQueueClient();
            CloudQueue queue = client.GetQueueReference(this.queueName);

            IList<T> results = new List<T>();
            if (queue.Exists())
            {
                var messages = queue.PeekMessages(CloudQueueMessage.MaxNumberOfMessagesToPeek);
                foreach (CloudQueueMessage message in messages)
                {
                    try
                    {
                        T msg = Serializer.ToObject<T>(message.AsBytes);
                        msg.MessageId = message.Id;
                        results.Add(msg);
                    }
                    catch (Exception) { }
                }
            }

            return results;
        }

        public T GetFirst<T>() where T : IQueueMessage
        {
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("ActiveStorageConnectionString");
            CloudQueueClient client = account.CreateCloudQueueClient();
            CloudQueue queue = client.GetQueueReference(this.queueName);

            T result = default(T);
            if (queue.Exists())
            {
                CloudQueueMessage message = queue.GetMessage();

                if (message != null)
                {
                    result = Serializer.ToObject<T>(message.AsBytes);
                    result.MessageId = message.Id;
                }
            }

            return result;
        }

        public void ClearQueue()
        {
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("ActiveStorageConnectionString");
            CloudQueueClient client = account.CreateCloudQueueClient();
            CloudQueue queue = client.GetQueueReference(this.queueName);
            if (queue.Exists())
            {
                foreach (CloudQueueMessage message in queue.GetMessages(CloudQueueMessage.MaxNumberOfMessagesToPeek))
                {}
            }
        }
    }
}
