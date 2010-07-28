using System;
using System.Collections.Generic;
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

        public override void Save(String directory, string filename, byte[] data, Permissions permissions)
        {
            if ((data != null) && (data.Length > 0))
            {
                CloudBlobContainer container = GetBlobContainer(directory);
                bool created = container.CreateIfNotExist();
                if (created)
                {
                    BlobContainerPermissions perms = new BlobContainerPermissions();
                    if (permissions == Permissions.Private)
                        perms.PublicAccess = BlobContainerPublicAccessType.Off;
                    else if (permissions == Permissions.Public)
                        perms.PublicAccess = BlobContainerPublicAccessType.Blob;

                    container.SetPermissions(perms);
                }

                CloudBlob blob = container.GetBlobReference(filename);
                blob.UploadByteArray(data);

                blob.Metadata["filename"] = filename;
                blob.Metadata["created"] = DateTime.Now.ToString();
                blob.Metadata["size"] = data.Length.ToString();
                blob.SetMetadata();
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

        public override IList<StorageFile> List(String directory)
        {
            CloudBlobContainer container = GetBlobContainer(directory);
            var items = container.ListBlobs();

            IList<StorageFile> results = new List<StorageFile>();
            foreach (IListBlobItem item in items)
            {
                CloudBlob blob = container.GetBlobReference(item.Uri.ToString());
                blob.FetchAttributes();
                results.Add(new StorageFile()
                {
                    Filename = GetBlobFilename(blob),
                    Uri = blob.Uri
                });
            }

            return results;
        }

        private static string GetBlobFilename(CloudBlob blob)
        {
            return blob.Uri.ToString().Substring(blob.Uri.ToString().LastIndexOf("/") + 1);
        }

        public override StorageFile GetInfo(string directory, string filename)
        {
            CloudBlobContainer container = GetBlobContainer(directory);
            CloudBlob blob = container.GetBlobReference(filename);

            return new StorageFile()
            {
                Filename = GetBlobFilename(blob),
                Uri = blob.Uri
            };
        }

        public override StorageContainer GetContainerInfo(String name)
        {
            CloudBlobContainer container = GetBlobContainer(name);
            return new StorageContainer() { Uri = container.Uri };
        }
    }
}
