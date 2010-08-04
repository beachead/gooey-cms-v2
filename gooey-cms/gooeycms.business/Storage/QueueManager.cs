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
    public class QueueNames
    {
        public const string PreviewQueue = "preview-queue-{0}";
        public const string PageActionQueue = "worker-page-action";
    }

    public class QueueManager
    {
        private CloudQueue queue;

        public static String GetPreviewQueueName(Data.Guid guid)
        {
            return String.Format(QueueNames.PreviewQueue, guid.Value);
        }

        static QueueManager()
        {
            AzureBlobStorageClient client = new AzureBlobStorageClient();
        }

        private String queueName;
        public QueueManager(String queueName)
        {
            this.queueName = queueName;
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting(StorageHelper.StorageConfigName);
            CloudQueueClient client = account.CreateCloudQueueClient();
            this.queue = client.GetQueueReference(this.queueName);
        }

        public void Put<T>(T obj, TimeSpan ttl) where T : IQueueMessage
        {
            queue.CreateIfNotExist();

            byte[] data = Serializer.ToByteArray<T>(obj);

            QueueMessageWrapper wrapper = new QueueMessageWrapper();
            wrapper.IsExternal = false;
            wrapper.BinaryData = data;
            if (data.Length >= (CloudQueueMessage.MaxMessageSize - 50))
            {
                wrapper.IsExternal = true;
                wrapper.ExternalGuid = System.Guid.NewGuid().ToString();

                //Save the data externally
                SaveExternally(wrapper.ExternalGuid, wrapper.BinaryData);
                wrapper.BinaryData = null;
            }

            byte[] wrapperData = Serializer.ToByteArray<QueueMessageWrapper>(wrapper);
            CloudQueueMessage message = new CloudQueueMessage(wrapperData);
            queue.AddMessage(message, ttl);
        }

        private CloudQueue GetQueue()
        {

            return queue;
        }

        public void Put<T>(T obj) where T : IQueueMessage
        {
            Put<T>(obj, TimeSpan.FromMinutes(2));
        }

        public IList<T> PeekAll<T>() where T : IQueueMessage
        {
            IList<T> results = new List<T>();
            if (queue.Exists())
            {
                var messages = queue.PeekMessages(CloudQueueMessage.MaxNumberOfMessagesToPeek);
                foreach (CloudQueueMessage message in messages)
                {
                    try
                    {
                        T msg = ProcessMessage<T>(message);
                        results.Add(msg);
                    }
                    catch (Exception) { }
                }
            }

            return results;
        }

        public T GetFirst<T>() where T : IQueueMessage
        {
            T result = default(T);
            if (queue.Exists())
            {
                CloudQueueMessage message = queue.GetMessage();

                if (message != null)
                {
                    result = ProcessMessage<T>(message);
                    queue.DeleteMessage(message);
                }
            }

            return result;
        }


        public IList<T> GetAll<T>() where T : IQueueMessage
        {
            IList<T> results = new List<T>();
            if (queue.Exists())
            {
                var messages = queue.GetMessages(CloudQueueMessage.MaxNumberOfMessagesToPeek);
                foreach (CloudQueueMessage message in messages)
                {
                    T result = ProcessMessage<T>(message);
                    queue.DeleteMessage(message);

                    results.Add(result);
                }
            }

            return results;
        }

        public void ClearQueue()
        {
            if (queue.Exists())
            {
                foreach (CloudQueueMessage message in queue.GetMessages(CloudQueueMessage.MaxNumberOfMessagesToPeek))
                {
                    queue.DeleteMessage(message);
                }
            }
        }

        private T ProcessMessage<T>(CloudQueueMessage message) where T : IQueueMessage
        {
            QueueMessageWrapper wrapper = Serializer.ToObject<QueueMessageWrapper>(message.AsBytes);
            if (wrapper.IsExternal)
                wrapper.BinaryData = ReadExternally(wrapper.ExternalGuid);

            T msg = Serializer.ToObject<T>(wrapper.BinaryData);
            msg.MessageId = message.Id;
            return msg;
        }

        private void SaveExternally(String guid, byte[] data)
        {
            IStorageClient client = StorageHelper.GetStorageClient();
            client.Save("queue-tempstorage", guid, data, Permissions.Private);
        }

        private byte[] ReadExternally(String guid)
        {
            IStorageClient client = StorageHelper.GetStorageClient();
            try
            {
                return client.Open("queue-tempstorage", guid);
            }
            finally
            {
                try
                {
                    client.Delete("queue-tempstorage", guid);
                }
                catch (Exception) { }
            }
        }
    }
}
