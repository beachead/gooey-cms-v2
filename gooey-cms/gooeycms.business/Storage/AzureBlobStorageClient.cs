using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using Gooeycms.Extensions;

namespace Gooeycms.Business.Storage
{
    public class AzureBlobStorageClient : IStorageClient
    {
        private IDictionary<String, String> metadata = new Dictionary<String, String>();
        private Dictionary<String, Object> options = new Dictionary<string, object>();

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

        public void AddClientOption<T>(String key, T item)
        {
            options.Add(key, item);
        }

        public T GetClientOption<T>(String key)
        {
            T result;

            Object temp = options.GetValue(key);
            if (temp != null)
                result = (T)temp;
            else
                result = default(T);

            return result;
        }

        private CloudBlobContainer GetBlobContainer(String name)
        {
            name = name.Replace(" ", "");
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting(StorageHelper.StorageConfigName);
            CloudBlobClient client = account.CreateCloudBlobClient();
            
            CloudBlobContainer container = client.GetContainerReference(name);
            return container;
        }

        private CloudBlob GetBlobReference(String url)
        {
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting(StorageHelper.StorageConfigName);
            CloudBlobClient client = account.CreateCloudBlobClient();

            return client.GetBlobReference(url);
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
                bool created = container.CreateIfNotExist(true);
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

        /// <summary>
        /// Deletes all of the snapshots associated with each file in the container and directory
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="directoryName"></param>
        public void DeleteSnapshots(String containerName, String directoryName)
        {
            BlobRequestOptions options = new BlobRequestOptions()
            {
                BlobListingDetails = BlobListingDetails.Snapshots,
                UseFlatBlobListing = true
            };

            IList<BlobSnapshot> snapshotNames = new List<BlobSnapshot>();

            ResultContinuation continuation = null;
            CloudBlobContainer container = GetBlobContainer(containerName);

            if (container.Exists())
            {
                do
                {
                    ResultSegment<IListBlobItem> blobPages = container.ListBlobsSegmented(0, continuation, options);
                    continuation = blobPages.ContinuationToken;

                    foreach (CloudBlob blob in blobPages.Results)
                    {
                        if (blob.SnapshotTime.HasValue)
                            blob.DeleteIfExists();
                    }
                } while (continuation != null);
            }

        }

        public Boolean ContainsSnapshots(String containerName, String directoryName, String filename)
        {
            Boolean result = false;
            CloudBlobContainer container = GetBlobContainer(containerName);

            filename = GetRelativeFilename(directoryName, filename);
            CloudBlob blob = container.GetBlobReference(filename);
            if (blob.Exists())
            {
                result = blob.SnapshotTime.HasValue;
            }

            return result;
        }

        public Boolean ContainsSnapshots(String containerName, String directoryName)
        {
            Boolean result = false;
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlobDirectory dir = container.GetDirectoryReference(directoryName);
            foreach (CloudBlob blob in dir.ListBlobs())
            {
                if (blob.SnapshotTime.HasValue)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes all the blobs in the container and then removes the container
        /// </summary>
        /// <param name="containerName"></param>
        public void Delete(String containerName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            if (container.Exists())
                container.Delete();
        }

        public void Delete(String containerName, String directoryName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            CloudBlobDirectory dir = container.GetDirectoryReference(directoryName);
            foreach (CloudBlob blob in dir.ListBlobs()) 
            {
                if ((blob.Exists()) && (!blob.SnapshotTime.HasValue))
                {
                    blob.DeleteIfExists();
                }
            }
        }

        public void Delete(String containerName, String directoryName, string filename)
        {
            filename = filename.Replace(" ", "");
            CloudBlobContainer container = GetBlobContainer(containerName);
            
            if (container.Exists())
            {
                filename = GetRelativeFilename(directoryName, filename);
                CloudBlob blob = container.GetBlobReference(filename);
                if (blob.Exists())
                {
                    if (!blob.SnapshotTime.HasValue)
                        blob.DeleteIfExists();
                }
            }
        }


        public Boolean Exists(String containerName)
        {
            CloudBlobContainer container = GetBlobContainer(containerName);
            return (container.Exists());
        }

        public Boolean Exists(String containerName, String directoryName, String filename)
        {
            Boolean result = false;

            CloudBlobContainer container = GetBlobContainer(containerName);
            if (container.Exists())
            {
                CloudBlob blob = GetCloudBlob(container, directoryName, filename);
                result = (blob.Exists());
            }

            return result;
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

        public void DownloadToStream(Stream target, String containerName, String directoryName, String filename)
        {
            filename = filename.Replace(" ", "");
            byte[] result = new byte[] { };

            CloudBlobContainer container = GetBlobContainer(containerName);
            if (container.Exists())
            {
                CloudBlob blob = GetCloudBlob(container, directoryName, filename);
                if (blob.Exists())
                    blob.DownloadToStream(target);
                else
                    throw new FileNotFoundException("The file does not exist at " + blob.Uri);
            }
            else
            {
                throw new DirectoryNotFoundException("The cloud container " + containerName + " does not exist");
            }
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
                    result.Uri = BuildUri(blob);
                    result.Filename = GetBlobFilename(blob);
                    result.Metadata = blob.Metadata;
                    result.Data = blob.DownloadByteArray();
                    result.LastModified = blob.Properties.LastModifiedUtc;
                }
            }

            return result;
        }

        //Renames a specific file
        public void Rename(String containerName, String directoryName, String filename, String renameTo, Permissions permissions)
        {
            if (ContainsSnapshots(containerName, directoryName, filename))
                throw new ArgumentException("Could not rename container=" + containerName + ", directory=" + directoryName + ", filename=" + filename + " because it contains snapshots.");

            byte[] data = this.Open(containerName, directoryName, filename);

            //Create the file with the new name
            this.Save(containerName, directoryName, renameTo, data, permissions);

            //Delete the old file
            this.Delete(containerName, directoryName, filename);
        }

        public void CopyFromSnapshots(IList<BlobSnapshot> snapshots, String copyFromContainerName, String copyToContainerName, String copyToDirectoryName, Permissions permissions)
        {
            CloudBlobContainer copyFromContainer = GetBlobContainer(copyFromContainerName);
            CloudBlobContainer copyToContainer = GetBlobContainer(copyToContainerName);

            Boolean containerCreated = copyToContainer.CreateIfNotExist(true);
            if (containerCreated)
            {
                BlobContainerPermissions perms = new BlobContainerPermissions();
                if (permissions == Permissions.Private)
                    perms.PublicAccess = BlobContainerPublicAccessType.Off;
                else if (permissions == Permissions.Public)
                    perms.PublicAccess = BlobContainerPublicAccessType.Blob;

                copyToContainer.SetPermissions(perms);
            }

            foreach (BlobSnapshot snapshot in snapshots)
            {
                String filename = snapshot.Filename.Replace(" ", "");

                String copyToBlobFileName = GetRelativeFilename(copyToDirectoryName, filename);
                //Get a reference to the snapshot itself
                String snapshotUri = snapshot.Uri + "?snapshot=" + snapshot.SnapshotTime.Value.ToString("o");

                CloudBlob copyToBlob = copyToContainer.GetBlobReference(copyToBlobFileName);
                CloudBlob snapshotBlob = copyFromContainer.GetBlobReference(snapshotUri);
                if (snapshotBlob.Exists())
                    copyToBlob.CopyFromBlob(snapshotBlob);
            }
        }

        public IList<BlobSnapshot> CreateSnapshot(String containerFolder, String directoryName)
        {
            BlobRequestOptions options = new BlobRequestOptions()
            {
                BlobListingDetails = BlobListingDetails.None,
            };

            IList<BlobSnapshot> snapshotNames = new List<BlobSnapshot>();

            ResultContinuation continuation = null;
            CloudBlobContainer container = GetBlobContainer(containerFolder);

            if (container.Exists())
            {
                do
                {
                    ResultSegment<IListBlobItem> blobPages;
                    if (directoryName != null)
                    {
                        CloudBlobDirectory directory = container.GetDirectoryReference(directoryName);
                        blobPages = directory.ListBlobsSegmented(0, continuation, options);
                    }
                    else
                        blobPages = container.ListBlobsSegmented(0, continuation, options);

                    continuation = blobPages.ContinuationToken;
                    foreach (IListBlobItem item in blobPages.Results)
                    {
                        if (item is CloudBlob)
                        {
                            CloudBlob blob = (CloudBlob)item;

                            //Create a snapshot of this blob
                            CloudBlob snapshot = blob.CreateSnapshot();

                            BlobSnapshot temp = new BlobSnapshot();
                            temp.SnapshotTime = snapshot.SnapshotTime;
                            temp.Uri = new Uri(snapshot.Attributes.Uri.AbsoluteUri);
                            temp.Filename = GetBlobFilename(snapshot);

                            snapshotNames.Add(temp);
                        }
                    }
                } while (continuation != null);
            }

            return snapshotNames;
        }

        private IEnumerable<IListBlobItem> GetCloudBlobs(CloudBlobContainer container, String directoryName)
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

            return items;
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
                IEnumerable<IListBlobItem> items = GetCloudBlobs(container, directoryName);

                if (items != null)
                {
                    foreach (IListBlobItem item in items)
                    {
                        if (item is CloudBlob)
                        {
                            CloudBlob blob = (CloudBlob)item;
                            if (blob.Exists())
                            {
                                results.Add(new StorageFile()
                                {
                                    Filename = GetBlobFilename(blob),
                                    Uri = BuildUri(blob),
                                    Metadata = blob.Metadata
                                });
                            }
                        }
                    }
                }
            }

            return results;
        }

