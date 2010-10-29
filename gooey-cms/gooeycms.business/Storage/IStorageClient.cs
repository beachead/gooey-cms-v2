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

    public static class StorageClientConst
    {
        public const String RootFolder = null;
    }

    public interface IStorageClient
    {
        void Save(String containerName, String directoryName, String filename, byte[] data, Permissions permissions);
        void Save(String containerName, String directoryName, String filename, String contents, Permissions permissions);
        
        void CopyDirectory(String sourceContainer, String sourceDirectory, String destinationContainer, String destinationDirectory);
        void CopyFromSnapshots(IList<BlobSnapshot> snapshots, String copyFromContainerName, String copyToContainerName, String copyToDirectoryName, Permissions permissions);
        IList<BlobSnapshot> CreateSnapshot(String containerName, String directoryName);

        void Delete(String containerName);
        void Delete(String containerName, String directoryName);
        void Delete(String containerName, String directoryName, String filename);

        Boolean ContainsSnapshots(String containerName, String directoryName);
        void DeleteSnapshots(String containerName, String directoryName);

        byte[] Open(String containerName, String directoryName, String filename);
        String OpenAsString(String containerName, String directoryName, String filename);
        void Read(String containerName, String directoryName, String filename, Stream stream);

        StorageFile GetFile(String containerName, String directoryName, String filename);
        StorageFile GetInfo(String containerName, String directoryName, String filename);
        StorageContainer GetContainerInfo(String container);

        IList<StorageFile> List(String containerName, String directoryName);

        void AddMetadata(String key, String value);
        void SetMetadata(String containerName, String directoryName, String filename);
    }
}
