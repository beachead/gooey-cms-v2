using System;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Gooeycms.Business.Storage
{
    public class AzureBlobStorageClient : BaseStorageClient
    {
        static AzureBlobStorageClient()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                string connectionString;

                if (RoleEnvironment.IsAvailable)
                    connectionString = RoleEnvironment.GetConfigurationSettingValue(configName);
                else
                    connectionString = ConfigurationManager.AppSettings[configName];

                configSetter(connectionString);
            });
        }

        private CloudBlobContainer GetBlobContainer(String name)
        {
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("ActiveStorageConnectionString");
            CloudBlobClient client = account.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference(name);
            return container;
        }

        public override void Save(String directory, string filename, byte[] data)
        {
            if ((data != null) && (data.Length > 0))
            {
                CloudBlobContainer container = GetBlobContainer(directory);
                bool created = container.CreateIfNotExist();
                if (created)
                {
                    BlobContainerPermissions perms = new BlobContainerPermissions();
                    perms.PublicAccess = BlobContainerPublicAccessType.Off;
                    container.SetPermissions(perms);
                }

                CloudBlob blob = container.GetBlobReference(filename);
                blob.UploadByteArray(data);

                blob.Metadata["created"] = DateTime.Now.ToString();
                blob.Metadata["size"] = data.Length.ToString();
                blob.SetProperties();
            }
        }

        public override void Delete(String directory, string filename)
        {
            CloudBlobContainer container = GetBlobContainer(directory);
            CloudBlob blob = container.GetBlobReference(filename);
            blob.DeleteIfExists();
        }

        public override byte[] Open(String directory, string filename)
        {
            CloudBlobContainer container = GetBlobContainer(directory);
            CloudBlob blob = container.GetBlobReference(filename);

            byte[] result = new byte [] {};
            if (blob.Exists())
                result = blob.DownloadByteArray();

            return result;
        }
    }
}