        private Uri BuildUri(CloudBlob blob)
        {
            return BuildUri(blob.Uri);
        }

        private Uri BuildUri(CloudBlobContainer container)
        {
            return BuildUri(container.Uri);
        }

        private Uri BuildUri(Uri originalUri)
        {
            Boolean usehttps = this.GetClientOption<Boolean>(CloudStorageOptions.UseHttps);

            String scheme = (usehttps) ? "https" : "http";
            int port = (usehttps) ? 443 : 80;

            //Handle the development server
            if (originalUri.Port > 1000)
                port = originalUri.Port;

            UriBuilder builder = new UriBuilder(originalUri) { Scheme = scheme, Port = port };

            if (this.GetClientOption<Boolean>(CloudStorageOptions.UseCdn))
            {
                builder.Scheme = "http"; //force http for cdn access (required)
                builder.Host = GooeyConfigManager.AzureCdnHost;
                builder.Port = 80; //always use port 80 for the cdn
            }


            return builder.Uri;
        }

        public void CopyDirectory(String sourceContainer, String sourceDirectory, String destinationContainer, String destinationDirectory)
        {
            CloudBlobContainer sourceBlobContainer = GetBlobContainer(sourceContainer);
            CloudBlobContainer destinationBlobContainer = GetBlobContainer(destinationContainer);
            
            destinationBlobContainer.CreateIfNotExist(true);

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

        public StorageFile GetInfo(String url)
        {
            CloudBlob blob = this.GetBlobReference(url);

            StorageFile file = new StorageFile();
            if (blob.Exists())
            {
                file.Filename = GetBlobFilename(blob);
                file.Uri = BuildUri(blob);
                file.Metadata = blob.Metadata;
                file.Size = blob.Properties.Length;
                file.LastModified = blob.Properties.LastModifiedUtc;
            }

            return file;
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
                file.Uri = BuildUri(blob);
                file.Metadata = blob.Metadata;
                file.Size = blob.Properties.Length;
                file.LastModified = blob.Properties.LastModifiedUtc;
            }

            return file;
        }

        public StorageContainer GetContainerInfo(String name)
        {
            CloudBlobContainer container = GetBlobContainer(name);
            return new StorageContainer() { Uri = BuildUri(container) };
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
