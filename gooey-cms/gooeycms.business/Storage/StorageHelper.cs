
using System;
namespace Gooeycms.Business.Storage
{
    public static class StorageHelper
    {
        public const String StorageConfigName = "ActiveStorageConnectionString";
        public static IStorageClient GetStorageClient()
        {
            IStorageClient client = new AzureBlobStorageClient();
            return client;
        }
    }
}
