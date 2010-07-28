
namespace Gooeycms.Business.Storage
{
    public static class StorageHelper
    {
        public static IStorageClient GetStorageClient()
        {
            IStorageClient client = new AzureBlobStorageClient();
            return client;
        }
    }
}
