using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Threading;

namespace Gooeycms.Business.Storage
{
    public static class BlobExtensions
    {
        public static bool Exists(this CloudBlob blob)
        {
            try
            {
                blob.FetchAttributes();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public static bool CreateIfNotExist(this CloudBlobContainer container,bool validateCreation)
        {
            int maxAttempts = 60;
            int attempts = 0;

            Boolean created = false;
            while (true)
            {
                try
                {
                    container.Create();
                    created = true;
                    break;
                }
                catch (StorageClientException e)
                {
                    if (e.ErrorCode == StorageErrorCode.ContainerAlreadyExists)
                    {
                        created = false;
                        break;
                    }
                }

                if (++attempts >= maxAttempts)
                    break;

                Thread.Sleep(1000);
            }

            return created;
        }

        public static bool Exists(this CloudBlobContainer container)
        {
            try
            {
                container.FetchAttributes();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
