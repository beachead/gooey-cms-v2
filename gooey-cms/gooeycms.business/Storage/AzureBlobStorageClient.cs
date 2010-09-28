using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Text;
using System.IO;

namespace Gooeycms.Business.Storage
{
    public class AzureBlobStorageClient : IStorageClient
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

        public void Save(String containerName, String directoryName, String filename, String contents, Permissions permissions)
        {
            if (contents == null)
                contents = "";
            this.Save(containerName, directoryName, filename, Encoding.UTF8.GetBytes(contents), permissions);
        }

        public void Save(String containerName, String directoryName, String filename, byte[] data, Permissions permissions)
        {
            filename = filename.Replace(" ", "");
            if ((data != null) && (data.Length > 0))
            {
                CloudBlobContainer container = GetBlobContainer(containerName);
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

                String blobfilename = GetRelativeFilename(directoryName, filename);

                CloudBlob blob = container.GetBlobReference(blobfilename);
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

        private static String GetRelativeFilename(String directoryName, String filename)
        {
            if (directoryName != StorageClientConst.RootFolder)
            {
                filename = directoryName + "/" + filename;
            }
            return filename;
        }

        public void Delete(String containerName, String directoryName, string filename)
        {
            filename = filename.Replace(" ", "");
            CloudBlobContainer container = GetBlobContainer(containerName);
            if (container.Exists())
            {
                filename = GetRelativeFilename(directoryName, filename);
                CloudBlob blob = container.GetBlobReference(filename);
                blob.DeleteIfExists();
            }
        }

        public byte[] Open(String containerName, String directoryName, string filename)
        {
            filename = filename.Replace(" ","");
            byte[] result = new byte[] { };

            CloudBlobContainer container = GetBlobContainer(containerName);
            if (container.Exists())
            {
                CloudBlob blob = GetCloudBlob(container,directoryName, filename);
                if (blob.Exists())
                    result = blob.DownloadByteArray();
            }
            return result;
        }

        public String OpenAsString(String containerName, String directoryName, String filename)
        {
            byte[] data = this.Open(containerName,directoryName, filename);
            return Encoding.UTF8.GetString(data);
        }

        public void Read(String containerName, String directoryName, String filename, Stream stream)
        {
            filename = filename.Replace(" ", "");
            CloudBlobContainer container = GetBlobContainer(containerName);
            if (container.Exists())
            {
                CloudBlob blob = GetCloudBlob(container, directoryName, filename);
                if (blob.Exists())
                {
                    byte[] buffer = new byte[1<<16]; //64KB
                    int bytesRead = 0;
                    using (BlobStream blobstream = blob.OpenRead())
                    {
                        while ((bytesRead = blobstream.Read(buffer, 0, buffer.Length)) != 0)
                            stream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public StorageFile GetFile(String containerName, String directoryName, String filename)
        {
            filename = filename.Replace(" ", "");

            StorageFile result = new StorageFile();
            CloudBlobContainer container = GetBlobContainer(containerName);
            if (container.Exists())
            {
                CloudBlob blob = GetCloudBlob(container, directoryName, filename);
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

        public IList<StorageFile> List(String containerFolder)
        {
            return List(containerFolder, StorageClientConst.RootFolder);
        }

        public IList<StorageFile> List(String containerFolder, String directoryName)
        {
            IList<StorageFile> results = new List<StorageFile>();

            CloudBlobContainer container = GetBlobContainer(containerFolder);
            if (container.Exists())
            {
                IEnumerable<IListBlobItem> items = null; ;
                if (directoryName != null)
                {
                    try
                    {
                        CloudBlobDirectory directory = container.GetDirectoryReference(directoryName);
                        items = directory.ListBlobs();
                    }
                    catch (Exception) { }
                }
                else
                {
                    items = container.ListBlobs();
                }

                if (items != null)
                {
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
            }

            return results;
        }

        public void CopyDirectory(String sourceContainer, String sourceDirectory, String destinationContainer, String destinationDirectory)
        {
            CloudBlobContainer sourceBlobContainer = GetBlobContainer(sourceContainer);
            CloudBlobContainer destinationBlobContainer = GetBlobContainer(destinationContainer);
            
            destinationBlobContainer.CreateIfNotExist();

            IList<StorageFile> files = List(sourceContainer, sourceDirectory);
            foreach (StorageFile file in files)
            {
                CloudBlob destination = GetCloudBlob(destinationBlobContainer, destinationDirectory, file.Filename);
                CloudBlob source = GetCloudBlob(sourceBlobContainer, sourceDirectory, file.Filename);
                destination.CopyFromBlob(source);
            }
        }

        private static string GetBlobFilename(CloudBlob blob)
        {
            return blob.Uri.ToString().Substring(blob.Uri.ToString().LastIndexOf("/") + 1);
        }

        public StorageFile GetInfo(String containerName, String directoryName, String filename)
        {
            filename = filename.Replace(" ", "");
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlob blob = GetCloudBlob(container, directoryName, filename);

            StorageFile file = new StorageFile();
            if (blob.Exists())
            {
                file.Filename = GetBlobFilename(blob);
                file.Uri = blob.Uri;
                file.Metadata = blob.Metadata;
                file.Size = blob.Properties.Length;
            }

            return file;
        }

        public StorageContainer GetContainerInfo(String name)
        {
            CloudBlobContainer container = GetBlobContainer(name);
            return new StorageContainer() { Uri = container.Uri };
        }

        public void AddMetadata(String key, String value)
        {
            this.metadata.Add(key, value);
        }

        public void SetMetadata(String containerName, String directoryName, string filename)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            if (container.Exists())
            {
                filename = GetRelativeFilename(directoryName, filename);
                CloudBlob blob = container.GetBlobReference(filename);
                if (blob.Exists())
                {
                    foreach (String key in metadata.Keys)
                        blob.Metadata[key] = metadata[key];

                    blob.SetMetadata();
                }
            }
        }

        private static CloudBlob GetCloudBlob(CloudBlobContainer container, String directoryName, string filename)
        {
            CloudBlob blob;
            if (directoryName != StorageClientConst.RootFolder)
            {
                CloudBlobDirectory directory = container.GetDirectoryReference(directoryName);
                blob = directory.GetBlobReference(filename);
            }
            else
            {
                blob = container.GetBlobReference(filename);
            }

            return blob;
        }
    }
}
