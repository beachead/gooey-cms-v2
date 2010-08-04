using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Azure
{
    public class InstanceCommunication
    {
        public const String QUEUE_NAME = "interprocess-queue";
        private static IDictionary<String, Boolean> readMessages = new Dictionary<String, Boolean>();

        public static IList<WebroleMessage> ReceiveAllUnread()
        {
            QueueManager manager = new QueueManager(QUEUE_NAME);
            IList<WebroleMessage> messages = manager.PeekAll<WebroleMessage>();

            IList<WebroleMessage> unread = new List<WebroleMessage>();
            foreach (WebroleMessage message in messages)
            {
                if (!readMessages.ContainsKey(message.MessageId))
                {
                    unread.Add(message);
                    readMessages.Add(message.MessageId, true);
                }
            }

            return unread;
        }

        public static void Broadcast<T>(Type processorType, T msg)
        {
            WebroleMessage message = new WebroleMessage();
            message.MessageProcessor = processorType;
            message.Message = msg;

            Broadcast(message);
        }

        public static void Broadcast(WebroleMessage message)
        {
            QueueManager manager = new QueueManager(QUEUE_NAME);
            manager.Put<WebroleMessage>(message);
        }

        public static void ProcessMessages(IList<WebroleMessage> messages)
        {
            if ((messages != null) && (messages.Count > 0))
            {
                foreach (WebroleMessage message in messages)
                {
                    IQueueMessageProcessor processor = (IQueueMessageProcessor)Activator.CreateInstance(message.MessageProcessor);
                    processor.Process(message.Message);
                }
            }
        }
    }
}
