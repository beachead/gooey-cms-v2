
using System;
using Gooeycms.Business.Util;
namespace Gooeycms.Business.Storage
{
    public static class StorageHelper
    {
        public const String StorageConfigName = "ActiveStorageConnectionString";
        public static IStorageClient GetStorageClient()
        {
            IStorageClient client = new AzureBlobStorageClient();

            if (CurrentSite.IsAvailable)
            {
                client.AddClientOption<Boolean>(CloudStorageOptions.UseCdn, CurrentSite.Configuration.IsCdnEnabled);
                client.AddClientOption<Boolean>(CloudStorageOptions.UseHttps, CurrentSite.Configuration.IsCdnEnabled);
            }

            return client;
        }
    }
}
