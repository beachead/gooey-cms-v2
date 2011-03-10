using System;
using System.Collections.Generic;
using System.IO;

namespace Gooeycms.Business.Storage
{
    public enum Permissions
    {
        Private,
        Public
    }

    public static class CloudStorageOptions
    {
        public const String UseCdn = "use-cdn";
        public const String UseHttps = "use-https";
    }

    public static class StorageClientConst
    {
        public const String RootFolder = null;
        public const String RootContainerIdentifier = "$root";
    }

    public interface IStorageClient
    {
        void Save(String containerName, String directoryName, String filename, byte[] data, Permissions permissions);
        void Save(String containerName, String directoryName, String filename, String contents, Permissions permissions);

        void Rename(String container, String directory, String filename, String renameTo, Permissions permissions);
        void CopyDirectory(String sourceContainer, String sourceDirectory, String destinationContainer, String destinationDirectory);
        void CopyFromSnapshots(IList<BlobSnapshot> snapshots, String copyFromContainerName, String copyToContainerName, String copyToDirectoryName, Permissions permissions);
        IList<BlobSnapshot> CreateSnapshot(String containerName, String directoryName);

        void Delete(String containerName);
        void Delete(String containerName, String directoryName);
        void Delete(String containerName, String directoryName, String filename);

        Boolean ContainsSnapshots(String containerName, String directoryName, String filename);
        Boolean ContainsSnapshots(String containerName, String directoryName);
        void DeleteSnapshots(String containerName, String directoryName);

        Boolean Exists(String containerName);
        Boolean Exists(String containerName, String directoryName, String filename);

        byte[] Open(String containerName, String directoryName, String filename);
        String OpenAsString(String containerName, String directoryName, String filename);
        void DownloadToStream(Stream target, String containerName, String directoryName, String filename);
        void Read(String containerName, String directoryName, String filename, Stream stream);

        StorageFile GetFile(String containerName, String directoryName, String filename);
        StorageFile GetInfo(String containerName, String directoryName, String filename);
        StorageFile GetInfo(String url);
        StorageContainer GetContainerInfo(String container);

        IList<StorageFile> List(String containerName, String directoryName);

        void AddMetadata(String key, String value);
        void SetMetadata(String containerName, String directoryName, String filename);

        void AddClientOption<T>(String key, T item);
        T GetClientOption<T>(String key);
    }
}
