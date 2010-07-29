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
        private IDictionary<String, String> metadata = new Dictionary<String, String>();

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
            name = name.Replace(" ", "");
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("ActiveStorageConnectionString");
            CloudBlobClient client = account.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference(name);
            return container;
        }

        public override void Save(String directory, string filename, byte[] data, Permissions permissions)
        {
            filename = filename.Replace(" ", "");
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

                foreach(String key in this.metadata.Keys)
                {
                    blob.Metadata[key] = this.metadata[key];
                }

                blob.SetMetadata();
                blob.SetProperties();
            }
        }

        public override void Delete(String directory, string filename)
        {
            filename = filename.Replace(" ", "");
            CloudBlobContainer container = GetBlobContainer(directory);
            CloudBlob blob = container.GetBlobReference(filename);
            blob.DeleteIfExists();
        }

        public override byte[] Open(String directory, string filename)
        {
            filename = filename.Replace(" ","");
            CloudBlobContainer container = GetBlobContainer(directory);
            CloudBlob blob = container.GetBlobReference(filename);

            byte[] result = new byte [] {};
            if (blob.Exists())
                result = blob.DownloadByteArray();

            return result;
        }

        public override StorageFile GetFile(string directory, string filename)
        {
            filename = filename.Replace(" ", "");

            StorageFile result = new StorageFile();
            CloudBlobContainer container = GetBlobContainer(directory);
            if (container.Exists())
            {
                CloudBlob blob = container.GetBlobReference(filename);
                if (blob.Exists())
                {
                    result.Uri = blob.Uri;
                    result.Filename = GetBlobFilename(blob);
                    result.Metadata = blob.Metadata;
                    result.Data = blob.DownloadByteArray();
                }
            }

            return result;
        }

        public override IList<StorageFile> List(String directory)
        {
            IList<StorageFile> results = new List<StorageFile>();

            CloudBlobContainer container = GetBlobContainer(directory);
            if (container.Exists())
            {
                var items = container.ListBlobs();
                foreach (IListBlobItem item in items)
                {
                    CloudBlob blob = container.GetBlobReference(item.Uri.ToString());
                    if (blob.Exists())
                    {
                        blob.FetchAttributes();
                        results.Add(new StorageFile()
                        {
                            Filename = GetBlobFilename(blob),
                            Uri = blob.Uri,
                            Metadata = blob.Metadata
                        });
                    }
                }
            }

            return results;
        }

        private static string GetBlobFilename(CloudBlob blob)
        {
            return blob.Uri.ToString().Substring(blob.Uri.ToString().LastIndexOf("/") + 1);
        }

        public override StorageFile GetInfo(string directory, string filename)
        {
            filename = filename.Replace(" ", "");
            CloudBlobContainer container = GetBlobContainer(directory);
            CloudBlob blob = container.GetBlobReference(filename);

            StorageFile file = new StorageFile();
            if (blob.Exists())
            {
                file.Filename = GetBlobFilename(blob);
                file.Uri = blob.Uri;
                file.Metadata = blob.Metadata;
            }

            return file;
        }

        public override StorageContainer GetContainerInfo(String name)
        {
            CloudBlobContainer container = GetBlobContainer(name);
            return new StorageContainer() { Uri = container.Uri };
        }

        public override void AddMetadata(String key, String value)
        {
            this.metadata.Add(key, value);
        }

        public override void SetMetadata(string directory, string filename)
        {
            CloudBlobContainer container = GetBlobContainer(directory);
            CloudBlob blob = container.GetBlobReference(filename);
            if (blob.Exists())
            {
                foreach (String key in metadata.Keys)
                    blob.Metadata[key] = metadata[key];

                blob.SetMetadata();
            }
        }
    }
}
